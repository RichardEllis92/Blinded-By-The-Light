using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    private bool _startingRoomDialogue;
    private const float WaitTime = 1f;
    private bool _dialogueTriggered;
    public bool gameObjectRequired;
    public new GameObject gameObject;

    [SerializeField] private DialogueObject dialogueObject;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !_startingRoomDialogue && !_dialogueTriggered)
        {
            if ((gameObjectRequired && gameObject.activeSelf) || !gameObjectRequired)
            {
                DialogueUI.Instance.ShowDialogue(dialogueObject);
                _dialogueTriggered = true;
            }
        }
        else if (other.CompareTag("Player") && _startingRoomDialogue && !_dialogueTriggered)
        {
            if ((gameObjectRequired && gameObject.activeSelf) || !gameObjectRequired)
            {
                StartCoroutine(WaitForFadeIn());
                DialogueUI.Instance.ShowDialogue(dialogueObject);
                _dialogueTriggered = true;
            }
        }
    }

    private static IEnumerator WaitForFadeIn()
    {
        yield return new WaitForSeconds(WaitTime);
    }

}
