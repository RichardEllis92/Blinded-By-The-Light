using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CheatUnlocks : MonoBehaviour
{
    public static CheatUnlocks Instance;
    
    public bool _windSpellUnlocked = false;
    private bool _windSpellAlreadyUnlocked = false;
    [SerializeField]
    private GameObject windSpellUnlockedPopUp;
    [SerializeField]
    private GameObject windSpellAlreadyUnlockedPopUp;
    
    public bool _iceSpellUnlocked = false;
    private bool _iceSpellAlreadyUnlocked = false;
    [SerializeField]
    private GameObject iceSpellUnlockedPopUp;
    [SerializeField]
    private GameObject iceSpellAlreadyUnlockedPopUp;

    [SerializeField]
    private GameObject complimentPlayer;

    private const float InvincibilityLength = 30f;

    private void Awake()
    {
        Instance = this;
    }

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
            AudioManager.Instance.PlaySfx(17);
            yield return new WaitForSeconds(2f);
            windSpellUnlockedPopUp.SetActive(false);
            _windSpellAlreadyUnlocked = true;
        }
        else
        {
            windSpellAlreadyUnlockedPopUp.SetActive(true);
            AudioManager.Instance.PlaySfx(18);
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
            AudioManager.Instance.PlaySfx(17);
            yield return new WaitForSeconds(2f);
            iceSpellUnlockedPopUp.SetActive(false);
            _iceSpellAlreadyUnlocked = true;
        }
        else
        {
            iceSpellAlreadyUnlockedPopUp.SetActive(true);
            AudioManager.Instance.PlaySfx(18);
            yield return new WaitForSeconds(2f);
            iceSpellAlreadyUnlockedPopUp.SetActive(false);
        }
    }
    public void ComplimentPlayer()
    {
        StartCoroutine(ComplimentPlayerPopUp());
    }

    IEnumerator ComplimentPlayerPopUp()
    {
        complimentPlayer.SetActive(true);
        string[] compliments = {"You're doing great!", "You're awesome!", "Keep up the good work!", "You're a superstar!", "HAIL SATAN!"};
        int index = Random.Range(0, compliments.Length);
        complimentPlayer.GetComponent<Text>().text = compliments[index];
        yield return new WaitForSeconds(2f);
        complimentPlayer.SetActive(false);
    }
    
    public void HealPlayer()
    {
        PlayerHealthController.Instance.HealPlayer(PlayerHealthController.Instance.maxHealth);
        AudioManager.Instance.PlaySfx(17);
        CheatSystemController.Instance.RemoveFromListHealPlayer();
    }
    
    public void IncreaseMaxHealth()
    {
        PlayerHealthController.Instance.IncreaseMaxHealth(1);
        AudioManager.Instance.PlaySfx(17);
        CheatSystemController.Instance.RemoveFromListIncreaseMaxHealth();
    }

    public void Invincibility()
    {
        PlayerHealthController.Instance.MakeInvincible(InvincibilityLength);
        AudioManager.Instance.PlaySfx(17);
        CheatSystemController.Instance.RemoveFromListInvincibility();
    }

}
