using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour
{
    public string levelToLoad;
    int _secretEnding = 0;
    public GameObject secretEndingScreen;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        Scene currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;

        if (other.CompareTag("Player") && gameObject.activeSelf)
        {
            if (sceneName == "Luci Room Complete")
            {
                SceneManager.LoadScene(levelToLoad);
            }
            //SceneManager.LoadScene(levelToLoad);
            if(sceneName == "Boss")
            {
                LevelManager.Instance.RemoveCoins();
                PlayerHealthController.Instance.DefaultHealth();
                UIController.Instance.SecretEnding();
            }
            if(sceneName != "Boss")
            {
                StartCoroutine(LevelManager.Instance.LevelEnd());
            }
            else if(UIController.Instance.secretEnding < 2)
            {
                StartCoroutine(LevelManager.Instance.LevelEnd());
            }
            else
            {
                secretEndingScreen.SetActive(true);
                AudioManager.Instance.PlaySecretEndingMusic();
            }
        }
    }
}
