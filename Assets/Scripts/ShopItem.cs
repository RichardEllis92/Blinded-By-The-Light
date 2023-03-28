using UnityEngine;

public class ShopItem : MonoBehaviour
{
    public GameObject buyMessage;

    private bool _inBuyZone;

    public bool isHealthRestore, isHealthUpgrade, isWeapon;

    public int itemCost;

    public int healthUpgradeAmount;

    void Update()
    {
        if(_inBuyZone)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if(LevelManager.Instance.currentCoins >= itemCost)
                {
                    LevelManager.Instance.SpendCoins(itemCost);

                    if (isHealthRestore)
                    {
                        PlayerHealthController.Instance.HealPlayer(PlayerHealthController.Instance.maxHealth);
                    }

                    if (isHealthUpgrade)
                    {
                        PlayerHealthController.Instance.IncreaseMaxHealth(healthUpgradeAmount);
                    }

                    gameObject.SetActive(false);
                    _inBuyZone = false;

                    AudioManager.Instance.PlaySfx(11);
                }
                else
                {
                    AudioManager.Instance.PlaySfx(12);
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            buyMessage.SetActive(true);

            _inBuyZone = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            buyMessage.SetActive(false);
            _inBuyZone = false;
        }
    }
}
