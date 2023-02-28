using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    public static UIController instance;
    
    public Slider healthSlider;
    public Text healthText, coinText;

    public GameObject deathscreen;

    public Image fadeScreen;
    public float fadeSpeed;
    private bool fadeToBlack, fadeOutBlack;

    public string newGameScene, mainMenuScene;

    public GameObject pauseMenu, mapDisplay, bigMapText, bossHealthUI;

    public Slider bossHealthBar;

    public int defaultCoins = 0;
    public int defaultHealth = 5;
    public int defaultMaxHealth = 5;

    public int secretEnding = 0;

    public int playerCurrentHealth = 0;

    // Start is called before the first frame update
    private void Awake()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;
        instance = this;

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
            fadeOutBlack = true;
            fadeToBlack = false;
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
        fadeOutBlack = true;
        fadeToBlack = false;
    }

    // Update is called once per frame
    void Update()
    {

        Scene currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;

        if (fadeOutBlack)
        {
            fadeScreen.color = new Color(fadeScreen.color.r, fadeScreen.color.g, fadeScreen.color.b, Mathf.MoveTowards(fadeScreen.color.a, 0f, fadeSpeed * Time.deltaTime));
            if(fadeScreen.color.a == 0f)
            {
                fadeOutBlack = false;
            }
        }

        if (fadeToBlack)
        {
            fadeScreen.color = new Color(fadeScreen.color.r, fadeScreen.color.g, fadeScreen.color.b, Mathf.MoveTowards(fadeScreen.color.a, 1f, fadeSpeed * Time.deltaTime));
            if (fadeScreen.color.a == 1f)
            {
                fadeToBlack = false;
            }
        }

        if (sceneName == "Boss")
        {
            mapDisplay.SetActive(false);
        }

        playerCurrentHealth = PlayerHealthController.instance.currentHealth;
   
    }

    public void StartFadeToBlack()
    {
        fadeToBlack = true;
        fadeOutBlack = false;
    }

    public void NewGame()
    {
        Time.timeScale = 1f;

        DialogueUI.instance.talkedToGuide = false;
        SceneManager.LoadScene(newGameScene);

        Destroy(PlayerController.instance.gameObject);
        Destroy(gameObject);
        DestroyAllGameObjects();

    }

    public void ReturnToMainMenu()
    {
        
        Destroy(PlayerController.instance.gameObject);
        Destroy(gameObject);
        DestroyAllGameObjects();
        DialogueUI.instance.talkedToGuide = false;

        SceneManager.LoadScene(mainMenuScene);
        
    }

    public void Resume()
    {
        LevelManager.instance.PauseUnpause();
    }

    // called when the game is terminated
    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void DestroyAllGameObjects()
    {
        GameObject[] GameObjects = (FindObjectsOfType<GameObject>() as GameObject[]);

        for (int i = 0; i < GameObjects.Length; i++)
        {
            Destroy(GameObjects[i]);
        }
    }
    public void SecretEnding()
    {
        secretEnding++;
    }
}
