using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour
{
    public string levelToLoad;
    public float waitToLoad = 4f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Scene currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;

        if (other.CompareTag("Player") && gameObject.activeSelf)
        {
            if (sceneName == "Luci Room Complete" || sceneName == "Luci Room Doll")
            {
                //SceneManager.LoadScene(levelToLoad);
                StartCoroutine(LevelEndLuciRoom());
            }
            else if(sceneName != "Boss" && sceneName != "BossFail")
            {
                StartCoroutine(LevelManager.Instance.LevelEnd());
            }
        }
    }

    private IEnumerator LevelEndLuciRoom()
    {
        LuciRoomUI.Instance.FadeToBlack();
        yield return new WaitForSeconds(waitToLoad);
        SceneManager.LoadScene(levelToLoad);
    }
}
