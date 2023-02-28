using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class DialogueUI : MonoBehaviour
{
    public static DialogueUI instance;
    public GameObject dialogueBox;

    [SerializeField] private GameObject nameBox;
    [SerializeField] private TMP_Text textLabel;
    [SerializeField] private DialogueObject startingDialogueObject;
    [SerializeField] private TMP_Text nameLabel;
    [SerializeField] private GameObject continueText;
    public bool isOpen { get; private set; }
    public bool startingDialogue = false;
    public bool secondDialogue = false;
    public bool talkedToGuide = false;
    public bool talkedToFinalGuide = false;

    public string playerName = (System.Environment.UserName);


    private ResponseHandler responseHandler;
    private TypeWriterEffect typeWriterEffect;

    private void Start()
    {
        instance = this;
        

        typeWriterEffect = GetComponent<TypeWriterEffect>();
        responseHandler = GetComponent<ResponseHandler>();

    }

    private void Update()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;

        if (sceneName == "Boss")
        {
            talkedToFinalGuide = false;
        }
        
    }

    public void ShowDialogue(DialogueObject dialogueObject)
    {
        isOpen = true;
        nameBox.SetActive(true);
        nameLabel.text = dialogueObject.name;
        dialogueBox.SetActive(true);
        StartCoroutine(StepThroughDialogue(dialogueObject));
    }

    public void AddResponseEvents(ResponseEvent[] responseEvents)
    {
        responseHandler.AddResponseEvents(responseEvents);
    }
        private IEnumerator StepThroughDialogue(DialogueObject dialogueObject)
    {
        if (!startingDialogue)
        {
            yield return new WaitForSeconds(2f);
        }
        
        for (int i = 0; i < dialogueObject.Dialogue.Length; i++)
        {
            continueText.SetActive(false);
            string name = dialogueObject.name;
            string dialogue = dialogueObject.Dialogue[i];
            dialogue = dialogue.Replace("{playerName}", playerName);
            yield return RunTypingEffect(dialogue);

            
            textLabel.text = dialogue;
            
            if (i == dialogueObject.Dialogue.Length - 1 && dialogueObject.hasResponses) break;

            yield return new WaitForSeconds(0.3f);
            continueText.SetActive(true);
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.E));
        }

        if (dialogueObject.hasResponses)
        {
            responseHandler.ShowResponses(dialogueObject.Responses);
        }
        else
        {
            if (!startingDialogue)
            {
                startingDialogue = true;
            }
            
            if((dialogueObject.name == "Guide" || dialogueObject.name == "???") && talkedToGuide == false)
            {
                talkedToGuide = true;
            }

            if (dialogueObject.name == "Me" && talkedToFinalGuide == false)
            {
                talkedToFinalGuide = true;
            }

            CloseDialogueBox();
        }
    }

    private IEnumerator RunTypingEffect(string dialogue)
    {
        AudioManager.instance.PlaySFX(14);
        typeWriterEffect.Run(dialogue, textLabel);

        while (typeWriterEffect.isRunning)
        {
            yield return null;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                AudioManager.instance.StopSFX(14);
                typeWriterEffect.Stop();
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
