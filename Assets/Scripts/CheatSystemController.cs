using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CheatSystemController : MonoBehaviour
{
    public static CheatSystemController instance;

    [SerializeField] bool showConsole;
    bool showHelp;
    bool displayHelp;

    string input;

    
    public static CheatSystemCommand WIND_SPELL;
    public static CheatSystemCommand ICE_SPELL;

    public static CheatSystemCommand HELP;

    public List<object> cheatList;

    Vector2 scroll;
    GUI style;

    public GameObject invalidCheat;

    private void Awake()
    {
        WIND_SPELL = new CheatSystemCommand("wind_spell", "Gives the player the ability to summon wind spell", "wind_spell", () =>
        {
            CheatUnlocks.instance.UnlockWindSpell();
        });
        
        ICE_SPELL = new CheatSystemCommand("ice_spell", "Gives the player the ability to summon ice spell", "ice_spell", () =>
        {
            CheatUnlocks.instance.UnlockIceSpell();
        });

        HELP = new CheatSystemCommand("help", "Shows a list of commands", "help", () =>
        {
            showHelp = true;
        });

        cheatList = new List<object>
        {
            HELP,
        };
    }

    private void Start()
    {
        instance = this;
    }

    private void Update()
    {
        DisplayCheatBox();
        SubmitCheat();
    }
    private void OnAnimatorIK(int layerIndex)
    {
        
    }

    private void OnGUI()
    {
        if (!showConsole) { displayHelp = false; return; }

        float y = 0f;
        if (displayHelp)
        {
            if (showHelp)
            {
                GUI.Box(new Rect(0, y, Screen.width, 100), "");

                Rect viewport = new Rect(0, 0, Screen.width - 30, 40 * cheatList.Count);

                scroll = GUI.BeginScrollView(new Rect(0, y + 5f, Screen.width, 90), scroll, viewport);

                for (int i = 0; i < cheatList.Count; i++)
                {
                    CheatSystemCommandBase cheat = cheatList[i] as CheatSystemCommandBase;

                    string label = $"{cheat.cheatFormat} - {cheat.cheatDescription}";

                    Rect labelRect = new Rect(5, 40 * i, viewport.width - 100, 50);

                    GUI.skin.label.fontSize = 30;

                    GUI.Label(labelRect, label);

                }

                GUI.EndScrollView();
                y += 100;
            }
        }

        GUI.Box(new Rect(0, y, Screen.width, 50), "");
        GUI.backgroundColor = new Color(0, 0, 0);
        GUI.skin.textField.fontSize = 30;
        input = GUI.TextField(new Rect(10f, y + 5f, Screen.width - 20f, 50f), input);
        Input.eatKeyPressOnTextFieldFocus = false;
    }



    private void HandleInput()
    {
        for (int i = 0; i < cheatList.Count; i++)
        {
            CheatSystemCommandBase commandBase = cheatList[i] as CheatSystemCommandBase;

            if (input.Contains(commandBase.cheatID))
            {
                if (cheatList[i] as CheatSystemCommand != null)
                {
                    (cheatList[i] as CheatSystemCommand).Invoke();
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
        if (Input.GetKeyDown("`"))
        {
            showConsole = !showConsole;
            //Debug.Log("` Pressed");
        }
    }

    private void SubmitCheat()
    {
        if (showConsole)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                HandleInput();
                input = "";
                displayHelp = true;
            }
        }
    }
    
    public void AddToListWindSpell()
    {
        cheatList.Add(WIND_SPELL);
    }
    public void AddToListIceSpell()
    {
        cheatList.Add(ICE_SPELL);
    }
}
