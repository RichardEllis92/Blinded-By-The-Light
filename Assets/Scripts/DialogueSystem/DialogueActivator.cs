using UnityEngine;

public class DialogueActivator : MonoBehaviour, IInteractable
{
    [SerializeField] private DialogueObject dialogueObject;

    public void UpdateDialogueObject(DialogueObject dialogueObject)
    {
        this.dialogueObject = dialogueObject;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player") && other.TryGetComponent(out PlayerController playercontroller))
        {
            playercontroller.Interactable = this;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && other.TryGetComponent(out PlayerController playercontroller))
        {
            if(playercontroller.Interactable is DialogueActivator dialogueActivator && dialogueActivator == this)
            {
                playercontroller.Interactable = null;
            }
        }
    }
    public void Interact(PlayerController playercontroller)
    {
        foreach(DialogueResponseEvents responseEvents in GetComponents<DialogueResponseEvents>())
        {
            if(responseEvents.DialogueObject == dialogueObject)
            {
                playercontroller.DialogueUI.AddResponseEvents(responseEvents.Events);
                break;
            }
        }

        playercontroller.DialogueUI.ShowDialogue(dialogueObject);
    }
}
