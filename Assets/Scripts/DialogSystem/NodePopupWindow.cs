using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Dialog.Editor {
    public class NodePopupWindow : EditorWindow
    {
        private static NodePopupWindow Instance;
        private string graphName = "Enter a name ...";

        public static void Init()
        {
            Instance = GetWindow<NodePopupWindow>();
            Instance.titleContent = new GUIContent("Graph Name");
            Instance.maxSize = new Vector2(300, 80);
            Instance.minSize = Instance.maxSize;
        }

        public void OnGUI()
        {
            GUILayout.Space(20);
            GUILayout.BeginHorizontal();
            GUILayout.Space(20);
            EditorGUILayout.LabelField("New Graph", EditorStyles.boldLabel, GUILayout.Width(80));
            graphName = EditorGUILayout.TextField(graphName);
            GUILayout.Space(20);
            GUILayout.EndHorizontal();

            GUILayout.Space(6);
            GUILayout.BeginHorizontal();
            GUILayout.Space(20);
            if (GUILayout.Button("Create")) {
                if (!string.IsNullOrEmpty(graphName) && !graphName.Equals("Enter a name ...")) {
                    DialogGraph.CreateDialogGraph(graphName);
                    Instance.Close();
                } else {
                    EditorUtility.DisplayDialog("Error", "Enter a valid name", "OK");
                }
            }
            GUILayout.Space(10);
            if (GUILayout.Button("Cancel")) {
                Instance.Close();
            }
            GUILayout.Space(20);
            GUILayout.EndHorizontal();
            GUILayout.Space(20);
            Repaint();
        }
    }
}
