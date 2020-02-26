using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dialog {
    [Serializable]
    public class CharacterInSlotAction
    {
        public Characters Character;
        public int Slot;
        public CharactersActions Action;
    }

    [Serializable]
    public class CharacterAnimation
    {
        public Characters Character;
        public CharactersAnimations Animation;
    }

    [Serializable]
    public class CharacterText
    {
        [SerializeField]
        public Characters Character;
        [SerializeField]
        public string Text;
    }

    //[CreateAssetMenu]
    [Serializable]
    public class Dialog //: ScriptableObject
    {
        [SerializeField]
        public List<CharacterInSlotAction> CharactertActions;
        [SerializeField]
        public List<CharacterAnimation> CharacterAnimations;
        [SerializeField]
        public CharacterText CharacterText;

        public void Init()
        {
            CharacterText = new CharacterText();
            CharacterAnimations = new List<CharacterAnimation>();
            CharactertActions = new List<CharacterInSlotAction>();
        }
    }
}
