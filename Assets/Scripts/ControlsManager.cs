using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControlsManager : MonoBehaviour
{
    public static ControlsManager Instance;
    
    public GameObject controlsMenu;
    public GameObject pauseMenu;
    public string  mainMenuScene;
    public GameObject titleMenu;
    public bool controlsMenuOpen;
    public GameObject titleScreen;
    public GameObject level3Fade;
    public string levelToLoad;
    
    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;
        
        if (sceneName != "Title Menu")
            LevelManager.Instance.isPaused = false;
        controlsMenu.SetActive(true);
        controlsMenu.SetActive(false);
        pauseMenu.SetActive(false);
        level3Fade.SetActive(false);
    }

    private void Update()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;
        
        if (sceneName != "Level 3")
            level3Fade.SetActive(false);
    }

    public void SwitchPauseToControls()
    {
        pauseMenu.SetActive(false);
        controlsMenu.SetActive(true);
        controlsMenuOpen = true;
    }
    
    public void SwitchControlsToPause()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;
        if (sceneName == "Title Menu" || sceneName == "Title Menu 2")
        {
            titleMenu.SetActive(true);
            controlsMenu.SetActive(false);
            controlsMenuOpen = false;
        }
        else
        {
            pauseMenu.SetActive(true);
            controlsMenu.SetActive(false);
            controlsMenuOpen = false;
        }
    }
    
    public void Resume()
    {
        LevelManager.Instance.PauseUnpause();
    }
    
    public void ReturnToMainMenu()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;
        
        if (sceneName == "Level 1" || sceneName == "Level 2" || sceneName == "Level 3")
        {
            UIController.Instance.ReturnToMainMenu();
            UIController.Instance.DestroyAllGameObjects();
        }
        titleScreen.SetActive(true);
        SceneManager.LoadScene(mainMenuScene);
        LevelManager.Instance.isPaused = false;
        pauseMenu.SetActive(false);
    }
    
    public void Controls()
    {
        controlsMenu.SetActive(true);
        titleScreen.SetActive(false);
    }

    public void HideControls()
    {
        controlsMenu.SetActive(false);
        titleScreen.SetActive(true);
    }
    
    public void StartGame()
    {
        SceneManager.LoadScene(levelToLoad);
        titleScreen.SetActive(false);
    }

    public void ExitGame()
    {
        PlayerPrefs.DeleteAll();
        Application.Quit();
    }
}
