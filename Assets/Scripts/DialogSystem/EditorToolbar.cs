using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Dialog.Editor
{
    public class EditorToolbar
    {
        public static string Title;

        public static void Draw(Rect viewRect)
        {
            GUI.Box(viewRect, "");
            
            
            GUILayout.BeginArea(viewRect);
            {
                GUILayout.BeginHorizontal();
                GUILayout.Box("", GUILayout.Height(viewRect.height), GUILayout.Width(2));
                if (GUILayout.Button("NEW", GUILayout.Height(viewRect.height), GUILayout.Width(viewRect.height))) {
                    NodePopupWindow.Init();
                }
                if (GUILayout.Button("OPEN", GUILayout.Height(viewRect.height), GUILayout.Width(viewRect.height))) {
                    DialogGraph.LoadGraph();
                }
                if (GUILayout.Button("RESET POSITION", GUILayout.Height(viewRect.height), GUILayout.Width(viewRect.height * 3))) {
                    DialogEditor.Instance.TryResetViewToStartNode();
                }
                if (GUILayout.Button("SAVE", GUILayout.Height(viewRect.height), GUILayout.Width(viewRect.height * 3))) {
                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();
                }
                GUILayout.FlexibleSpace();
                GUILayout.Box(Title, GUILayout.Height(viewRect.height), GUILayout.Width(viewRect.height * 4));
                GUILayout.EndHorizontal();
            }
            GUILayout.EndArea();
        }
    }
}
