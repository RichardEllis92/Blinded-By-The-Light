using System;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;

public class CharacterTracker : MonoBehaviour
{
    public static CharacterTracker Instance;

    public int currentHealth, maxHealth, currentHellBucks, experience;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;

        currentHealth = PlayerHealthController.Instance.currentHealth;
        maxHealth = PlayerHealthController.Instance.maxHealth;
        experience = Experience.Instance.experiencePoints;
        currentHellBucks = 0;

    }

    private void Update()
    {
        currentHealth = PlayerHealthController.Instance.currentHealth;
        maxHealth = PlayerHealthController.Instance.maxHealth;
        experience = Experience.Instance.experiencePoints;
    }
}
