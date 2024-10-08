using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Character", menuName = "Character")]
public class DialogueCharacter : ScriptableObject
{
    public string characterName;
    public Sprite icon;
}
