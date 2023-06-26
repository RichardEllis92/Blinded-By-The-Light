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
    
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Scene currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;
        if (sceneName == "Boss" || sceneName == "BossFail")
        {
            CheatUnlocks.Instance.UnlockWindSpell();
            CheatUnlocks.Instance.UnlockIceSpell();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;

        if (sceneName == "Luci Room" || sceneName == "Luci Room Complete" || sceneName == "Luci Room Doll")
        {
            return;
        }

        if (sceneName != "Boss" && sceneName != "BossFail" && sceneName != "RecordingScene")
        {
            PlayerController.Instance.transform.position = startPoint.position;
            PlayerController.Instance.canMove = true;
            UIController.Instance.hellBucksText.text = currentHellBucks.ToString("D2");
            currentHellBucks = CharacterTracker.Instance.currentHellBucks;
            UIController.Instance.hellBucksText.text = CharacterTracker.Instance.currentHellBucks.ToString("D2");
        }
        
        Time.timeScale = 1f;
        LevelText();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) && !DialogueUI.Instance.dialogueBox.activeSelf && !CheatSystemController.Instance.showConsole)
        {
            PauseUnpause();
        }
        
        LevelText();
    }

    public IEnumerator LevelEnd()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;

        AudioManager.Instance.PlayLevelWin();

        PlayerController.Instance.canMove = false;

        if(sceneName == "Luci Room" || sceneName == "Luci Room Complete" || sceneName == "Luci Room Doll")
        {
            LuciRoomUI.Instance.StartFadeToBlack();
        }
        else
        {
            UIController.Instance.StartFadeToBlack();
        }
        

        yield return new WaitForSeconds(waitToLoad);

        if(sceneName != "Luci Room" && sceneName != "Luci Room Complete" && sceneName != "Luci Room Doll" && sceneName != "Boss")
        {
            CharacterTracker.Instance.currentHealth = PlayerHealthController.Instance.currentHealth;
            CharacterTracker.Instance.maxHealth = PlayerHealthController.Instance.maxHealth;
            Experience.Instance.UpdateExperiencePoints(LevelEndExperience);
        }     

        DialogueUI.Instance.talkedToGuide = false;

        if (sceneName == "Level 3")
        {
            UIController.Instance.NewGame();
            SceneManager.LoadScene("Luci Room Complete");
        }
        else
        {
            SceneManager.LoadScene(nextLevel);
        }
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
        CharacterTracker.Instance.currentHellBucks += amount;

        UIController.Instance.hellBucksText.text = CharacterTracker.Instance.currentHellBucks.ToString("D2");
    }

    public void SpendCoins(int amount)
    {
        CharacterTracker.Instance.currentHellBucks -= amount;

        if(CharacterTracker.Instance.currentHellBucks < 0)
        {
            CharacterTracker.Instance.currentHellBucks = 0;
        }

        UIController.Instance.hellBucksText.text = CharacterTracker.Instance.currentHellBucks.ToString("D2");
    }

    public void LevelText()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;

        switch (sceneName)
        {
            case "Level 1":
                UIController.Instance.levelText.text = "Treachery: 1";
                break;
            case "Level 2":
                UIController.Instance.levelText.text = "Treachery: 2";
                break;
            case "Level 3":
                UIController.Instance.levelText.text = "Treachery: 3";
                break;
            case "Boss":
                UIController.Instance.levelText.text = "Boss Room";
                break;
            case "BossFail":
                UIController.Instance.levelText.text = "Boss Room";
                break;
        }
    }
    
    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

}
