using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour
{
    public string levelToLoad;
    int secretEnding = 0;
    public GameObject secretEndingScreen;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Scene currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;

        if (other.tag == "Player" && gameObject.activeSelf)
        {
            //SceneManager.LoadScene(levelToLoad);
            if(sceneName == "Boss")
            {
                LevelManager.instance.RemoveCoins();
                PlayerHealthController.instance.DefaultHealth();
                UIController.instance.SecretEnding();
            }
            if(sceneName != "Boss")
            {
                StartCoroutine(LevelManager.instance.LevelEnd());
            }
            else if(UIController.instance.secretEnding < 2)
            {
                StartCoroutine(LevelManager.instance.LevelEnd());
            }
            else
            {
                secretEndingScreen.SetActive(true);
                AudioManager.instance.PlaySecretEndingMusic();
            }
        }
    }
}
