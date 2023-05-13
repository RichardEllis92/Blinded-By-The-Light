using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    public float waitToLoad = 4f;

    public string nextLevel;

    public bool isPaused;

    public int currentHellBucks;

    public int defaultHellBucks;
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

        if (sceneName == "Luci Room")
        {
            return;
        }
/*
        if (sceneName == "Level 1" || sceneName == "Level 1 Again")
        {
            // Set default values for new game or specific levels
            PlayerHealthController.Instance.currentHealth = defaultHealth;
            PlayerHealthController.Instance.maxHealth = defaultMaxHealth;
            currentHellBucks = defaultHellBucks;
        }
        else
        {
            // Load values from CharacterTracker
            currentHellBucks = CharacterTracker.Instance.currentHellBucks;
            PlayerHealthController.Instance.currentHealth = CharacterTracker.Instance.currentHealth;
            PlayerHealthController.Instance.maxHealth = CharacterTracker.Instance.maxHealth;
        }
*/
        PlayerController.Instance.transform.position = startPoint.position;
        PlayerController.Instance.canMove = true;

        Time.timeScale = 1f;

        UIController.Instance.hellBucksText.text = currentHellBucks.ToString() + " Hell Bucks";
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
            CharacterTracker.Instance.currentHellBucks = currentHellBucks;
            CharacterTracker.Instance.currentHealth = PlayerHealthController.Instance.currentHealth;
            CharacterTracker.Instance.maxHealth = PlayerHealthController.Instance.maxHealth;
            Experience.Instance.UpdateExperiencePoints(LevelEndExperience);
        }     

        DialogueUI.Instance.talkedToGuide = false;
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

    public void GetHellBucks(int amount)
    {
        currentHellBucks += amount;

        UIController.Instance.hellBucksText.text = currentHellBucks.ToString() + " Hell Bucks";
    }

    public void SpendCoins(int amount)
    {
        currentHellBucks -= amount;

        if(currentHellBucks < 0)
        {
            currentHellBucks = 0;
        }

        UIController.Instance.hellBucksText.text = currentHellBucks.ToString() + " Hell Bucks";
    }

    public void RemoveCoins()
    {
        currentHellBucks = defaultHellBucks;
    }

}
