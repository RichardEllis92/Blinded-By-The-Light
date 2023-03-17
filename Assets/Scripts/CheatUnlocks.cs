using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheatUnlocks : MonoBehaviour
{
    public static CheatUnlocks instance;
    
    private bool windSpellUnlocked = false;
    private bool windSpellAlreadyUnlocked = false;
    [SerializeField]
    private GameObject windSpellUnlockedPopUp;
    [SerializeField]
    private GameObject windSpellAlreadyUnlockedPopUp;
    
    private bool iceSpellUnlocked = false;
    private bool iceSpellAlreadyUnlocked = false;
    [SerializeField]
    private GameObject iceSpellUnlockedPopUp;
    [SerializeField]
    private GameObject iceSpellAlreadyUnlockedPopUp;
    
    public void UnlockWindSpell()
    {
        windSpellUnlocked = true;
        
        StartCoroutine(UnlockWindSpellPopUp());
    }
    
    IEnumerator UnlockWindSpellPopUp()
    {
        if (!windSpellAlreadyUnlocked)
        {
            windSpellUnlockedPopUp.SetActive(true);
            AudioManager.instance.PlaySFX(1);
            yield return new WaitForSeconds(2f);
            windSpellUnlockedPopUp.SetActive(false);
            windSpellAlreadyUnlocked = true;
        }
        else
        {
            windSpellAlreadyUnlockedPopUp.SetActive(true);
            AudioManager.instance.PlaySFX(2);
            yield return new WaitForSeconds(2f);
            windSpellAlreadyUnlockedPopUp.SetActive(false);
        }

    }
    
    public void UnlockIceSpell()
    {
        iceSpellUnlocked = true;
        
        StartCoroutine(UnlockIceSpellPopUp());
    }
    
    IEnumerator UnlockIceSpellPopUp()
    {
        if (!iceSpellAlreadyUnlocked)
        {
            iceSpellUnlockedPopUp.SetActive(true);
            AudioManager.instance.PlaySFX(1);
            yield return new WaitForSeconds(2f);
            iceSpellUnlockedPopUp.SetActive(false);
            iceSpellAlreadyUnlocked = true;
        }
        else
        {
            iceSpellAlreadyUnlockedPopUp.SetActive(true);
            AudioManager.instance.PlaySFX(2);
            yield return new WaitForSeconds(2f);
            iceSpellAlreadyUnlockedPopUp.SetActive(false);
        }

    }
}
