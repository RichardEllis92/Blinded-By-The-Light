using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class UIController : MonoBehaviour
{
    public static UIController Instance;

    public Slider healthSlider, experienceSlider;
    public Text healthText, coinText;

    [FormerlySerializedAs("deathscreen")] public GameObject deathScreen;

    public Image fadeScreen;
    public float fadeSpeed;
    private bool _fadeToBlack, _fadeOutBlack;

    public string newGameScene, mainMenuScene;

    public GameObject pauseMenu, mapDisplay, bigMapText, bossHealthUI;

    public Slider bossHealthBar;

    public int defaultCoins = 0;
    public int defaultHealth = 5;
    public int defaultMaxHealth = 5;

    public int secretEnding;

    public int playerCurrentHealth;

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

    // called second
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name != "Level 1")
        {
            _fadeOutBlack = true;
            _fadeToBlack = false;
        }

        if (scene.name == "Boss")
        {
            bossHealthUI.SetActive(true);
        }
        else
        {
            bossHealthUI.SetActive(false);
            mapDisplay.SetActive(true);
        }
    }

    void Start()
    {
        _fadeOutBlack = true;
        _fadeToBlack = false;
    }

    // Update is called once per frame
    void Update()
    {

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

        DialogueUI.Instance.talkedToGuide = false;
        SceneManager.LoadScene(newGameScene);

        Destroy(PlayerController.Instance.gameObject);
        Destroy(gameObject);
        DestroyAllGameObjects();

    }

    public void ReturnToMainMenu()
    {
        
        Destroy(PlayerController.Instance.gameObject);
        Destroy(gameObject);
        DestroyAllGameObjects();
        DialogueUI.Instance.talkedToGuide = false;

        SceneManager.LoadScene(mainMenuScene);
        
    }

    public void Resume()
    {
        LevelManager.Instance.PauseUnpause();
    }

    // called when the game is terminated
    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void DestroyAllGameObjects()
    {
        GameObject[] gameObjects = FindObjectsOfType<GameObject>();

        foreach (var t in gameObjects)
        {
            Destroy(t);
        }
    }
    public void SecretEnding()
    {
        secretEnding++;
    }
}
