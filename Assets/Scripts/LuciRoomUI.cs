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
    public string mainMenuScene;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        FadeOutFromBlack();
    }

    private void Update()
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

    public void FadeToBlack()
    {
        _fadeToBlack = true;
        _fadeOutBlack = false;
    }

    public void FadeOutFromBlack()
    {
        _fadeOutBlack = true;
        _fadeToBlack = false;
    }

    public void Resume()
    {
        LevelManager.Instance.PauseUnpause();
    }

    public void SwitchPauseToControls()
    {
        pauseMenu.SetActive(false);
        ControlsManager.Instance.controlsMenu.SetActive(true);
        LevelManager.Instance.controlsMenuOpen = true;
    }

    public void SwitchControlsToPause()
    {
        pauseMenu.SetActive(true);
        ControlsManager.Instance.controlsMenu.SetActive(false);
        LevelManager.Instance.controlsMenuOpen = false;
    }

    public void ReturnToMainMenu()
    {
        FadeToBlack();
        LevelManager.Instance.isPaused = false;
        Invoke("LoadMainMenuScene", fadeSpeed);
    }

    private void LoadMainMenuScene()
    {
        SceneManager.LoadScene(mainMenuScene);
    }

    public void DestroyAllGameObjects()
    {
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("DontDestroy");

        foreach (var t in gameObjects)
        {
            Destroy(t);
        }
    }
}