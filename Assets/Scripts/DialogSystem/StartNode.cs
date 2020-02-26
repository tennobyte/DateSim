using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Dialog;
using System;

namespace Dialog.Editor
{
    [Serializable]
    public class StartNode : BaseNode
    {
        [SerializeField]
        public NodeOutput NodeOutput;

        public StartNode()
        {
            NodeInput = null;
            NodeOutput = new NodeOutput(this);
        }

        public override void DrawWindow()
        {
            DrawOutput();
            EditorUtility.SetDirty(this);
        }

        public override void DrawConnections()
        {
            NodeOutput.DrawConnection();
        }

        protected override void DrawInput()
        {
            return; //no input for this node
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
            EditorGUILayout.EndHorizontal();
        }

        public override Color GetColor()
        {
            return new Color(1f, 0.698f, 0.7372f, 1f);
        }
    }
}
