using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Rewired;

public class UIController : MonoBehaviour
{
    public static UIController Instance;

    public Slider healthSlider, experienceSlider;
    public Text healthText, hellBucksText, levelText, smallMapText, bigMapText;
    
    public Image fadeScreen;
    public float fadeSpeed;
    private bool _fadeToBlack, _fadeOutBlack;

    public string newGameScene, mainMenuScene, updatedMapText;

    public GameObject mapDisplay, bigMap, activeSpells, health, hellBucks, experience, levelTextObject;

    public Slider bossHealthBar;

    public int defaultHellBucks = 0;
    public int defaultHealth = 5;
    public int defaultMaxHealth = 5;

    public int secretEnding;

    public int playerCurrentHealth;

    public int playerId = 0;
    private Player player;
    bool skipDisabledMaps = true;
    
    // Start is called before the first frame update
    private void Awake()
    {
        Instance = this;

        DontDestroyOnLoad(gameObject);
        
    }

    // called first
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name != "Level 1")
        {
            _fadeOutBlack = true;
            _fadeToBlack = false;
        }

        if (scene.name == "Boss" || scene.name == "BossFail")
        {
            experience.SetActive(false);
            hellBucks.SetActive(false);
            mapDisplay.SetActive(false);
        }
    }

    void Start()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;

        if (sceneName == "Boss" || sceneName == "BossFail")
        {
            experience.SetActive(false);
            hellBucks.SetActive(false);
        }
        _fadeOutBlack = true;
        _fadeToBlack = false;
        
        player = ReInput.players.GetPlayer(0);

        UpdateMapText();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateMapText();
        
        Scene currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;

        if (_fadeOutBlack)
        {
            fadeScreen.color = new Color(fadeScreen.color.r, fadeScreen.color.g, fadeScreen.color.b, Mathf.MoveTowards(fadeScreen.color.a, 0f, fadeSpeed * Time.deltaTime));
            if(fadeScreen.color.a == 0f)
            {
                _fadeOutBlack = false;
            }
        }

        if (_fadeToBlack)
        {
            fadeScreen.color = new Color(fadeScreen.color.r, fadeScreen.color.g, fadeScreen.color.b, Mathf.MoveTowards(fadeScreen.color.a, 1f, fadeSpeed * Time.deltaTime));
            if (fadeScreen.color.a == 1f)
            {
                _fadeToBlack = false;
            }
        }

        if (sceneName == "Boss")
        {
            mapDisplay.SetActive(false);
        }

        playerCurrentHealth = PlayerHealthController.Instance.currentHealth;
   
    }

    public void StartFadeToBlack()
    {
        _fadeToBlack = true;
        _fadeOutBlack = false;
    }

    public void NewGame()
    {
        Time.timeScale = 1f;
        _fadeOutBlack = true;
        _fadeToBlack = false;
        DialogueUI.Instance.talkedToGuide = false;
        SceneManager.LoadScene(newGameScene);

        Destroy(PlayerController.Instance.gameObject);
        Destroy(gameObject);
        
        Scene currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;
    }

    public void ReturnToMainMenu()
    {
        
        Destroy(PlayerController.Instance.gameObject);
        Destroy(gameObject);
        DialogueUI.Instance.talkedToGuide = false;
        LevelManager.Instance.isPaused = false;
        SceneManager.LoadScene(mainMenuScene);
        ControlsManager.Instance.pauseMenu.SetActive(false);
        
    }

    // called when the game is terminated
    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void DestroyAllGameObjects()
    {
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("DontDestroy");
        
        foreach (var t in gameObjects)
        {
            Destroy(t);
        }
    }

    private void UpdateMapText()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;
        if (sceneName != "Boss" && sceneName != "BossFail")
        {
            var updatedMapText = player.controllers.maps.GetFirstElementMapWithAction("Toggle Map", skipDisabledMaps).elementIdentifierName;

            // Update the smallMapText
            smallMapText.GetComponent<Text>().text = "'" + updatedMapText  + "' for full map";

            // Update the bigMapText
            bigMapText.GetComponent<Text>().text = "'" + updatedMapText  + "' to exit map";
        }
    }
}
