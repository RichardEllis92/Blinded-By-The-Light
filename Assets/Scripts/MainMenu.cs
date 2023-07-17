using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string levelToLoad;
    public GameObject controlsPanel;
    public GameObject titleScreen;

    private void Start()
    {
        controlsPanel.SetActive(false);
        titleScreen.SetActive(true);
    }

    public void StartGame()
    {
        SceneManager.LoadScene(levelToLoad);
    }

    public void ExitGame()
    {
        PlayerPrefs.DeleteAll();
        Application.Quit();
    }

    public void Controls()
    {
        controlsPanel.SetActive(true);
        titleScreen.SetActive(false);
    }

    public void HideControls()
    {
        controlsPanel.SetActive(false);
        titleScreen.SetActive(true);
    }
}
