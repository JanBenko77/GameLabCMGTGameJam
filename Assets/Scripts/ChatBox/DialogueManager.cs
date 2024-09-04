using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    public bool hasHintTest = false;

    public void SetHintTest()
    {
        hasHintTest = !hasHintTest;
    }

    public GameObject choice1;
    public GameObject choice2;
    public GameObject choice3;
    public GameObject choice4;

    public Image characterIcon;
    public TextMeshProUGUI characterName;
    public TextMeshProUGUI dialogueArea;

    public GameObject dialogueBox;
    public GameObject choicesBox;
    public GameObject checkpointChoice;

    public TextMeshProUGUI choice1Text;
    public TextMeshProUGUI choice2Text;
    public TextMeshProUGUI choice3Text;
    public TextMeshProUGUI choice4Text;

    private bool isChoosing = false;

    private Queue<DialogueLine> lines;

    public bool isDialogueActive = false;

    public float typingSpeed = 0.2f;

    public Animator animator;

    private bool playAnimation = true;

    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void StartDialogue(Dialogue dialogue)
    {
        isDialogueActive = true;
        if (playAnimation)
        {
            animator.SetTrigger("TrOpen");
        }
        else
        {
            playAnimation = true;
        }

        lines = new Queue<DialogueLine>();

        foreach (DialogueLine line in dialogue.dialogueLines)
        {
            lines.Enqueue(line);
        }

        DisplayNextDialogueLine();
    }

    public void DisplayNextDialogueLine()
    {
        if (lines.Count == 0)
        {
            EndDialogue();
            return;
        }

        DialogueLine currentLine = lines.Dequeue();

        if (currentLine is Choice choice)
        {
            dialogueBox.SetActive(false);
            choicesBox.SetActive(true);
            checkpointChoice.SetActive(false);

            choice1Text.text = choice.choice1;
            choice2Text.text = choice.choice2;
            choice3Text.text = choice.choice3;
            choice4Text.text = choice.choice4;


            choice1Text.GetComponentInParent<Button>().onClick.RemoveAllListeners();
            choice1Text.GetComponentInParent<Button>().onClick.AddListener(() => OnChoiceSelected(choice.nextDialogue1));

            choice2Text.GetComponentInParent<Button>().onClick.RemoveAllListeners();
            choice2Text.GetComponentInParent<Button>().onClick.AddListener(() => OnChoiceSelected(choice.nextDialogue2));

            choice3Text.GetComponentInParent<Button>().onClick.RemoveAllListeners();
            choice3Text.GetComponentInParent<Button>().onClick.AddListener(() => OnChoiceSelected(choice.nextDialogue3));

            choice4Text.GetComponentInParent<Button>().onClick.RemoveAllListeners();
            choice4Text.GetComponentInParent<Button>().onClick.AddListener(() => OnChoiceSelected(choice.nextDialogue4));
        }
        else if (currentLine is Checkpoint checkpoint)
        {
            CheckpointReached(checkpoint);
        }
        else if (currentLine is ChoiceCheckpoint choiceCheckpoint)
        {
            ChoiceCheckpointReached(choiceCheckpoint);
        }
        else
        {
            dialogueBox.SetActive(true);
            choicesBox.SetActive(false);
            checkpointChoice.SetActive(false);

            characterIcon.sprite = currentLine.character.icon;
            characterName.text = currentLine.character.name;
            dialogueArea.text = "";

            StopAllCoroutines();
            StartCoroutine(TypeSentence(currentLine));
        }
    }

    IEnumerator TypeSentence(DialogueLine dialogueLine)
    {
        dialogueArea.text = "";

        foreach (char letter in dialogueLine.line.ToCharArray())
        {
            dialogueArea.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }

    public void OnChoiceSelected(Dialogue nextDialogue)
    {
        if (nextDialogue != null)
        {
            playAnimation = false;
            StartDialogue(nextDialogue);
        }
        else
        {
            EndDialogue();
        }
    }

    private void CheckpointReached(Checkpoint checkpoint)
    {
        //DialogueManager sends a request to the HintsManager to check if the checkpoint's required hints have been collected
        //if they have, the checkpoint will continue a different dialogue, if not, it will end the dialogue

        if (hasHintTest)
        {
            StartDialogue(checkpoint.continueDialogue);
        }
        else
        {
            EndDialogue();
        }

        //if (HintsManager.Instance.CheckHints(checkpoint.requiredHints))
        //{
        //    StartDialogue(checkpoint.continueDialogue);
        //}
        //else
        //{
        //    EndDialogue();
        //}
    }

    private void ChoiceCheckpointReached(ChoiceCheckpoint choiceCheckpoint)
    {
        //DialogueManager sends a request to the HintsManager to check which choices the panel will display, depending on the acquired hints
        //if the player has acquired a certain amount of hints, the panel will display different choices
        //for example, if the player has acquired hint1 and hint3, the panel will display choice1 and choice3


        dialogueBox.SetActive(false);
        choicesBox.SetActive(false);
        checkpointChoice.SetActive(true);

        //if (HintsManager.Instance.CheckHints(choiceCheckpoint.requiredHints))
        //{
        //    dialogueBox.SetActive(false);
        //    choicesBox.SetActive(true);
        //    choice1Text.text = choiceCheckpoint.choice1;
        //    choice2Text.text = choiceCheckpoint.choice2;
        //    choice3Text.text = choiceCheckpoint.choice3;
        //    choice4Text.text = choiceCheckpoint.choice4;
        //}
    }

    public void EndDialogue()
    {
        isDialogueActive = false;
        animator.SetTrigger("TrClose");
    }
}
