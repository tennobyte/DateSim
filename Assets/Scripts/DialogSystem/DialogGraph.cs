using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace Dialog.Editor {
    [Serializable]
    public class DialogGraph : ScriptableObject
    {
        public class DialogGraphAssetHandler {
            [OnOpenAsset(1)]
            public static bool OpenDialogGraphAsset(int instanceID, int line)
            {
                var assetObject = EditorUtility.InstanceIDToObject(instanceID);
                var assetPath = AssetDatabase.GetAssetPath(assetObject);
                if (assetPath != null && assetPath.EndsWith(".asset") && assetObject.GetType() == typeof(DialogGraph)) {
                    Debug.Log("Trying to open dialog graph");
                    LoadGraph(assetPath);
                    return true;
                }
                return false;
            }
        }

        public string Name = "New Graph";
        public List<BaseNode> Nodes;

        private void OnEnable()
        {
            if (Nodes == null) {
                Nodes = new List<BaseNode>();
            }
        }

        public void Initialize()
        {

        }

        public static void CreateDialogGraph(string name)
        {
            DialogGraph currentGraph = ScriptableObject.CreateInstance<DialogGraph>();
            if (currentGraph != null) {
                currentGraph.Name = name;
                currentGraph.Initialize();
                AssetDatabase.CreateAsset(currentGraph, @"Assets/Resources/Database/" + name + ".asset");
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                DialogEditor currentWindow = EditorWindow.GetWindow<DialogEditor>();
                if (currentWindow != null) {
                    currentWindow.CurrentGraph = currentGraph;
                } else {
                    ErrorMessage("Dialog editor window is lost, reopen it");
                }
            }
        }

        public static void LoadGraph()
        {
            string path = EditorUtility.OpenFilePanel("Load Graph", Application.dataPath + @"/Resources/Database/", "asset");
            LoadGraph(FileUtil.GetProjectRelativePath(path));
        }

        public static void LoadGraph(string path)
        {
            DialogGraph currentGraph;
            if (string.IsNullOrEmpty(path)) {
                return;
            }
            currentGraph = AssetDatabase.LoadAssetAtPath<DialogGraph>(path);
            if (currentGraph != null) {
                DialogEditor currentWindow = EditorWindow.GetWindow<DialogEditor>();
                if (currentWindow != null) {
                    currentWindow.CurrentGraph = currentGraph;
                } else {
                    ErrorMessage("Dialog editor window is lost, reopen it");
                }
            } else {
                ErrorMessage($"Failed to load graph {path}");
            }
        }

        private static void ErrorMessage(string body)
        {
            EditorUtility.DisplayDialog("Error", body, "OK");
        }

        public StartNode GetStartNode()
        {
            return (StartNode)Nodes.FirstOrDefault(n => n is StartNode);
        }
    }
}
