using System;
using UnityEngine;

public class Experience : MonoBehaviour
{
    public static Experience Instance;
    
    public int experiencePoints;
    private const int MaxExperience = 1000;
    private bool _windSpellUnlocked;
    private bool _iceSpellUnlocked;
    private int _windUnlockExperience = 100;
    private int _iceUnlockExperience = 200;
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        UIController.Instance.experienceSlider.value = experiencePoints;
        UIController.Instance.experienceSlider.maxValue = MaxExperience;
    }

    private void Update()
    {
        if (experiencePoints >= _windUnlockExperience && !_windSpellUnlocked)
        {
            CheatSystemController.Instance.AddToListWindSpell();
            _windSpellUnlocked = true;
            Debug.Log("Wind Spell Unlocked");
        }
        //Unlocks Ice spell
        if (experiencePoints >= _iceUnlockExperience && !_iceSpellUnlocked)
        {
            CheatSystemController.Instance.AddToListIceSpell();
            _windSpellUnlocked = true;
            Debug.Log("Ice Spell Unlocked");
        }
        
    }

    public void UpdateExperiencePoints(int updateExperience)
    {
        experiencePoints += updateExperience;
        UIController.Instance.experienceSlider.value = experiencePoints;
    }
}
