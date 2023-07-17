using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class CheatSystemController : MonoBehaviour
{
    public static CheatSystemController Instance;
    
    public bool showConsole;
    bool _showHelp;
    bool _displayHelp;

    string _input;


    private static CheatSystemCommand _windSpell;
    private static CheatSystemCommand _iceSpell;
    private static CheatSystemCommand _complimentPlayer;
    private static CheatSystemCommand _fullHealth;
    private static CheatSystemCommand _increaseMaxHealth;
    private static CheatSystemCommand _invincibility;
    
    private static CheatSystemCommand Help;

    private List<object> _cheatList;

    Vector2 _scroll;
    GUI _style;

    public GameObject invalidCheat;
    
    [SerializeField] private DialogueUI dialogueUI;
    public DialogueUI DialogueUI => dialogueUI;
    
    [SerializeField] private DialogueObject complimentPlayerText;
    [SerializeField] private DialogueObject healPlayerText;
    [SerializeField] private DialogueObject increaseMaxHealthText;
    [SerializeField] private DialogueObject invincibilityText;
    
    public int playerId = 0;
    private Player player;
    [System.NonSerialized] // Don't serialize this so the value is lost on an editor script recompile.
    private bool initialized;

    private void Awake()
    {
        Instance = this;
        
        _windSpell = new CheatSystemCommand("wind_spell", "Gives the player the ability to summon wind spell", "wind_spell", () =>
        {
            CheatUnlocks.Instance.UnlockWindSpell();
        });
        
        _iceSpell = new CheatSystemCommand("ice_spell", "Gives the player the ability to summon ice spell", "ice_spell", () =>
        {
            CheatUnlocks.Instance.UnlockIceSpell();
        });
        
        _complimentPlayer = new CheatSystemCommand("compliment", "Gives the player a nice compliment", "compliment", () =>
        {
            CheatUnlocks.Instance.ComplimentPlayer();
        });
        
        _fullHealth = new CheatSystemCommand("healme", "Heals the player to full health. Can only be used once.", "healme", () =>
        {
            CheatUnlocks.Instance.HealPlayer();
        });
        
        _increaseMaxHealth = new CheatSystemCommand("morehealth", "Gives the player more max health. Can only be used once.", "morehealth", () =>
        {
            CheatUnlocks.Instance.IncreaseMaxHealth();
        });
        
        _invincibility = new CheatSystemCommand("makemeinvincible", "Makes the player invincible for 30 seconds. Can only be used once.", "makemeinvincible", () =>
        {
            CheatUnlocks.Instance.Invincibility();
        });

        Help = new CheatSystemCommand("help", "Shows a list of commands", "help", () =>
        {
            _showHelp = true;
        });

        _cheatList = new List<object>
        {
            Help,
        };
    }
    
    private void Initialize() {
        // Get the Rewired Player object for this player.
        player = ReInput.players.GetPlayer(playerId);
            
        initialized = true;
    }

    private void Update()
    {
        if(!ReInput.isReady) return; // Exit if Rewired isn't ready. This would only happen during a script recompile in the editor.
        if(!initialized) Initialize(); // Reinitialize after a recompile in the editor
        
        DisplayCheatBox();
        SubmitCheat();
    }

    [Obsolete("Obsolete")]
    private void OnGUI()
    {
        if (LevelManager.Instance.isPaused || CameraController.Instance.bigMapActive) { return;}
        if (!showConsole) { _displayHelp = false; return; }

        float y = 0f;
        if (_displayHelp)
        {
            if (_showHelp)
            {
                GUI.Box(new Rect(0, y, Screen.width, 100), "");

                Rect viewport = new Rect(0, 0, Screen.width - 30, 40 * _cheatList.Count);

                _scroll = GUI.BeginScrollView(new Rect(0, y + 5f, Screen.width, 90), _scroll, viewport);

                for (int i = 0; i < _cheatList.Count; i++)
                {
                    if (_cheatList[i] is CheatSystemCommandBase cheat)
                    {
                        string label = $"{cheat.CheatFormat} - {cheat.CheatDescription}";

                        Rect labelRect = new Rect(5, 40 * i, viewport.width - 100, 50);

                        GUI.skin.label.fontSize = 30;

                        GUI.Label(labelRect, label);
                    }
                }

                GUI.EndScrollView();
                y += 100;
            }
        }

        GUI.Box(new Rect(0, y, Screen.width, 50), "");
        GUI.backgroundColor = new Color(0, 0, 0);
        GUI.skin.textField.fontSize = 30;
        _input = GUI.TextField(new Rect(10f, y + 5f, Screen.width - 20f, 50f), _input);
        Input.eatKeyPressOnTextFieldFocus = false;
    }



    private void HandleInput()
    {
        for (var i = 0; i < _cheatList.Count; i++)
        {
            if (_cheatList[i] is CheatSystemCommandBase commandBase && _input.Contains(commandBase.CheatID))
            {
                if (_cheatList[i] is CheatSystemCommand)
                {
                    (_cheatList[i] as CheatSystemCommand)?.Invoke();
                }
                else
                {
                    StartCoroutine(InvalidCheat());
                }
            }
        }
    }

    IEnumerator InvalidCheat()
    {
        invalidCheat.SetActive(true);
        yield return new WaitForSeconds(2f);
        invalidCheat.SetActive(false);
    }

    private void DisplayCheatBox()
    {
        if (player.GetButtonDown("Console") && !LevelManager.Instance.isPaused && !CameraController.Instance.bigMapActive)
        {
            showConsole = !showConsole;
            _input = "";
        }
    }

    private void SubmitCheat()
    {
        if (showConsole)
        {
            if (player.GetButtonDown("Enter"))
            {
                HandleInput();
                _input = "";
                _displayHelp = true;
            }
        }
    }
    
    public void AddToListWindSpell()
    {
        _cheatList.Add(_windSpell);
    }
    public void AddToListIceSpell()
    {
        _cheatList.Add(_iceSpell);
    }
    public void AddToListComplimentPlayer()
    {
        _cheatList.Add(_complimentPlayer);
        DialogueUI.ShowDialogue(complimentPlayerText);
        ShopItem.Instance.RemoveComplimentFromList();
    }
    public void AddToListHealPlayer()
    {
        DialogueUI.ShowDialogue(healPlayerText);
        _cheatList.Add(_fullHealth);
    }
    public void RemoveFromListHealPlayer()
    {
        _cheatList.Remove(_fullHealth);
        ShopItem.Instance.AddHealPlayerCheat();
    }
    public void AddToListIncreaseMaxHealth()
    {
        DialogueUI.ShowDialogue(increaseMaxHealthText);
        _cheatList.Add(_increaseMaxHealth);
    }
    public void RemoveFromListIncreaseMaxHealth()
    {
        _cheatList.Remove(_increaseMaxHealth);
        ShopItem.Instance.AddIncreaseMaxHealthCheat();
    }
    public void AddToListInvincibility()
    {
        DialogueUI.ShowDialogue(invincibilityText);
        _cheatList.Add(_invincibility);
    }
    public void RemoveFromListInvincibility()
    {
        _cheatList.Remove(_invincibility);
        ShopItem.Instance.AddInvincibleCheat();
    }
}
