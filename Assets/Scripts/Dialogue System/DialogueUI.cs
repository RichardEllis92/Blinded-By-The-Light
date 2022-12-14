using System.Collections;
using UnityEngine;
using TMPro;

public class DialogueUI : MonoBehaviour
{
    public static DialogueUI instance;

    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private TMP_Text textLabel;
    [SerializeField] private GameObject continueText;

    public bool isOpen { get; private set; }

    private ResponseHandler responseHandler;
    private TypewriterEffect typewriterEffect;

    public GameObject endGameUI;
    public GameObject music;
    private void Start()
    {
        instance = this;

        typewriterEffect = GetComponent<TypewriterEffect>();
        responseHandler = GetComponent<ResponseHandler>();
        CloseDialogueBox();
    }

    public void ShowDialogue(DialogueObject dialogueObject)
    {
        isOpen = true;
        dialogueBox.SetActive(true);
        StartCoroutine(StepThroughDialogue(dialogueObject));
    }

    public void AddResponseEvents(ResponseEvent[] responseEvents)
    {
        responseHandler.AddResponseEvents(responseEvents);
    }
    private IEnumerator StepThroughDialogue(DialogueObject dialogueObject)
    {
        for (int i = 0; i < dialogueObject.Dialogue.Length; i++)
        {
            continueText.SetActive(false);

            string dialogue = dialogueObject.Dialogue[i];

            yield return RunTypingEffect(dialogue);

            textLabel.text = dialogue;

            if (i == dialogueObject.Dialogue.Length - 1 && dialogueObject.HasResponses) break;

            yield return new WaitForSeconds(0.3f);
            
            if ((DialogueTriggerEndGame.instance.endGame))
            {
                AudioManager.instance.PlaySFX(8);
                AudioManager.instance.StopMusic();
                yield return new WaitForSeconds(0.1f);
                endGameUI.SetActive(true); 
            }
            else
            {
                continueText.SetActive(true);
                yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.E));
            }
            
            
        }

        if (dialogueObject.HasResponses)
        {
            responseHandler.ShowResponses(dialogueObject.Responses);
        }
        else if (DialogueTriggerEndGame.instance.endGame)
        {
            endGameUI.SetActive(true);
        }
        else
        {
            CloseDialogueBox();
        }
    }

    private IEnumerator RunTypingEffect(string dialogue)
    {
        AudioManager.instance.PlaySFX(0);
        typewriterEffect.Run(dialogue, textLabel);

        while (typewriterEffect.IsRunning)
        {
            yield return null;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                AudioManager.instance.StopSFX(0);
                typewriterEffect.Stop();
            }
        }
    }

    public void CloseDialogueBox()
    {
        continueText.SetActive(false);
        isOpen = false;
        dialogueBox.SetActive(false);
        textLabel.text = string.Empty;
    }
}