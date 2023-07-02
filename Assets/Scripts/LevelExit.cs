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
            if (sceneName == "Luci Room Complete" || sceneName == "Luci Room Doll")
            {
                SceneManager.LoadScene(levelToLoad);
            }
            //SceneManager.LoadScene(levelToLoad);
            if(sceneName == "Boss" || sceneName == "BossFail")
            {
                PlayerHealthController.Instance.DefaultHealth();
                UIController.Instance.SecretEnding();
            }
            if(sceneName != "Boss" && sceneName != "BossFail")
            {
                StartCoroutine(LevelManager.Instance.LevelEnd());
            }
        }
    }
}
