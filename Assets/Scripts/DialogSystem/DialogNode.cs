using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Dialog;
using System;

namespace Dialog.Editor
{
    [Serializable]
    public class DialogNode : BaseNode
    {
        [SerializeField]
        public Dialog currentDialog;
        [SerializeField]
        public NodeOutput NodeOutput;

        private int lineCount;

        public DialogNode()
        {
            currentDialog = new Dialog();
            NodeInput = new NodeInput(this);
            NodeOutput = new NodeOutput(this);
            currentDialog.Init();
        }

        public override void DrawWindow()
        {
            lineCount = 6;

            DrawInput();

            DrawCharacterText();

            DrawCharacterAnimations();
            if (GUILayout.Button("Add Char Animation")) {
                AddCharacterAnimation();
            }

            DrawCharacterActions();
            if (GUILayout.Button("Add Char Action")) {
                AddCharacterAction();
            }
            DrawOutput();
            EditorUtility.SetDirty(this);
        }

        public override void DrawConnections()
        {
            NodeOutput.DrawConnection();
        }

        public override Rect CalculateRect()
        {
            return new Rect(WindowRect.position, new Vector2(WindowRect.width, lineCount * 20));
        }

        private void DrawCharacterText()
        {
            GUILayout.Label("Character:");
            var character = (Characters)EditorGUILayout.EnumPopup(currentDialog.CharacterText.Character);
            currentDialog.CharacterText.Character = character;
            lineCount += 2;
            if (character != Characters.None) {
                GUILayout.Label("Says:");
                currentDialog.CharacterText.Text = EditorGUILayout.TextField(currentDialog.CharacterText.Text);
                lineCount += 2;
            }
        }

        private void DrawCharacterAnimations()
        {
            int itemToDelete = -1;
            for (int i = 0; i < currentDialog.CharacterAnimations.Count; i++) {
                GUILayout.Label($"Animation {i}:");
                GUILayout.BeginHorizontal();
                currentDialog.CharacterAnimations[i].Character = (Characters)EditorGUILayout.EnumPopup(currentDialog.CharacterAnimations[i].Character);
                currentDialog.CharacterAnimations[i].Animation = (CharactersAnimations)EditorGUILayout.EnumPopup(currentDialog.CharacterAnimations[i].Animation);
                GUILayout.EndHorizontal();
                if (GUILayout.Button("Delete")) {
                    itemToDelete = i;
                }
                lineCount += 3;
            }
            if (itemToDelete != -1) {
                currentDialog.CharacterAnimations.RemoveAt(itemToDelete);
            }
        }

        private void DrawCharacterActions()
        {
            int itemToDelete = -1;
            for (int i = 0; i < currentDialog.CharactertActions.Count; i++) {
                GUILayout.Label($"Action {i}:");
                GUILayout.BeginHorizontal();
                currentDialog.CharactertActions[i].Character = (Characters)EditorGUILayout.EnumPopup(currentDialog.CharactertActions[i].Character);
                currentDialog.CharactertActions[i].Action = (CharactersActions)EditorGUILayout.EnumPopup(currentDialog.CharactertActions[i].Action);
                currentDialog.CharactertActions[i].Slot = EditorGUILayout.IntField(currentDialog.CharactertActions[i].Slot);
                GUILayout.EndHorizontal();
                if (GUILayout.Button("Delete")) {
                    itemToDelete = i;
                }
                lineCount += 4;
            }
            if (itemToDelete != -1) {
                currentDialog.CharactertActions.RemoveAt(itemToDelete);
            }
        }

        private void AddCharacterAnimation()
        {
            currentDialog.CharacterAnimations.Add(new CharacterAnimation());
        }

        private void AddCharacterAction()
        {
            currentDialog.CharactertActions.Add(new CharacterInSlotAction());
        }

        protected override void DrawOutput()
        {
            GUILayout.FlexibleSpace();
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("", GUILayout.Width(25f), GUILayout.Height(25f))) {
                DialogEditor.ConnectionNode = this;
                DialogEditor.MakeConnection = true;
                SelectedNodeOutput = NodeOutput;
                NodeOutput.Occupied = false;
                NodeOutput.Clear();
            }
            if (Event.current.type == EventType.Repaint) {
                var lastRectangle = GUILayoutUtility.GetLastRect();
                NodeOutput.Rectangle = new Rect(lastRectangle.x + WindowRect.x, lastRectangle.y + WindowRect.y, lastRectangle.width, lastRectangle.height);
            }
            //Debug.Log(NodeOutput.Rectangle);
            EditorGUILayout.EndHorizontal();
        }

        public override Color GetColor()
        {
            return new Color(0.8823f, 0.949f, 0.9843f, 1f);
        }
    }
}
