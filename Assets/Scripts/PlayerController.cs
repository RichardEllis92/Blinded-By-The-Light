using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;

    public float moveSpeed;
    private Vector2 _moveInput;

    [FormerlySerializedAs("theRB")] public Rigidbody2D theRb;
    
    public Animator anim;

    [FormerlySerializedAs("bodySR")] public SpriteRenderer bodySr;

    private float _activeMoveSpeed;
    public float dashSpeed = 8f, dashLength = 0.5f, dashCooldown = 1f, dashInvincibility = 0.5f;
    [HideInInspector]
    public float dashCounter;
    private float _dashCoolCounter;

    public bool animationOverride;

    [HideInInspector]
    public bool canMove = true;

    [SerializeField] private DialogueUI dialogueUI;

    public DialogueUI DialogueUI => dialogueUI;

    public IInteractable Interactable { get; set; }

    private int _experiencePoints = 0;
    private static readonly int IsRolling = Animator.StringToHash("isRolling");
    private static readonly int IsMovingRight = Animator.StringToHash("isMovingRight");
    private static readonly int IsMovingRightBackwards = Animator.StringToHash("isMovingRightBackwards");
    private static readonly int IsMovingLeft = Animator.StringToHash("isMovingLeft");
    private static readonly int IsMovingLeftBackwards = Animator.StringToHash("isMovingLeftBackwards");
    private static readonly int IsMovingUp = Animator.StringToHash("isMovingUp");
    private static readonly int IsMovingUpBackwards = Animator.StringToHash("isMovingUpBackwards");
    private static readonly int IsMovingDown = Animator.StringToHash("isMovingDown");
    private static readonly int IsMovingDownBackwards = Animator.StringToHash("isMovingDownBackwards");

    public bool isRolling;
    private void Awake()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;
        Instance = this;

        if(sceneName != "Luci Room")
        {
            DontDestroyOnLoad(gameObject);
        }  
    }

    // Start is called before the first frame update
    void Start()
    {
        //theCam = Camera.main;

        _activeMoveSpeed = moveSpeed;
    }

    
    // Update is called once per frame
    void Update()
    {
        if (!CheatSystemController.Instance.showConsole)
        {
            PlayerMove();
            StopPlayer();
            Interact();
        }
    }

    void PlayerMove()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;

        if (dialogueUI.IsOpen)
        {
            animationOverride = true;
            theRb.velocity = Vector3.zero;
            anim.Play("Player_Idle");
            anim.enabled = false;
            theRb.velocity = Vector3.zero;
        }
        else
        {
            animationOverride = false;
            anim.enabled = true;
        }

        if (canMove && !LevelManager.Instance.isPaused && !dialogueUI.IsOpen)
        {
            _moveInput.x = Input.GetAxisRaw("Horizontal");
            _moveInput.y = Input.GetAxisRaw("Vertical");

            _moveInput.Normalize();

            theRb.velocity = _moveInput * _activeMoveSpeed;


            if (Input.GetKeyDown(KeyCode.Space) && sceneName != "Luci Room")
            {
                if (_dashCoolCounter <= 0 && dashCounter <= 0)
                {
                    _activeMoveSpeed = dashSpeed;
                    dashCounter = dashLength;

                    anim.SetTrigger(IsRolling);

                    PlayerHealthController.Instance.MakeInvincible(dashInvincibility);

                    AudioManager.Instance.PlaySfx(6);
                }
            }

            if (this.anim.GetCurrentAnimatorStateInfo(0).IsName("IsRolling"))
            {
                isRolling = true;
            }
            else
            {
                isRolling = false;
            }
            
            if (dashCounter > 0)
            {
                dashCounter -= Time.deltaTime;
                if (dashCounter <= 0)
                {
                    _activeMoveSpeed = moveSpeed;
                    _dashCoolCounter = dashCooldown;
                }
            }

            if (_dashCoolCounter > 0)
            {
                _dashCoolCounter -= Time.deltaTime;
            }

            if (!animationOverride)
            {
                if (_moveInput.x > 0 && (!Input.GetKeyDown(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.LeftArrow)))
                {
                    anim.SetBool(IsMovingRight, true);
                }
                else if (anim.GetCurrentAnimatorStateInfo(0).IsName("Player_Walk_Right_Backwards") && Input.GetKey(KeyCode.RightArrow))
                {
                    anim.SetBool(IsMovingRightBackwards, false);
                }
                else if (_moveInput.x > 0 && (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKey(KeyCode.LeftArrow)))
                {
                    anim.SetBool(IsMovingRightBackwards, true);
                    anim.SetBool(IsMovingRight, false);
                }
                else
                {
                    anim.SetBool(IsMovingRight, false);
                    anim.SetBool(IsMovingRightBackwards, false);
                }
                if (_moveInput.x < 0 && (!Input.GetKeyDown(KeyCode.RightArrow) && !Input.GetKey(KeyCode.RightArrow)))
                {
                    anim.SetBool(IsMovingLeft, true);
                }
                else if (anim.GetCurrentAnimatorStateInfo(0).IsName("Player_Walk_Left_Backwards") && Input.GetKey(KeyCode.LeftArrow))
                {
                    anim.SetBool(IsMovingLeftBackwards, false);
                }
                else if (_moveInput.x < 0 && (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKey(KeyCode.RightArrow)))
                {
                    anim.SetBool(IsMovingLeftBackwards, true);
                    anim.SetBool(IsMovingLeft, false);
                }
                else
                {
                    anim.SetBool(IsMovingLeft, false);
                    anim.SetBool(IsMovingLeftBackwards, false);
                }
                if (_moveInput.y > 0 && (!Input.GetKeyDown(KeyCode.DownArrow) && !Input.GetKey(KeyCode.DownArrow)))
                {
                    anim.SetBool(IsMovingUp, true);
                }
                else if (anim.GetCurrentAnimatorStateInfo(0).IsName("Player_Walk_Up_Backwards") && Input.GetKey(KeyCode.UpArrow))
                {
                    anim.SetBool(IsMovingUpBackwards, false);
                }
                else if (_moveInput.y > 0 && (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKey(KeyCode.DownArrow)))
                {
                    anim.SetBool(IsMovingUp, false);
                    anim.SetBool(IsMovingUpBackwards, true);
                }
                else
                {
                    anim.SetBool(IsMovingUp, false);
                    anim.SetBool(IsMovingUpBackwards, false);
                }
                if (_moveInput.y < 0 && (!Input.GetKeyDown(KeyCode.UpArrow) && !Input.GetKey(KeyCode.UpArrow)))
                {
                    anim.SetBool(IsMovingDown, true);
                    //anim.SetBool("isMovingUpBackwards", false);
                }
                else if (anim.GetCurrentAnimatorStateInfo(0).IsName("Player_Walk_Down_Backwards") && Input.GetKey(KeyCode.DownArrow))
                {
                    anim.SetBool(IsMovingDownBackwards, false);
                }
                else if (_moveInput.y < 0 && (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKey(KeyCode.UpArrow)))
                {
                    anim.SetBool(IsMovingDown, false);
                    anim.SetBool(IsMovingDownBackwards, true);
                    //Play moving up backwards anim
                }
                else
                {
                    anim.SetBool(IsMovingDown, false);
                    anim.SetBool(IsMovingDownBackwards, false);
                }
            }
        }
    }

    void StopPlayer()
    {
        if (dialogueUI.IsOpen)
        {
            anim.Play("Player_Idle");
            theRb.velocity = Vector3.zero;
        }
    }

    void Interact()
    {
        if (Input.GetKeyDown(KeyCode.E) && dialogueUI.IsOpen == false)
        {

            if (Interactable != null)
            {
                Interactable.Interact(this);
            }

        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Level Exit"))
        {
            anim.Play("Player_Idle");
            theRb.velocity = Vector3.zero;
        }
    }
    public Vector3 GetVelocity()
    {
        return theRb.velocity;
    }
}
