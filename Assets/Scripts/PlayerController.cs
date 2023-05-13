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
    
    public float detectionRadius = 16.0f;
    public string enemyTag = "Enemy";

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

        if (canMove && !LevelManager.Instance.isPaused && !dialogueUI.IsOpen && !CheatSystemController.Instance.showConsole)
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

    public bool EnemyNearby()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, detectionRadius);

        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag(enemyTag))
            {
                return true;
            }
        }
        return false;
    }
}
