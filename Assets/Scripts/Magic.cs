using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Rewired;

public class Magic : MonoBehaviour
{
    public enum SpellType
    {
        Fire,
        Ice,
        Wind
    }

    public static Magic Instance;

    public SpellType currentSpell = SpellType.Fire;

    public GameObject fireUnlocked;
    public GameObject windUnlocked;
    public GameObject iceUnlocked;

    public GameObject fireSpell;
    public GameObject iceSpell;
    public GameObject windSpell;
    public GameObject windSpellUp;
    public GameObject windSpellDown;
    public GameObject impactEffect;
    [SerializeField] private Transform firePointUp, firePointDown, firePointLeft, firePointRight;

    public float _shotCounter;
    public float timeBetweenSpells;
    public float fireTime = 0.8f;
    public float windTime = 0.2f;
    public float iceTime = 0f;

    public bool spellMovingUp;
    public bool spellMovingDown;

    [SerializeField] private DialogueUI dialogueUI;
    private static readonly int IsMovingUp = Animator.StringToHash("isMovingUp");
    private static readonly int IsMovingDown = Animator.StringToHash("isMovingDown");
    private static readonly int IsMovingLeft = Animator.StringToHash("isMovingLeft");
    private static readonly int IsMovingRight = Animator.StringToHash("isMovingRight");
    public DialogueUI DialogueUI => dialogueUI;

    public bool multipleArrows;
    public string currentSpellCheck;
    
    [System.NonSerialized] // Don't serialize this so the value is lost on an editor script recompile.
    private bool initialized;
    public int playerId = 0;
    private Player player;

    private void Awake()
    {
        Instance = this;
    }
    
    private void Initialize() {
        // Get the Rewired Player object for this player.
        player = ReInput.players.GetPlayer(playerId);
            
        initialized = true;
    }

    private void Start()
    {
        switch (currentSpell)
        {
            case SpellType.Fire:
                timeBetweenSpells = fireTime;
                fireUnlocked.SetActive(true);
                windUnlocked.SetActive(false);
                iceUnlocked.SetActive(false);
                break;
            case SpellType.Wind:
                timeBetweenSpells = windTime;
                fireUnlocked.SetActive(false);
                windUnlocked.SetActive(true);
                iceUnlocked.SetActive(false);
                break;
            case SpellType.Ice:
                timeBetweenSpells = windTime;
                fireUnlocked.SetActive(false);
                windUnlocked.SetActive(false);
                iceUnlocked.SetActive(true);
                break;
        }
    }

    void Update()
    {
        if(!ReInput.isReady) return; // Exit if Rewired isn't ready. This would only happen during a script recompile in the editor.
        if(!initialized) Initialize(); // Reinitialize after a recompile in the editor

        ActiveSpell();
        Scene currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;
        MultipleArrowKeys();
        if (multipleArrows)
        {
            return;
        }

        if (PlayerController.Instance.canMove && !PlayerController.Instance.animationOverride &&
            !LevelManager.Instance.isPaused && !dialogueUI.IsOpen && (sceneName != "Luci Room" &&
                                                                      sceneName != "Luci Room Complete" &&
                                                                      sceneName != "Luci Room Doll"))
        {
            if (_shotCounter > 0)
            {
                _shotCounter -= Time.deltaTime;
            }

            else if (player.GetButton("Fire Up"))
            {
                spellMovingUp = true;
                switch (currentSpell)
                {
                    case SpellType.Fire:
                        Instantiate(fireSpell, firePointUp.position, Quaternion.Euler(new Vector3(0, 0, 90)));
                        break;
                    case SpellType.Ice:
                        if(!EnemyController.Instance.isFrozen)
                            Instantiate(iceSpell, firePointUp.position, Quaternion.Euler(new Vector3(0, 0, 90)));
                        break;
                    case SpellType.Wind:
                        Instantiate(windSpellUp, firePointUp.position, Quaternion.Euler(new Vector3(0, 0, 90)));
                        break;
                }

                _shotCounter = timeBetweenSpells;
                if (currentSpell == SpellType.Fire)
                {
                    AudioManager.Instance.PlaySfx(9);
                }
                else if (currentSpell == SpellType.Ice)
                {
                    AudioManager.Instance.PlaySfx(15);
                }
                else if (currentSpell == SpellType.Wind)
                {
                    AudioManager.Instance.PlaySfx(16);
                }

            }
            else if (player.GetButton("Fire Down"))
            {
                spellMovingDown = true;
                switch (currentSpell)
                {
                    case SpellType.Fire:
                        Instantiate(fireSpell, firePointDown.position, Quaternion.Euler(new Vector3(0, 0, -90)));
                        break;
                    case SpellType.Ice:
                        if(!EnemyController.Instance.isFrozen)
                            Instantiate(iceSpell, firePointDown.position, Quaternion.Euler(new Vector3(0, 0, -90)));
                        break;
                    case SpellType.Wind:
                        Instantiate(windSpellDown, firePointDown.position, Quaternion.Euler(new Vector3(0, 0, -90)));
                        break;
                }

                _shotCounter = timeBetweenSpells;
                if (currentSpell == SpellType.Fire)
                {
                    AudioManager.Instance.PlaySfx(9);
                }
                else if (currentSpell == SpellType.Ice)
                {
                    AudioManager.Instance.PlaySfx(15);
                }
                else if (currentSpell == SpellType.Wind)
                {
                    AudioManager.Instance.PlaySfx(16);
                }
            }
            else if (player.GetButton("Fire Left"))
            {

                switch (currentSpell)
                {
                    case SpellType.Fire:
                        Instantiate(fireSpell, firePointLeft.position, Quaternion.Euler(new Vector3(0, 0, 180)));
                        break;
                    case SpellType.Ice:
                        if(!EnemyController.Instance.isFrozen)
                            Instantiate(iceSpell, firePointLeft.position, Quaternion.Euler(new Vector3(0, 0, 180)));
                        break;
                    case SpellType.Wind:
                        Instantiate(windSpell, firePointLeft.position, Quaternion.Euler(new Vector3(180, 0, 180)));
                        break;
                }

                _shotCounter = timeBetweenSpells;
                if (currentSpell == SpellType.Fire)
                {
                    AudioManager.Instance.PlaySfx(9);
                }
                else if (currentSpell == SpellType.Ice)
                {
                    AudioManager.Instance.PlaySfx(15);
                }
                else if (currentSpell == SpellType.Wind)
                {
                    AudioManager.Instance.PlaySfx(16);
                }
            }
            else if (player.GetButton("Fire Right"))
            {
                switch (currentSpell)
                {
                    case SpellType.Fire:
                        Instantiate(fireSpell, firePointRight.position, Quaternion.identity);
                        timeBetweenSpells = fireTime;
                        break;
                    case SpellType.Ice:
                        if(!EnemyController.Instance.isFrozen)
                            Instantiate(iceSpell, firePointRight.position, Quaternion.identity);
                        timeBetweenSpells = iceTime;
                        break;
                    case SpellType.Wind:
                        Instantiate(windSpell, firePointRight.position, Quaternion.identity);
                        timeBetweenSpells = windTime;
                        break;
                }

                _shotCounter = timeBetweenSpells;
                if (currentSpell == SpellType.Fire)
                {
                    AudioManager.Instance.PlaySfx(9);
                }
                else if (currentSpell == SpellType.Ice)
                {
                    AudioManager.Instance.PlaySfx(15);
                }
                else if (currentSpell == SpellType.Wind)
                {
                    AudioManager.Instance.PlaySfx(16);
                }
            }
        }

        if (player.GetButtonDown("Toggle Spells") && (sceneName != "Luci Room" && sceneName != "Luci Room Complete" && sceneName != "Luci Room Doll"))
        {
            switch (currentSpell)
            {
                case SpellType.Fire:
                    if (CheatUnlocks.Instance._windSpellUnlocked)
                    {
                        currentSpell = SpellType.Wind;
                        timeBetweenSpells = windTime;
                        fireUnlocked.SetActive(false);
                        windUnlocked.SetActive(true);
                        iceUnlocked.SetActive(false);

                    }
                    else if (CheatUnlocks.Instance._iceSpellUnlocked)
                    {
                        currentSpell = SpellType.Ice;
                        timeBetweenSpells = iceTime;
                        fireUnlocked.SetActive(false);
                        windUnlocked.SetActive(false);
                        iceUnlocked.SetActive(true);
                    }
                    else
                    {
                        currentSpell = SpellType.Fire;
                        timeBetweenSpells = fireTime;
                        fireUnlocked.SetActive(true);
                        windUnlocked.SetActive(false);
                        iceUnlocked.SetActive(false);
                    }

                    break;
                case SpellType.Wind:
                    if (CheatUnlocks.Instance._iceSpellUnlocked)
                    {
                        currentSpell = SpellType.Ice;
                        timeBetweenSpells = iceTime;
                        fireUnlocked.SetActive(false);
                        windUnlocked.SetActive(false);
                        iceUnlocked.SetActive(true);
                    }
                    else
                    {
                        currentSpell = SpellType.Fire;
                        timeBetweenSpells = fireTime;
                        fireUnlocked.SetActive(true);
                        windUnlocked.SetActive(false);
                        iceUnlocked.SetActive(false);
                    }

                    break;
                case SpellType.Ice:
                    currentSpell = SpellType.Fire;
                    timeBetweenSpells = fireTime;
                    fireUnlocked.SetActive(true);
                    windUnlocked.SetActive(false);
                    iceUnlocked.SetActive(false);
                    break;

            }
        }
    }

    void MultipleArrowKeys()
    {
        bool up = player.GetButton("Fire Up");
        bool down = player.GetButton("Fire Down");
        bool left = player.GetButton("Fire Left");
        bool right = player.GetButton("Fire Right");

        if (up)
        {
            if (down || left || right)
            {
                multipleArrows = true;
            }
        }
        else if (down)
        {
            if (left || right)
            {
                multipleArrows = true;
            }
        }
        else if (left && right)
        {
            multipleArrows = true;
        }
        else
        {
            multipleArrows = false;
        }
    }

    void ActiveSpell()
    {
        if (currentSpell == SpellType.Fire)
        {
            currentSpellCheck = "Fire";
        }
        else if (currentSpell == SpellType.Wind)
        {
            currentSpellCheck = "Wind";
        }
        else
        {
            currentSpellCheck = "Ice";
        }
    }

}
