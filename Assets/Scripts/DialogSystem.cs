using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogSystem : MonoBehaviour
{
    public static DialogSystem Instance;
    public Text SpeechText;
    public Text SpeakerNameText;
    public GameObject SpeechPanel;
    public GameObject SpeakerNameBox;

    public bool IsWaitingForUserInput { get; private set; }

    private Coroutine speakingCoroutine;

    public bool IsSpeaking => speakingCoroutine != null;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Say(string speech, string speaker = "")
    {
        StopSpeaking();
        speakingCoroutine = StartCoroutine(Speaking(speech, speaker)); 
    }

    public void StopSpeaking()
    {
        if (IsSpeaking) {
            StopCoroutine(speakingCoroutine);
        }
        speakingCoroutine = null;
    }

    public IEnumerator Speaking(string targetSpeech, string speaker)
    {
        SpeechPanel.SetActive(true);
        SpeechText.text = "";
        if (speaker.IsNullOrEmpty()) {
            SpeakerNameBox.SetActive(false);
        } else {
            if (!SpeakerNameBox.activeInHierarchy) {
                SpeakerNameBox.SetActive(true);
            }
            SpeakerNameText.text = speaker;
        }
        IsWaitingForUserInput = false;

        while (SpeechText.text.Length < targetSpeech.Length) {
            SpeechText.text += targetSpeech[SpeechText.text.Length];
            yield return new WaitForEndOfFrame();
        }
        IsWaitingForUserInput = true;

        while (IsWaitingForUserInput) {
            yield return new WaitForEndOfFrame();
        }

        StopSpeaking();
    }
}
