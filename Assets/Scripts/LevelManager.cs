using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    public float waitToLoad = 4f;

    public string nextLevel;

    public bool isPaused;

    public int currentCoins;

    public int defaultCoins = 0;
    public int defaultHealth = 5;
    public int defaultMaxHealth = 5;
    
    public Transform startPoint;

    public GameObject dialogueBox;

    public GameObject bossDoor;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;

        if(sceneName == "Luci Room")
        {
            return;
        }
        else if (sceneName == "Level 1" || sceneName == "Level 1 Again")
        {
            currentCoins = 0;
        }
        else
        {
            currentCoins = CharacterTracker.instance.currentCoins;
        }

        PlayerController.instance.transform.position = startPoint.position;
        PlayerController.instance.canMove = true;

        Time.timeScale = 1f;

        UIController.instance.coinText.text = currentCoins.ToString() + " Gold";

      
    }

    // Update is called once per frame
    void Update()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;

        if (Input.GetKeyDown(KeyCode.P) && !DialogueUI.instance.dialogueBox.activeSelf)
        {
            PauseUnpause();
        }
    }

    public IEnumerator LevelEnd()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;

        AudioManager.instance.PlayLevelWin();

        PlayerController.instance.canMove = false;

        if(sceneName == "Luci Room")
        {
            LuciRoomUI.instance.StartFadeToBlack();
        }
        else
        {
            UIController.instance.StartFadeToBlack();
        }
        

        yield return new WaitForSeconds(waitToLoad);

        if(sceneName != "Luci Room")
        {
            CharacterTracker.instance.currentCoins = currentCoins;
            CharacterTracker.instance.currentHealth = PlayerHealthController.instance.currentHealth;
            CharacterTracker.instance.maxHealth = PlayerHealthController.instance.maxHealth;
        }     

        DialogueUI.instance.talkedToGuide = false;
        SceneManager.LoadScene(nextLevel);
    }

    public void PauseUnpause()
    {
        if (CameraController.instance.bigMapActive == false)
        {
            if (!isPaused)
            {
                UIController.instance.pauseMenu.SetActive(true);

                isPaused = true;

                Time.timeScale = 0f;
            }
            else
            {
                UIController.instance.pauseMenu.SetActive(false);

                isPaused = false;

                Time.timeScale = 1f;
            }
        }
    }

    public void GetCoins(int amount)
    {
        currentCoins += amount;

        UIController.instance.coinText.text = currentCoins.ToString() + " Gold";
    }

    public void SpendCoins(int amount)
    {
        currentCoins -= amount;

        if(currentCoins < 0)
        {
            currentCoins = 0;
        }

        UIController.instance.coinText.text = currentCoins.ToString() + " Gold";
    }

    public void RemoveCoins()
    {
        currentCoins = defaultCoins;
    }

}
