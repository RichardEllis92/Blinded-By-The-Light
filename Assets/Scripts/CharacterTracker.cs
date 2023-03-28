using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterTracker : MonoBehaviour
{
    public static CharacterTracker Instance;

    public int currentHealth, maxHealth, currentCoins;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;

        if (sceneName == "Level 1" || sceneName == "Level 1 Again")
        {
            currentCoins = 0;
        }
    }
}
