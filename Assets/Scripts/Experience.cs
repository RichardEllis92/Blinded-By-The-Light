using System;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using Scene = UnityEngine.SceneManagement.Scene;

public class Experience : MonoBehaviour
{
    public static Experience Instance;
    
    public int experiencePoints;
    public int MaxExperience = 1000;
    public bool hasMaxExperience;
    private bool _windSpellUnlocked;
    private bool _iceSpellUnlocked;
    private int _windUnlockExperience = 100;
    private int _iceUnlockExperience = 200;
    private bool windSpellDialogueUsed;
    private bool iceSpellDialogueUsed;
    
    
    [SerializeField] private DialogueUI dialogueUI;

    public DialogueUI DialogueUI => dialogueUI;

    [SerializeField] private DialogueObject windSpellUnlockDialogue;
    [SerializeField] private DialogueObject iceSpellUnlockDialogue;
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;
        
        if (sceneName != "Boss")
        {
            UIController.Instance.experienceSlider.value = experiencePoints;
            UIController.Instance.experienceSlider.maxValue = MaxExperience;
            experiencePoints = CharacterTracker.Instance.experience;
        }
    }

    private void Update()
    {
        if (experiencePoints >= _windUnlockExperience && !_windSpellUnlocked)
        {
            CheatSystemController.Instance.AddToListWindSpell();
            _windSpellUnlocked = true;
        }
        if (experiencePoints >= _windUnlockExperience && !windSpellDialogueUsed && !PlayerController.Instance.EnemyNearby())
        {
            DialogueUI.ShowDialogue(windSpellUnlockDialogue);
            windSpellDialogueUsed = true;
        }
        //Unlocks Ice spell
        if (experiencePoints >= _iceUnlockExperience && !_iceSpellUnlocked)
        {
            CheatSystemController.Instance.AddToListIceSpell();
            _iceSpellUnlocked = true;
            
            if (!iceSpellDialogueUsed && !PlayerController.Instance.EnemyNearby())
            {
                DialogueUI.ShowDialogue(iceSpellUnlockDialogue);
                iceSpellDialogueUsed = true;
            }
        }
    }

    public void UpdateExperiencePoints(int updateExperience)
    {
        experiencePoints += updateExperience;
        UIController.Instance.experienceSlider.value = experiencePoints;

        if (!hasMaxExperience && experiencePoints >= MaxExperience)
        {
            hasMaxExperience = true;
        }
    }
}
