using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    public float waitToLoad = 4f;

    public string nextLevel;

    public bool isPaused;

    public int currentCoins;

    public int defaultCoins;
    public int defaultHealth = 5;
    public int defaultMaxHealth = 5;
    
    public Transform startPoint;

    public GameObject dialogueBox;

    public GameObject bossDoor;

    private const int LevelEndExperience = 50;

    private void Awake()
    {
        Instance = this;
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
            currentCoins = CharacterTracker.Instance.currentCoins;
        }

        PlayerController.Instance.transform.position = startPoint.position;
        PlayerController.Instance.canMove = true;

        Time.timeScale = 1f;

        UIController.Instance.coinText.text = currentCoins.ToString() + " Gold";

      
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) && !DialogueUI.Instance.dialogueBox.activeSelf && !CheatSystemController.Instance.showConsole)
        {
            PauseUnpause();
        }
    }

    public IEnumerator LevelEnd()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;

        AudioManager.Instance.PlayLevelWin();

        PlayerController.Instance.canMove = false;

        if(sceneName == "Luci Room")
        {
            LuciRoomUI.Instance.StartFadeToBlack();
        }
        else
        {
            UIController.Instance.StartFadeToBlack();
        }
        

        yield return new WaitForSeconds(waitToLoad);

        if(sceneName != "Luci Room")
        {
            CharacterTracker.Instance.currentCoins = currentCoins;
            CharacterTracker.Instance.currentHealth = PlayerHealthController.Instance.currentHealth;
            CharacterTracker.Instance.maxHealth = PlayerHealthController.Instance.maxHealth;
        }     

        DialogueUI.Instance.talkedToGuide = false;
        Experience.Instance.UpdateExperiencePoints(LevelEndExperience);
        SceneManager.LoadScene(nextLevel);
    }

    public void PauseUnpause()
    {
        if (CameraController.Instance.bigMapActive == false)
        {
            if (!isPaused)
            {
                UIController.Instance.pauseMenu.SetActive(true);

                isPaused = true;

                Time.timeScale = 0f;
            }
            else
            {
                UIController.Instance.pauseMenu.SetActive(false);

                isPaused = false;

                Time.timeScale = 1f;
            }
        }
    }

    public void GetCoins(int amount)
    {
        currentCoins += amount;

        UIController.Instance.coinText.text = currentCoins.ToString() + " Gold";
    }

    public void SpendCoins(int amount)
    {
        currentCoins -= amount;

        if(currentCoins < 0)
        {
            currentCoins = 0;
        }

        UIController.Instance.coinText.text = currentCoins.ToString() + " Gold";
    }

    public void RemoveCoins()
    {
        currentCoins = defaultCoins;
    }

}
