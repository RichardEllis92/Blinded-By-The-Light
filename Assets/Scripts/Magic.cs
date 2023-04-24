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
    
    private float _shotCounter;
    public float timeBetweenSpells = 1f;

    public bool spellMovingUp;
    public bool spellMovingDown;
    
    [SerializeField] private DialogueUI dialogueUI;
    private static readonly int IsMovingUp = Animator.StringToHash("isMovingUp");
    private static readonly int IsMovingDown = Animator.StringToHash("isMovingDown");
    private static readonly int IsMovingLeft = Animator.StringToHash("isMovingLeft");
    private static readonly int IsMovingRight = Animator.StringToHash("isMovingRight");
    public DialogueUI DialogueUI => dialogueUI;

    private void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;
        
        if (PlayerController.Instance.canMove && !PlayerController.Instance.animationOverride && !LevelManager.Instance.isPaused && !dialogueUI.IsOpen && sceneName != "Luci Room")
        {
            if (_shotCounter > 0)
            {
                _shotCounter -= Time.deltaTime;
            }
            
            else if (Input.GetKey(KeyCode.UpArrow))
            {
                spellMovingUp = true;
                if (PlayerController.Instance.anim.GetCurrentAnimatorStateInfo(0).IsName("Player_Idle_Right") || PlayerController.Instance.anim.GetCurrentAnimatorStateInfo(0).IsName("Player_Idle_Left") || PlayerController.Instance.anim.GetCurrentAnimatorStateInfo(0).IsName("Player_Idle") || PlayerController.Instance.anim.GetCurrentAnimatorStateInfo(0).IsName("isMovingUpBackwards"))
                {
                    PlayerController.Instance.anim.SetBool(IsMovingUp, true);
                    
                }
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
                AudioManager.Instance.PlaySfx(9);
            }
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                spellMovingDown = true;
                if (PlayerController.Instance.anim.GetCurrentAnimatorStateInfo(0).IsName("Player_Idle_Right") || PlayerController.Instance.anim.GetCurrentAnimatorStateInfo(0).IsName("Player_Idle_Left") || PlayerController.Instance.anim.GetCurrentAnimatorStateInfo(0).IsName("Player_Idle_Back") || PlayerController.Instance.anim.GetCurrentAnimatorStateInfo(0).IsName("isMovingDownBackwards"))
                {
                    PlayerController.Instance.anim.SetBool(IsMovingDown, true);
                }
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
                AudioManager.Instance.PlaySfx(9);
            }
            else if (Input.GetKey(KeyCode.LeftArrow))
            {
                if (PlayerController.Instance.anim.GetCurrentAnimatorStateInfo(0).IsName("Player_Idle_Up") || PlayerController.Instance.anim.GetCurrentAnimatorStateInfo(0).IsName("Player_Idle_Down") || PlayerController.Instance.anim.GetCurrentAnimatorStateInfo(0).IsName("Player_Idle_Back") || PlayerController.Instance.anim.GetCurrentAnimatorStateInfo(0).IsName("isMovingLeftBackwards"))
                {
                    PlayerController.Instance.anim.SetBool(IsMovingLeft, true);
                }
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
                AudioManager.Instance.PlaySfx(9);
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                if (PlayerController.Instance.anim.GetCurrentAnimatorStateInfo(0).IsName("Player_Idle_Up") || PlayerController.Instance.anim.GetCurrentAnimatorStateInfo(0).IsName("Player_Idle_Down") || PlayerController.Instance.anim.GetCurrentAnimatorStateInfo(0).IsName("Player_Idle_Back") || PlayerController.Instance.anim.GetCurrentAnimatorStateInfo(0).IsName("isMovingRightBackwards"))
                {
                    PlayerController.Instance.anim.SetBool(IsMovingRight, true);
                }
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
                AudioManager.Instance.PlaySfx(9);
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
}
