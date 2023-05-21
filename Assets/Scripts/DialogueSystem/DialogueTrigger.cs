using System.Collections;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    private bool _startingRoomDialogue;
    private const float WaitTime = 1f;
    private bool _dialogueTriggered;
    public bool gameObjectRequired;
    public new GameObject gameObject;
    public string dialogueTriggerKey;

    [SerializeField] private DialogueObject dialogueObject;

    void Start()
    {
        // Check if the dialogue has been triggered before
        _dialogueTriggered = PlayerPrefs.GetInt(dialogueTriggerKey, 0) == 1;
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !_startingRoomDialogue && !_dialogueTriggered)
        {
            if ((gameObjectRequired && gameObject.activeSelf) || !gameObjectRequired)
            {
                DialogueUI.Instance.ShowDialogue(dialogueObject);
                _dialogueTriggered = true;
                PlayerPrefs.SetInt(dialogueTriggerKey, 1);
            }
        }
        else if (other.CompareTag("Player") && _startingRoomDialogue && !_dialogueTriggered)
        {
            if ((gameObjectRequired && gameObject.activeSelf) || !gameObjectRequired)
            {
                StartCoroutine(WaitForFadeIn());
                DialogueUI.Instance.ShowDialogue(dialogueObject);
                _dialogueTriggered = true;
                PlayerPrefs.SetInt(dialogueTriggerKey, 1);
            }
        }
    }

    private static IEnumerator WaitForFadeIn()
    {
        yield return new WaitForSeconds(WaitTime);
    }

}
