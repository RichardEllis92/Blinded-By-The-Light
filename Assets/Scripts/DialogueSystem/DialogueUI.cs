using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using Rewired;
using UnityEngine.UI;

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
    public string actionText;
    public string consoleText;


    private ResponseHandler _responseHandler;
    private TypeWriterEffect _typeWriterEffect;
    
    public int playerId = 0;
    private Player player;
    bool skipDisabledMaps = true;
    [System.NonSerialized] // Don't serialize this so the value is lost on an editor script recompile.
    private bool initialized;

    private void Initialize() {
        // Get the Rewired Player object for this player.
        player = ReInput.players.GetPlayer(playerId);
            
        initialized = true;
    }
    private void Start()
    {
        Instance = this;
        // Get the Rewired Player for the desired player ID
        player = ReInput.players.GetPlayer(0); // Replace 0 with the appropriate player ID
        
        playerName = GetUserName();
        actionText = player.controllers.maps.GetFirstElementMapWithAction("Action", skipDisabledMaps).elementIdentifierName;
        consoleText = player.controllers.maps.GetFirstElementMapWithAction("Console", skipDisabledMaps).elementIdentifierName;
        _typeWriterEffect = GetComponent<TypeWriterEffect>();
        _responseHandler = GetComponent<ResponseHandler>();
        dialogueBox.SetActive(true);
        var continueTextString = continueText.GetComponent<Text>().text;
        continueTextString = continueTextString.Replace("{action}", actionText);
        continueText.GetComponent<Text>().text = continueTextString;
        dialogueBox.SetActive(false);
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

        // Check if the username is not empty and contains at least one character
        if (!string.IsNullOrEmpty(userName) && userName.Length > 0)
        {
            // Capitalize the first letter and convert the rest to lowercase
            userName = char.ToUpper(userName[0]) + userName.Substring(1).ToLower();
        }

        return userName;
    }

    private void Update()
    {
        if(!ReInput.isReady) return; 
        if(!initialized) Initialize();

        var currentScene = SceneManager.GetActiveScene();
        var sceneName = currentScene.name;

        if (sceneName == "Boss" || sceneName == "BossFail")
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

            dialogue = dialogue.Replace("{playerName}", playerName).Replace("{console}", consoleText);
            yield return RunTypingEffect(dialogue);

            if (i == dialogueObject.Dialogue.Length - 1 && dialogueObject.HasResponses) break;

            yield return new WaitForSeconds(0.3f);
            continueText.SetActive(true);
            yield return new WaitUntil(() => player.GetButtonDown("Action"));
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

            if (player.GetButtonDown("Roll"))
            {
                AudioManager.Instance.StopSfx(14);
                _typeWriterEffect.Stop();
                textLabel.text = dialogue;
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
