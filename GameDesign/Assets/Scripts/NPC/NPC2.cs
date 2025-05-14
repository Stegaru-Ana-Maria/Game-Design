using UnityEngine;

public class NPC2 : MonoBehaviour, IInteractable, IDialogueNPC
{
    public NPCDialogue dialogueData;
    public NPCDialogue dialogueData1;
    public NPCDialogue dialogueData2;

    public NPCDialogue DialogueData => dialogueData;
    public int DialogueIndex => dialogueIndex;

    private int dialogueIndex;
    private bool isDialogueActive;
    [SerializeField] FirstBoosRoomDoorController door;
    [SerializeField] PlayerAttack player;


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

        if (player.rangedAttackUnlocked == true)
        {
            dialogueData = dialogueData2;
            door.OpenDoor();
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
