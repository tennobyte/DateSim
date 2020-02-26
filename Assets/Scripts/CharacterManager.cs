using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    public static CharacterManager Instance;

    public RectTransform CharacterPanel;

    public Dictionary<Characters, Character> Characters = new Dictionary<Characters, Character>();
    public Dictionary<Character, GameObject> CharacterDictionary = new Dictionary<Character, GameObject>();

    private void Awake()
    {
        Instance = this;
    }

    public Character GetCharacter(Characters character, bool createCharacterIfDoesNotExist = true)
    {
        if (Characters.TryGetValue(character, out Character result)) {
            return result;
        } else {
            return CreateCharacter(character);
        }
    }

    public Character CreateCharacter(Characters character)
    {
        return null;
    }
}
