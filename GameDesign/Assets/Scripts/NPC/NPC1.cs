using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;

public class NPC1 : MonoBehaviour, IInteractable, IDialogueNPC
{
    public NPCDialogue dialogueData;
    public NPCDialogue dialogueData1;
    public NPCDialogue dialogueData2;

    public NPCDialogue DialogueData => dialogueData;
    public int DialogueIndex => dialogueIndex;

    private int dialogueIndex;
    private bool isDialogueActive;
    public GameObject enemyToCheck;

    public bool CanInteract()
    {
        return !isDialogueActive;
    }

    public void Interact()
    {
        if (dialogueData == null || (PauseController.isGamePaused && !isDialogueActive))
            return;

        if (isDialogueActive)
        {
            NextLine();
        }
        else
        {
            StartDialogue();
        }
    }

    void StartDialogue()
    {
        isDialogueActive = true;
        dialogueIndex = 0;

        EnemyHealth enemyHealth = enemyToCheck != null ? enemyToCheck.GetComponent<EnemyHealth>() : null;

        if (enemyHealth == null || enemyHealth.IsDead)
        {
            dialogueData = dialogueData2;
        }
        else
        {
            dialogueData = dialogueData1;
        }

        DialogueManager.Instance.StartDialogue(this);
    }
    void NextLine()
    {
        if (DialogueManager.Instance.IsTyping)
        {
            DialogueManager.Instance.DisplayFullLine();
        }
        else if (++dialogueIndex < dialogueData.dialogLines.Length)
        {
            DialogueManager.Instance.StartDialogue(this);
        }
        else
        {
            EndDialogue();
        }
    }
    void IDialogueNPC.NextLine()
    {
        NextLine();
    }

    public void EndDialogue()
    {
        isDialogueActive = false;
    }
}