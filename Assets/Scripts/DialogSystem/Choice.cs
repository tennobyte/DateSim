using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Choice //: ScriptableObject
{
    public string Question;
    public List<string> Answers = new List<string>();
}
