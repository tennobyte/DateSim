using Dialog.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogController : MonoBehaviour
{
    public DialogSystem DialogSystem;
    public DialogGraph DialogGraph;

    private BaseNode currentNode;

    // Start is called before the first frame update
    void Start()
    {
        DialogSystem = DialogSystem.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) {
            if (!DialogSystem.IsSpeaking || DialogSystem.IsWaitingForUserInput) {
                currentNode = GetNextNode();
                if (currentNode != null) {
                    ProcessCurrentNode();
                }
            }
        }
    }

    private BaseNode GetNextNode()
    {
        if (currentNode == null && DialogGraph != null) {
            currentNode = DialogGraph.GetStartNode();
        }
        if (currentNode != null) {
            switch (currentNode.GetType()) {
                case var type when type == typeof(StartNode):
                    var startNode = (StartNode)currentNode;
                    return startNode.NodeOutput?.ConnectedNode;
                case var type when type == typeof(DialogNode):
                    var dialogNode = (DialogNode)currentNode;
                    return dialogNode.NodeOutput?.ConnectedNode;
                case var type when type == typeof(ChoiceNode):
                    break;
                case var type when type == typeof(BranchNode):
                    break;
                default:
                    break;
            }
        }
        return null;
    }

    private void ProcessCurrentNode()
    {
        switch (currentNode.GetType()) {
            case var type when type == typeof(StartNode):
                StartNode startNode = (StartNode)currentNode;
                break;
            case var type when type == typeof(DialogNode):
                DialogNode dialogNode = (DialogNode)currentNode;
                string speech = dialogNode.currentDialog.CharacterText.Text;
                var character = dialogNode.currentDialog.CharacterText.Character;
                string speaker = character == Characters.Narrator ? "" : character.ToString();
                DialogSystem.Say(speech, speaker);
                break;
            case var type when type == typeof(ChoiceNode):
                break;
            case var type when type == typeof(BranchNode):
                break;
            default:
                break;
        }
    }
}
