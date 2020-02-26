using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    public DialogSystem DialogSystem;

    public string[] s = new string[] {
        "Allou! Yoba, eto ti?",
        "Pshpshsh, da eto ya. Ti opyat vihodish na svyaz, mudilo?",
        "Narrator obosralsya",
        "Da, vihozhu",
        "Pizda"
    };

    public string[] speakers = new string[] {
        "Mudilo",
        "Yoba",
        null,
        "Mudilo",
        "Yoba"
    };


    // Start is called before the first frame update
    void Start()
    {
        DialogSystem = DialogSystem.Instance;
    }

    int index = 0;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) {
            if (!DialogSystem.IsSpeaking || DialogSystem.IsWaitingForUserInput) {
                if (index >= s.Length) {
                    return;
                }

                DialogSystem.Say(s[index], speakers[index]);
                index++;
            }
        }
    }
}
