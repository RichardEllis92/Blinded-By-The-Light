using UnityEngine;
using UnityEngine.SceneManagement;

public class EndingController : MonoBehaviour
{
    public string newGameScene, mainMenuScene;
    public GameObject endingScreen;
    
    public void NewGame()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene(newGameScene);

        endingScreen.SetActive(false);
    }

    public void ReturnToMainMenu()
    {
        Destroy(PlayerController.Instance.gameObject);
        Destroy(gameObject);
        UIController.Instance.DestroyAllGameObjects();
        DialogueUI.Instance.talkedToGuide = false;

        SceneManager.LoadScene(mainMenuScene);

        endingScreen.SetActive(false);
    }


}
