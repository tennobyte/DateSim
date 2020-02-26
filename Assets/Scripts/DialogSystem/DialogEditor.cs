using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Linq;

namespace Dialog.Editor
{
    public class EditorZoomArea
    {
        private const float kEditorWindowTabHeight = 21.0f;
        private static Matrix4x4 _prevGuiMatrix;

        public static Rect Begin(float zoomScale, Rect screenCoordsArea)
        {
            GUI.EndGroup();        // End the group Unity begins automatically for an EditorWindow to clip out the window tab. This allows us to draw outside of the size of the EditorWindow.

            Rect clippedArea = screenCoordsArea.ScaleSizeBy(1.0f / zoomScale, screenCoordsArea.TopLeft());
            clippedArea.y += kEditorWindowTabHeight;
            GUI.BeginGroup(clippedArea);

            _prevGuiMatrix = GUI.matrix;
            Matrix4x4 translation = Matrix4x4.TRS(clippedArea.TopLeft(), Quaternion.identity, Vector3.one);
            Matrix4x4 scale = Matrix4x4.Scale(new Vector3(zoomScale, zoomScale, 1.0f));
            GUI.matrix = translation * scale * translation.inverse * GUI.matrix;

            return clippedArea;
        }

        public static void End()
        {
            GUI.matrix = _prevGuiMatrix;
            GUI.EndGroup();
            GUI.BeginGroup(new Rect(0.0f, kEditorWindowTabHeight, Screen.width, Screen.height));
        }
    }

    public class DialogEditor : EditorWindow
    {
        private const float kZoomMin = 0.5f;
        private const float kZoomMax = 1f;
        private const float gridSpacing = 50f;
        private float _zoom = 1.0f;
        private Vector2 _zoomCoordsOrigin = Vector2.zero;
        private Vector3 mousePosition;
        private bool clickedOnWindow;
        private BaseNode selectedNode;

        public static DialogEditor Instance;
        public static bool MakeConnection;
        public static BaseNode ConnectionNode;
        public DialogGraph CurrentGraph;

        public enum UserActions
        {
            AddDialogNode,
            AddChoiceNode,
            AddBranchNode,
            AddStartNode,
            DeleteNode
        }


        [MenuItem("Dialog Editor/Editor")]
        private static void ShowEditor()
        {
            DialogEditor editor = GetWindow<DialogEditor>();
            Instance = editor;
            editor.minSize = new Vector2(800, 600);
        }

        public static void OpenEditor()
        {
            if (Instance == null) {
                ShowEditor();
            }
        }

        private void OnGUI()
        {
            DrawGrid();
            if (CurrentGraph != null) {
                EditorToolbar.Title = CurrentGraph.Name;
            }
            EditorZoomArea.Begin(_zoom, new Rect(0f, 0f, position.width, position.height));
            Event e = Event.current;
            mousePosition = e.mousePosition;
            UserInput(e);
            DrawWindows();
            if (MakeConnection && ConnectionNode != null) {
                //Repaint();
                DrawConnectionToMouse(mousePosition);
            }
            if (CurrentGraph != null) {
                for (int i = 0; i < CurrentGraph.Nodes.Count; i++) {
                    CurrentGraph.Nodes[i].WindowRect = new Rect(CurrentGraph.Nodes[i].WindowRect.position + _zoomCoordsOrigin, CurrentGraph.Nodes[i].WindowRect.size);
                }
            }
            EditorZoomArea.End();
            EditorToolbar.Draw(new Rect(0f, 0f, position.width, 50f));
        }

        private void Update()
        {
            Repaint();
        }

        private void OnEnable()
        {
            if (Instance == null) {
                Instance = this;
            }
        }

        private void DrawWindows()
        {
            if (CurrentGraph != null) {
                BeginWindows();
                for (int i = 0; i < CurrentGraph.Nodes.Count; i++) {
                    var rect = CurrentGraph.Nodes[i].CalculateRect();
                    var color = GUI.backgroundColor;
                    GUI.backgroundColor = CurrentGraph.Nodes[i].GetColor();
                    CurrentGraph.Nodes[i].WindowRect = GUI.Window(i, new Rect(rect.position - _zoomCoordsOrigin, rect.size), DrawNodeWindow, CurrentGraph.Nodes[i].WindowTitle);
                    GUI.backgroundColor = color;
                }
                EndWindows();
                for (int i = 0; i < CurrentGraph.Nodes.Count; i++) {
                    CurrentGraph.Nodes[i].DrawConnections();
                }
            }
        }

        private void DrawNodeWindow(int id)
        {
            CurrentGraph.Nodes[id].DrawWindow();
            GUI.DragWindow();
        }

        private void UserInput(Event e)
        {
            if (e.button == 1) {
                if (e.type == EventType.MouseDown) {
                    RightClick(e);
                }
            }

            if (e.button == 0) {
                if (e.type == EventType.MouseDown) {
                    LeftClick(e);
                }
            }

            if (e.type == EventType.MouseDrag && Event.current.button == 2) {
                Vector2 delta = Event.current.delta;
                _zoomCoordsOrigin -= delta;

                Event.current.Use();
            }

            if (Event.current.type == EventType.ScrollWheel) {
                Vector2 screenCoordsMousePos = Event.current.mousePosition;
                Vector2 delta = Event.current.delta;
                Vector2 zoomCoordsMousePos = ConvertScreenCoordsToZoomCoords(screenCoordsMousePos);
                float zoomDelta = -delta.y / 100.0f;
                float oldZoom = _zoom;
                _zoom += zoomDelta;
                _zoom = Mathf.Clamp(_zoom, kZoomMin, kZoomMax);
                _zoomCoordsOrigin += (zoomCoordsMousePos - _zoomCoordsOrigin) - (oldZoom / _zoom) * (zoomCoordsMousePos - _zoomCoordsOrigin);

                Event.current.Use();
            }
        }

        private Vector2 ConvertScreenCoordsToZoomCoords(Vector2 screenCoords)
        {
            return (screenCoords - new Rect(0f, 0f, position.width, position.height).TopLeft()) / _zoom + _zoomCoordsOrigin;
        }

        private void LeftClick(Event e)
        {
            clickedOnWindow = false;
            selectedNode = null;
            if (CurrentGraph != null) {
                for (int i = 0; i < CurrentGraph.Nodes.Count; i++) {
                    var rect = CurrentGraph.Nodes[i].WindowRect;
                    if (new Rect(rect.position - _zoomCoordsOrigin, rect.size).Contains(e.mousePosition)) {
                        clickedOnWindow = true;
                        selectedNode = CurrentGraph.Nodes[i];
                        break;
                    }
                }
                if (MakeConnection && !clickedOnWindow) {
                    MakeConnection = false;
                    ConnectionNode = null;
                }
            }
        }

        private void RightClick(Event e)
        {
            if (MakeConnection) {
                MakeConnection = false;
                ConnectionNode = null;
            }

            clickedOnWindow = false;
            selectedNode = null;
            if (CurrentGraph != null) {
                for (int i = 0; i < CurrentGraph.Nodes.Count; i++) {
                    var rect = CurrentGraph.Nodes[i].WindowRect;
                    if (new Rect(rect.position - _zoomCoordsOrigin, rect.size).Contains(e.mousePosition)) {
                        clickedOnWindow = true;
                        selectedNode = CurrentGraph.Nodes[i];
                        break;
                    }
                }
            }

            if (!clickedOnWindow) {
                AddNewNode(e);
            } else {
                ModifyNode(e);
            }
        }

        private void AddNewNode(Event e)
        {
            GenericMenu menu = new GenericMenu();
            menu.AddSeparator("");
            menu.AddItem(new GUIContent("Add Dialog node"), false, ContextCallback, UserActions.AddDialogNode);
            menu.AddItem(new GUIContent("Add Choice node"), false, ContextCallback, UserActions.AddChoiceNode);
            menu.AddItem(new GUIContent("Add Branch node"), false, ContextCallback, UserActions.AddBranchNode);
            if (!CurrentGraph.Nodes.Any(n => n is StartNode)) {
                menu.AddItem(new GUIContent("Add Start node"), false, ContextCallback, UserActions.AddStartNode);
            }
            menu.ShowAsContext();
            e.Use();
        }

        private void ModifyNode(Event e)
        {
            GenericMenu menu = new GenericMenu();
            switch (selectedNode) {
                case DialogNode dialogNode:
                case ChoiceNode choiceNode:
                case BranchNode branchNode:
                    menu.AddSeparator("");
                    menu.AddItem(new GUIContent("Delete node"), false, ContextCallback, UserActions.DeleteNode);
                    break;
            }
            menu.ShowAsContext();
            e.Use();
        }

        private void ContextCallback(object o)
        {
            UserActions a = (UserActions)o;
            BaseNode newNode = null;
            switch (a) {
                case UserActions.AddDialogNode:
                    newNode = ScriptableObject.CreateInstance<DialogNode>();
                    newNode.WindowTitle = "Dialog";
                    break;
                case UserActions.AddChoiceNode:
                    newNode = ScriptableObject.CreateInstance<ChoiceNode>();
                    newNode.WindowTitle = "Choice";
                    break;
                case UserActions.AddBranchNode:
                    newNode = ScriptableObject.CreateInstance<BranchNode>();
                    newNode.WindowTitle = "Branch";
                    break;
                case UserActions.AddStartNode:
                    newNode = ScriptableObject.CreateInstance<StartNode>();
                    newNode.WindowTitle = "Start";
                    break;
                case UserActions.DeleteNode:
                    if (selectedNode != null) {
                        CurrentGraph.Nodes.Remove(selectedNode);
                        selectedNode.IsDeleted = true;
                        AssetDatabase.RemoveObjectFromAsset(selectedNode);
                        //AssetDatabase.SaveAssets();
                        //AssetDatabase.Refresh();
                        selectedNode = null;
                    }
                    break;
                default:
                    break;
            }
            if (newNode != null) {
                newNode.WindowRect = new Rect(mousePosition.x + _zoomCoordsOrigin.x, mousePosition.y + _zoomCoordsOrigin.y, 200, 100);
                CurrentGraph.Nodes.Add(newNode);
                AssetDatabase.AddObjectToAsset(newNode, CurrentGraph);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
        }

        private void DrawConnectionToMouse(Vector2 mousePosition)
        {
            Handles.BeginGUI();
            Handles.color = Color.white;
            var outputRectangle = ConnectionNode.SelectedNodeOutput.Rectangle;
            Handles.DrawLine(
                new Vector3(outputRectangle.position.x + 12f, outputRectangle.y + 12f), 
                new Vector3(mousePosition.x, mousePosition.y)
            );
            Handles.EndGUI();
        }

        public void TryResetViewToStartNode()
        {
            var startNode = CurrentGraph.Nodes.FirstOrDefault(n => n is StartNode);
            if (startNode != null) {
                var startNodePos = startNode.WindowRect.position;
                //_zoomCoordsOrigin = new Vector2(startNodePos.x - position.width - position.x, startNodePos.y - position.height - position.y);
                _zoomCoordsOrigin = new Vector2(startNodePos.x - (position.width / 2) * (1 / _zoom), startNodePos.y - (position.height / 2) *  (1 / _zoom));
                Debug.Log(position);
            }
        }

        private void DrawGrid()
        {
            var spacing = gridSpacing * _zoom;
            var gridOffset = new Vector2(spacing - (_zoomCoordsOrigin.x * _zoom) % spacing, spacing - (_zoomCoordsOrigin.y * _zoom) % spacing);
            Toolbox.DrawGrid(new Rect(position.position, position.size), spacing, 0.15f, Color.black, gridOffset);
        }
    }
}
