using System.Collections;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    private bool _startingRoomDialogue;
    private bool _dialogueTriggered;
    public bool gameObjectRequired;
    public new GameObject gameObject;

    [SerializeField] private DialogueObject dialogueObject;

    private static bool sessionDialogueTriggered = false;

    private void Start()
    {
        _dialogueTriggered = sessionDialogueTriggered;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !_startingRoomDialogue && !_dialogueTriggered)
        {
            if ((gameObjectRequired && gameObject.activeSelf) || !gameObjectRequired)
            {
                DialogueUI.Instance.ShowDialogue(dialogueObject);
                _dialogueTriggered = true;
                sessionDialogueTriggered = true;
            }
        }
        else if (other.CompareTag("Player") && _startingRoomDialogue && !_dialogueTriggered)
        {
            if ((gameObjectRequired && gameObject.activeSelf) || !gameObjectRequired)
            {
                StartCoroutine(WaitForFadeIn());
                DialogueUI.Instance.ShowDialogue(dialogueObject);
                _dialogueTriggered = true;
                sessionDialogueTriggered = true;
            }
        }
    }

    private static IEnumerator WaitForFadeIn()
    {
        yield return new WaitForSeconds(1f);
    }
}
