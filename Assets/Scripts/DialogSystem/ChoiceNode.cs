using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Dialog.Editor
{
    [Serializable]
    public class ChoiceNode : BaseNode
    {
        [SerializeField]
        public Choice currentChoice;
        [SerializeField]
        public List<NodeOutput> NodeOutputs;

        private int lineCount;

        public ChoiceNode()
        {
            currentChoice = new Choice();
            NodeInput = new NodeInput(this);
            NodeOutputs = new List<NodeOutput>();
        }

        public override void DrawWindow()
        {
            lineCount = 4;

            DrawInput();
            DrawQuestionText();
            DrawAnswersList();
            DrawOutput();
            EditorUtility.SetDirty(this);
        }

        private void DrawQuestionText()
        {
            GUILayout.Label("Question:");
            lineCount += 2;
            currentChoice.Question = EditorGUILayout.TextField(currentChoice.Question);
        }

        private void DrawAnswersList()
        {
            GUILayout.Label("Answers:");
            var answers = currentChoice.Answers;
            var removeAt = -1;
            for (int i = 0; i < answers.Count; i++) {
                lineCount += 2;
                GUILayout.BeginHorizontal();
                GUILayout.Label($"{i}:");
                currentChoice.Answers[i] = EditorGUILayout.TextField(currentChoice.Answers[i], GUILayout.Width(CalculateRect().width * 0.7f));
                if (GUILayout.Button("X", GUILayout.Width(25f), GUILayout.Height(15f))) {
                    removeAt = i;
                }
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                DrawOutput(i);
                GUILayout.EndHorizontal();
            }

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Add", GUILayout.Width(60f), GUILayout.Height(15f))) {
                currentChoice.Answers.Add("");
                NodeOutputs.Add(new NodeOutput(this));
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            if (removeAt > -1) {
                answers.RemoveAt(removeAt);
            }
        }

        protected void DrawOutput(int index)
        {
            if (GUILayout.Button("", GUILayout.Width(25f), GUILayout.Height(25f))) {
                DialogEditor.ConnectionNode = this;
                DialogEditor.MakeConnection = true;
                SelectedNodeOutput = NodeOutputs[index];
                SelectedNodeOutput.Occupied = false;
                SelectedNodeOutput.Clear();
            }
            if (Event.current.type == EventType.Repaint) {
                var lastRectangle = GUILayoutUtility.GetLastRect();
                NodeOutputs[index].Rectangle = new Rect(lastRectangle.x + WindowRect.x, lastRectangle.y + WindowRect.y, lastRectangle.width, lastRectangle.height);
            }
        }

        public override void DrawConnections()
        {
            foreach (var nodeOutput in NodeOutputs) {
                nodeOutput.DrawConnection();
            }
        }

        public override Rect CalculateRect()
        {
            return new Rect(WindowRect.position, new Vector2(WindowRect.width, lineCount * 23));
        }

        public override Color GetColor()
        {
            return new Color(0.9529f, 0.8745f, 0.8901f, 1f);
        }
    }
}
