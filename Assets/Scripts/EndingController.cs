using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndingController : MonoBehaviour
{
    public string newGameScene, mainMenuScene;
    public GameObject endingScreen;
    private void update()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;
    }
    // Start is called before the first frame update
    public void NewGame()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene(newGameScene);

        endingScreen.SetActive(false);
    }

    public void ReturnToMainMenu()
    {
        Destroy(PlayerController.instance.gameObject);
        Destroy(gameObject);
        UIController.instance.DestroyAllGameObjects();
        DialogueUI.instance.talkedToGuide = false;

        SceneManager.LoadScene(mainMenuScene);

        endingScreen.SetActive(false);
    }


}
