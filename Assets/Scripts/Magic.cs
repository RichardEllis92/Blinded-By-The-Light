using System;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    public GameObject fireSpell;
    public GameObject iceSpell;
    public GameObject windSpell;
    public GameObject windSpellUp;
    public GameObject windSpellDown;
    public GameObject impactEffect;
    [SerializeField]
    private Transform firePointUp, firePointDown, firePointLeft, firePointRight;
    
    public float _shotCounter;
    public float timeBetweenSpells = 1f;

    public bool spellMovingUp;
    public bool spellMovingDown;
    
    [SerializeField] private DialogueUI dialogueUI;
    private static readonly int IsMovingUp = Animator.StringToHash("isMovingUp");
    private static readonly int IsMovingDown = Animator.StringToHash("isMovingDown");
    private static readonly int IsMovingLeft = Animator.StringToHash("isMovingLeft");
    private static readonly int IsMovingRight = Animator.StringToHash("isMovingRight");
    public DialogueUI DialogueUI => dialogueUI;

    public bool multipleArrows;

    private void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;
        MultipleArrowKeys();
        if (multipleArrows) { return;}
        
        if (PlayerController.Instance.canMove && !PlayerController.Instance.animationOverride && !LevelManager.Instance.isPaused && !dialogueUI.IsOpen && sceneName != "Luci Room")
        {
            if (_shotCounter > 0)
            {
                _shotCounter -= Time.deltaTime;
            }
            
            else if (Input.GetKey(KeyCode.UpArrow))
            {
                spellMovingUp = true;
                switch (currentSpell)
                {
                    case SpellType.Fire:
                        Instantiate(fireSpell, firePointUp.position, Quaternion.Euler(new Vector3(0, 0, 90)));
                        break;
                    case SpellType.Ice:
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
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                spellMovingDown = true;
                switch (currentSpell)
                {
                    case SpellType.Fire:
                        Instantiate(fireSpell, firePointDown.position, Quaternion.Euler(new Vector3(0, 0, -90)));
                        break;
                    case SpellType.Ice:
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
            else if (Input.GetKey(KeyCode.LeftArrow))
            {
                
                switch (currentSpell)
                {
                    case SpellType.Fire:
                        Instantiate(fireSpell, firePointLeft.position, Quaternion.Euler(new Vector3(0, 0, 180)));
                        break;
                    case SpellType.Ice:
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
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                switch (currentSpell)
                {
                    case SpellType.Fire:
                        Instantiate(fireSpell, firePointRight.position, Quaternion.identity);
                        timeBetweenSpells = 1f;
                        break;
                    case SpellType.Ice:
                        Instantiate(iceSpell, firePointRight.position, Quaternion.identity);
                        timeBetweenSpells = 2f;
                        break;
                    case SpellType.Wind:
                        Instantiate(windSpell, firePointRight.position, Quaternion.identity);
                        timeBetweenSpells = 0.2f;
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
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            switch (currentSpell)
            {
                case SpellType.Fire:
                    if (CheatUnlocks.Instance._windSpellUnlocked)
                    {
                        currentSpell = SpellType.Wind;
                        timeBetweenSpells = 0.2f;
                    }
                    else if (CheatUnlocks.Instance._iceSpellUnlocked)
                    {
                        currentSpell = SpellType.Ice;
                        timeBetweenSpells = 2f;
                    }
                    else
                    {
                        currentSpell = SpellType.Fire;
                        timeBetweenSpells = 1f;
                    }
                    break;
                case SpellType.Wind:
                    if (CheatUnlocks.Instance._iceSpellUnlocked)
                    {
                        currentSpell = SpellType.Ice;
                        timeBetweenSpells = 2f;
                    }
                    else
                    {
                        currentSpell = SpellType.Fire;
                        timeBetweenSpells = 1f;
                    }
                    break;
                case SpellType.Ice:
                    currentSpell = SpellType.Fire;
                    timeBetweenSpells = 1f;
                    break;
                
            }
        }
    }
    
    void MultipleArrowKeys()
    {
        bool up = Input.GetKey(KeyCode.UpArrow);
        bool down = Input.GetKey(KeyCode.DownArrow);
        bool left = Input.GetKey(KeyCode.LeftArrow);
        bool right = Input.GetKey(KeyCode.RightArrow);

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
}
