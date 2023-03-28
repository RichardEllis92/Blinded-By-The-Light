using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LuciRoomUI : MonoBehaviour
{
    public static LuciRoomUI Instance;

    public Image fadeScreen;
    public float fadeSpeed;
    private bool _fadeToBlack, _fadeOutBlack;
    public GameObject pauseMenu;
    public string  mainMenuScene;
    // Start is called before the first frame update
    private void Awake()
    {
        Instance = this;
    }

    // called first
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // called second
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        _fadeOutBlack = true;
        _fadeToBlack = false;
    }

    void Start()
    {
        _fadeOutBlack = true;
        _fadeToBlack = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (_fadeOutBlack)
        {
            fadeScreen.color = new Color(fadeScreen.color.r, fadeScreen.color.g, fadeScreen.color.b, Mathf.MoveTowards(fadeScreen.color.a, 0f, fadeSpeed * Time.deltaTime));
            if (fadeScreen.color.a == 0f)
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
    }

    public void StartFadeToBlack()
    {
        _fadeToBlack = true;
        _fadeOutBlack = false;
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

}

