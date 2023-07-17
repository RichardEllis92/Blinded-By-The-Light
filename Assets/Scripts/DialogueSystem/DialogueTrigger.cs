using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public bool _startingRoomDialogue;
    public bool _dialogueTriggered;
    public bool gameObjectRequired;
    public new GameObject gameObject;

    [SerializeField] private DialogueObject dialogueObject;

    private static bool dialogueTriggered = false;
    private static HashSet<string> triggeredDialogueNames = new HashSet<string>();

    private void Start()
    {
        _dialogueTriggered = false;
        _dialogueTriggered = dialogueTriggered && triggeredDialogueNames.Contains(dialogueObject.dialogue[0]);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_dialogueTriggered) // Check if dialogue has already been triggered
            return;

        if (other.CompareTag("Player") && !_startingRoomDialogue)
        {
            if ((gameObjectRequired && gameObject.activeSelf) || !gameObjectRequired)
            {
                DialogueUI.Instance.ShowDialogue(dialogueObject);
                _dialogueTriggered = true;
                dialogueTriggered = true;
                triggeredDialogueNames.Add(dialogueObject.dialogue[0]);
            }
        }
        else if (other.CompareTag("Player") && _startingRoomDialogue)
        {
            if ((gameObjectRequired && gameObject.activeSelf) || !gameObjectRequired)
            {
                StartCoroutine(WaitForFadeIn());
                DialogueUI.Instance.ShowDialogue(dialogueObject);
                _dialogueTriggered = true;
                dialogueTriggered = true;
                triggeredDialogueNames.Add(dialogueObject.dialogue[0]);
            }
        }
    }

    private static IEnumerator WaitForFadeIn()
    {
        yield return new WaitForSeconds(1f);
    }
}