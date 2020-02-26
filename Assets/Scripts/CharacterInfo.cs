using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Custom Assets/CharacterInfo")]
[Serializable]
public class CharacterInfo : ScriptableObject
{
    [SerializeField]
    public string Name;
    [SerializeField]
    public AnimationsSpritesDictionary Animations = AnimationsSpritesDictionary.New<AnimationsSpritesDictionary>();
}