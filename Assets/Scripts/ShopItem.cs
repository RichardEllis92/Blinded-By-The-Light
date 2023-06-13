 using System;
using System.Collections.Generic;
using UnityEngine;
using Random = Unity.Mathematics.Random;

public class ShopItem : MonoBehaviour
{
    public static ShopItem Instance;

    public GameObject buyMessage;

    private bool _inBuyZone;

    public bool isHealthRestore, isHealthUpgrade, isCheat;

    public int itemCost;

    public int healthUpgradeAmount;

    public delegate void MyFunctionDelegate();

    public List<MyFunctionDelegate> functions;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        functions = new List<MyFunctionDelegate>();

        // Add your functions to the list
        CheatSystemController cheatSystemController = FindObjectOfType<CheatSystemController>();
        functions.Add(() => cheatSystemController.AddToListComplimentPlayer());
        functions.Add(() => cheatSystemController.AddToListHealPlayer());
        functions.Add(() => cheatSystemController.AddToListIncreaseMaxHealth());
        functions.Add(() => cheatSystemController.AddToListInvincibility());
    }

    void Update()
    {
        if (_inBuyZone)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (CharacterTracker.Instance.currentHellBucks >= itemCost)
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

                    if (isCheat)
                    {
                        int index = UnityEngine.Random.Range(0, functions.Count);
                        functions[index]();
                        functions.RemoveAt(index);
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
        if (other.CompareTag("Player"))
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
    
    public void RemoveComplimentFromList()
    {
        CheatSystemController cheatSystemController = FindObjectOfType<CheatSystemController>();
        functions.Remove(() => cheatSystemController.AddToListComplimentPlayer());
    }

    public void AddHealPlayerCheat()
    {
        CheatSystemController cheatSystemController = FindObjectOfType<CheatSystemController>();
        functions.Add(() => cheatSystemController.AddToListHealPlayer());
    }
    public void AddIncreaseMaxHealthCheat()
    {
        CheatSystemController cheatSystemController = FindObjectOfType<CheatSystemController>();
        functions.Add(() => cheatSystemController.AddToListIncreaseMaxHealth());
    }
    public void AddInvincibleCheat()
    {
        CheatSystemController cheatSystemController = FindObjectOfType<CheatSystemController>();
        functions.Add(() => cheatSystemController.AddToListInvincibility());
    }
    
}