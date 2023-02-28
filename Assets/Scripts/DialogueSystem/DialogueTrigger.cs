using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    bool startingRoomDialogue;
    float waitTime = 1f;
    bool dialogueTriggered;
    public bool gameObjectRequired;
    public GameObject gameObject;

    [SerializeField] private DialogueObject DialogueObject;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !startingRoomDialogue && !dialogueTriggered)
        {
            if ((gameObjectRequired && gameObject.activeSelf) || !gameObjectRequired)
            {
                DialogueUI.instance.ShowDialogue(DialogueObject);
                dialogueTriggered = true;
            }
        }
        else if (other.CompareTag("Player") && startingRoomDialogue && !dialogueTriggered)
        {
            if ((gameObjectRequired && gameObject.activeSelf) || !gameObjectRequired)
            {
                StartCoroutine(WaitForFadeIn());
                DialogueUI.instance.ShowDialogue(DialogueObject);
                dialogueTriggered = true;
            }
        }
    }

    IEnumerator WaitForFadeIn()
    {
        yield return new WaitForSeconds(waitTime);
    }

}
