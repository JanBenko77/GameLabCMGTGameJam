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
        else if (currentLine is Flag checkpoint)
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

    private void CheckpointReached(Flag checkpoint)
    {
        // Check with HintManager if the required hints are collected
        if (HintManager.Instance.HasHint(checkpoint.requiredHints))
        {
            StartDialogue(checkpoint.continueDialogue);
        }
        else
        {
            DisplayNextDialogueLine();
        }
    }

    private void ChoiceCheckpointReached(ChoiceCheckpoint choiceCheckpoint)
    {
        dialogueBox.SetActive(false);
        choicesBox.SetActive(true);

        // Check which choices should be displayed based on collected hints
        choice1.SetActive(HintManager.Instance.HasHint(choiceCheckpoint.requiredHints1));
        choice2.SetActive(HintManager.Instance.HasHint(choiceCheckpoint.requiredHints2));
        choice3.SetActive(HintManager.Instance.HasHint(choiceCheckpoint.requiredHints3));
        choice4.SetActive(HintManager.Instance.HasHint(choiceCheckpoint.requiredHints4));

        choice1Text.text = choiceCheckpoint.choice1;
        choice2Text.text = choiceCheckpoint.choice2;
        choice3Text.text = choiceCheckpoint.choice3;
        choice4Text.text = choiceCheckpoint.choice4;

        choice1Text.GetComponentInParent<Button>().onClick.RemoveAllListeners();
        choice1Text.GetComponentInParent<Button>().onClick.AddListener(() => OnChoiceSelected(choiceCheckpoint.nextDialogue1));

        choice2Text.GetComponentInParent<Button>().onClick.RemoveAllListeners();
        choice2Text.GetComponentInParent<Button>().onClick.AddListener(() => OnChoiceSelected(choiceCheckpoint.nextDialogue2));

        choice3Text.GetComponentInParent<Button>().onClick.RemoveAllListeners();
        choice3Text.GetComponentInParent<Button>().onClick.AddListener(() => OnChoiceSelected(choiceCheckpoint.nextDialogue3));

        choice4Text.GetComponentInParent<Button>().onClick.RemoveAllListeners();
        choice4Text.GetComponentInParent<Button>().onClick.AddListener(() => OnChoiceSelected(choiceCheckpoint.nextDialogue4));
    }

    public void EndDialogue()
    {
        isDialogueActive = false;
        animator.SetTrigger("TrClose");
    }
}
