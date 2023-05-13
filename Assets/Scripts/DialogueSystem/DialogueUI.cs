using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class DialogueUI : MonoBehaviour
{
    public static DialogueUI Instance;
    public GameObject dialogueBox;
    
    [SerializeField] private TMP_Text textLabel;
    [SerializeField] private GameObject continueText;
    public bool IsOpen { get; private set; }
    public bool startingDialogue;
    public bool talkedToGuide;
    public bool talkedToFinalGuide;

    public string playerName;


    private ResponseHandler _responseHandler;
    private TypeWriterEffect _typeWriterEffect;

    private void Start()
    {
        Instance = this;

        playerName = GetUserName();
        _typeWriterEffect = GetComponent<TypeWriterEffect>();
        _responseHandler = GetComponent<ResponseHandler>();

    }
    private string GetUserName()
    {
        string userName = "";

        #if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
            userName = System.Environment.UserName;
        #elif UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX
            userName = System.IO.Path.GetFileNameWithoutExtension(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal));
        #else
            Debug.LogError("Retrieving username is not supported on this platform.");
        #endif
            return userName;
    }
    private void Update()
    {
        var currentScene = SceneManager.GetActiveScene();
        var sceneName = currentScene.name;

        if (sceneName == "Boss")
        {
            talkedToFinalGuide = false;
        }
        
    }

    public void ShowDialogue(DialogueObject dialogueObject)
    {
        IsOpen = true;
        dialogueBox.SetActive(true);
        StartCoroutine(StepThroughDialogue(dialogueObject));
    }

    public void AddResponseEvents(ResponseEvent[] responseEvents)
    {
        _responseHandler.AddResponseEvents(responseEvents);
    }
        private IEnumerator StepThroughDialogue(DialogueObject dialogueObject)
    {
        if (!startingDialogue)
        {
            yield return new WaitForSeconds(2f);
        }
        
        for (var i = 0; i < dialogueObject.Dialogue.Length; i++)
        {
            continueText.SetActive(false);
            var dialogue = dialogueObject.Dialogue[i];
            dialogue = dialogue.Replace("{playerName}", playerName);
            yield return RunTypingEffect(dialogue);

            
            textLabel.text = dialogue;
            
            if (i == dialogueObject.Dialogue.Length - 1 && dialogueObject.HasResponses) break;

            yield return new WaitForSeconds(0.3f);
            continueText.SetActive(true);
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.E));
        }

        if (dialogueObject.HasResponses)
        {
            _responseHandler.ShowResponses(dialogueObject.Responses);
        }
        else
        {
            if (!startingDialogue)
            {
                startingDialogue = true;
            }
            
            CloseDialogueBox();
        }
    }

    private IEnumerator RunTypingEffect(string dialogue)
    {
        AudioManager.Instance.PlaySfx(14);
        _typeWriterEffect.Run(dialogue, textLabel);

        while (_typeWriterEffect.IsRunning)
        {
            yield return null;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                AudioManager.Instance.StopSfx(14);
                _typeWriterEffect.Stop();
            }
        }
    }
    public void CloseDialogueBox()
    {
        continueText.SetActive(false);
        IsOpen = false;
        dialogueBox.SetActive(false);
        textLabel.text = string.Empty;
    }
}
