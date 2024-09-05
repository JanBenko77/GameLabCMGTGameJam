using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractForDialogue : InteractableObject
{
    [SerializeField] private DialogueSequencer dialogueSequencer;
    public override void OnInteract()
    {
        dialogueSequencer.StartSequencer();

    }
}
