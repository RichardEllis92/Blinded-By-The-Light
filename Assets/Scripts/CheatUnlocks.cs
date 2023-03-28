using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheatUnlocks : MonoBehaviour
{
    public static CheatUnlocks Instance;
    
    private bool _windSpellUnlocked = false;
    private bool _windSpellAlreadyUnlocked = false;
    [SerializeField]
    private GameObject windSpellUnlockedPopUp;
    [SerializeField]
    private GameObject windSpellAlreadyUnlockedPopUp;
    
    private bool _iceSpellUnlocked = false;
    private bool _iceSpellAlreadyUnlocked = false;
    [SerializeField]
    private GameObject iceSpellUnlockedPopUp;
    [SerializeField]
    private GameObject iceSpellAlreadyUnlockedPopUp;
    
    public void UnlockWindSpell()
    {
        _windSpellUnlocked = true;
        
        StartCoroutine(UnlockWindSpellPopUp());
    }
    
    IEnumerator UnlockWindSpellPopUp()
    {
        if (!_windSpellAlreadyUnlocked)
        {
            windSpellUnlockedPopUp.SetActive(true);
            AudioManager.Instance.PlaySfx(1);
            yield return new WaitForSeconds(2f);
            windSpellUnlockedPopUp.SetActive(false);
            _windSpellAlreadyUnlocked = true;
        }
        else
        {
            windSpellAlreadyUnlockedPopUp.SetActive(true);
            AudioManager.Instance.PlaySfx(2);
            yield return new WaitForSeconds(2f);
            windSpellAlreadyUnlockedPopUp.SetActive(false);
        }

    }
    
    public void UnlockIceSpell()
    {
        _iceSpellUnlocked = true;
        
        StartCoroutine(UnlockIceSpellPopUp());
    }
    
    IEnumerator UnlockIceSpellPopUp()
    {
        if (!_iceSpellAlreadyUnlocked)
        {
            iceSpellUnlockedPopUp.SetActive(true);
            AudioManager.Instance.PlaySfx(1);
            yield return new WaitForSeconds(2f);
            iceSpellUnlockedPopUp.SetActive(false);
            _iceSpellAlreadyUnlocked = true;
        }
        else
        {
            iceSpellAlreadyUnlockedPopUp.SetActive(true);
            AudioManager.Instance.PlaySfx(2);
            yield return new WaitForSeconds(2f);
            iceSpellAlreadyUnlockedPopUp.SetActive(false);
        }

    }
}
