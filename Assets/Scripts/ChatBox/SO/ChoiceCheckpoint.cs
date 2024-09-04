using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoiceCheckpoint : DialogueLine
{
    //public List<Hint> requiredHints;

    //Depending on the amount of acquired hints, the player will be able to have different conversations

    public Dialogue checkpointDialogue;

    public string choice1;
    public string choice2;
    public string choice3;
    public string choice4;

    public Dialogue nextDialogue1;
    public Dialogue nextDialogue2;
    public Dialogue nextDialogue3;
    public Dialogue nextDialogue4;
}