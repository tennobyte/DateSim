using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Dialog.Editor
{
    [Serializable]
    public abstract class NodeConnection
    {
        public Rect Rectangle;
        public bool Occupied;

        public BaseNode Parent;

        public NodeConnection(BaseNode parent)
        {
            Parent = parent;
        }

        public virtual void Clear()
        {
            Occupied = false;
        }
    }

    [Serializable]
    public class NodeInput : NodeConnection
    {
        public List<BaseNode> ConnectedNodes;

        public NodeInput(BaseNode parent) : base(parent)
        {
            ConnectedNodes = new List<BaseNode>();
        }

        public override void Clear()
        {
            base.Clear();
            ConnectedNodes.Clear();
        }
    }

    [Serializable]
    public class NodeOutput : NodeConnection
    {
        [SerializeField]
        public BaseNode ConnectedNode;
        //public NodeInput ConnectedNodeInput;

        public NodeOutput(BaseNode parent) : base(parent)
        {

        }

        public override void Clear()
        {
            base.Clear();
            //if (ConnectedNodeInput != null && ConnectedNodeInput.ConnectedNodes != null) {
            //    ConnectedNodeInput.ConnectedNodes.Remove(Parent);
            //}
            //Occupied = false;
            //ConnectedNodeInput = null;
            ConnectedNode = null;
        }

        public void DrawConnection()
        {
            //if (ConnectedNodeInput != null && ConnectedNodeInput.Parent != null && ConnectedNodeInput.Parent.IsDeleted) {
            //    Clear();
            //}
            //if (Occupied && ConnectedNodeInput != null && ConnectedNodeInput.Parent != null) {
            //    Handles.BeginGUI();
            //    Handles.color = Color.black;
            //    Handles.DrawLine(
            //        new Vector3(Rectangle.position.x + 12f, Rectangle.y + 12f),
            //        new Vector3(ConnectedNodeInput.Rectangle.position.x + 12f, ConnectedNodeInput.Rectangle.position.y + 12f)
            //    );
            //    Handles.EndGUI();
            //}
            if (ConnectedNode != null && ConnectedNode.IsDeleted) {
                Clear();
            }
            if (Occupied && ConnectedNode != null) {
                Handles.BeginGUI();
                Handles.color = Color.black;
                Handles.DrawLine(
                    new Vector3(Rectangle.position.x + 12f, Rectangle.y + 12f),
                    new Vector3(ConnectedNode.NodeInput.Rectangle.position.x + 12f, ConnectedNode.NodeInput.Rectangle.position.y + 12f)
                );
                Handles.EndGUI();
            }
        }
    }

    [Serializable]
    public abstract class BaseNode : ScriptableObject
    {
        public Rect WindowRect;
        public string WindowTitle;
        public NodeOutput SelectedNodeOutput;
        public NodeInput NodeInput;

        public bool IsDeleted;

        public virtual void DrawWindow()
        {

        }

        public virtual void DrawConnections()
        {

        }

        public virtual Rect CalculateRect()
        {
            return WindowRect;
        }

        public virtual Rect GetSelectedConnectionRect()
        {
            return WindowRect;
        }

        public virtual Color GetColor()
        {
            return Color.white;
        }

        protected virtual void DrawInput()
        {
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("", GUILayout.Width(25f), GUILayout.Height(25f))) {
                if (DialogEditor.MakeConnection && DialogEditor.ConnectionNode != null && DialogEditor.ConnectionNode != this) {
                    NodeInput.Occupied = true;
                    NodeInput.ConnectedNodes.Add(DialogEditor.ConnectionNode); // do i need this at all?
                    DialogEditor.ConnectionNode.SelectedNodeOutput.Occupied = true;
                    DialogEditor.ConnectionNode.SelectedNodeOutput.ConnectedNode = this;

                    DialogEditor.MakeConnection = false;
                    DialogEditor.ConnectionNode = null;
                }
            }
            //GUILayout.FlexibleSpace();
            if (Event.current.type == EventType.Repaint) {
                var laseRectangle = GUILayoutUtility.GetLastRect();
                NodeInput.Rectangle = new Rect(laseRectangle.x + WindowRect.x, laseRectangle.y + WindowRect.y, laseRectangle.width, laseRectangle.height);
            }
            EditorGUILayout.EndHorizontal();
        }

        protected virtual void DrawOutput()
        {

        }
    }
}
