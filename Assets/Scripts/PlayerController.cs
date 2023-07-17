using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using Rewired;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;
    
    public int playerId = 0;
    private Player player;
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

    [SerializeField]
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
    [System.NonSerialized] // Don't serialize this so the value is lost on an editor script recompile.
    private bool initialized;
    
    private Vector3 previousPosition;
    private Vector3 currentPosition;
    
    private void Awake()
    {
        Instance = this;
    }
    private void Initialize() {
        // Get the Rewired Player object for this player.
        player = ReInput.players.GetPlayer(playerId);
            
        initialized = true;
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
        if(!ReInput.isReady) return; // Exit if Rewired isn't ready. This would only happen during a script recompile in the editor.
        if(!initialized) Initialize(); // Reinitialize after a recompile in the editor
        
        if (LevelManager.Instance.isPaused)
        {
            theRb.velocity = Vector2.zero;
            anim.speed = 0;
            return;
        }

        if (!LevelManager.Instance.isPaused)
        {
            anim.speed = 1;
        }
        
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

        currentPosition = transform.position;
        
        if (dialogueUI.IsOpen)
        {
            animationOverride = true;
            theRb.velocity = Vector3.zero;
            
            if (sceneName == "Luci Room Doll" || sceneName == "Boss" || sceneName == "BossFail")
            {
                anim.Play("Doll_Idle");
            }
            else
            {
                anim.Play("Player_Idle");
            }
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
            // Get the raw input values
            float horizontalInput = player.GetAxisRaw("HAIL");
            float verticalInput = player.GetAxisRaw("SATAN");

            // Normalize the input vector
            Vector2 moveInput = new Vector2(horizontalInput, verticalInput).normalized;

            // Check if the player is moving
            bool isMoving = moveInput.magnitude > 0.01f;

            // Update the player's velocity
            if (isMoving)
            {
                theRb.velocity = moveInput * _activeMoveSpeed;
            }
            else
            {
                theRb.velocity = Vector2.zero;
            }

            // Update the player's animation based on the movement direction
            if (!animationOverride)
            {
                if (isMoving)
                {
                    anim.SetFloat(IsMovingRight, Mathf.Clamp(moveInput.x, 0f, 1f));
                    anim.SetFloat(IsMovingRightBackwards, Mathf.Clamp(moveInput.x, -1f, 0f));
                    anim.SetFloat(IsMovingLeft, Mathf.Clamp(-moveInput.x, 0f, 1f));
                    anim.SetFloat(IsMovingLeftBackwards, Mathf.Clamp(-moveInput.x, -1f, 0f));
                    anim.SetFloat(IsMovingUp, Mathf.Clamp(moveInput.y, 0f, 1f));
                    anim.SetFloat(IsMovingUpBackwards, Mathf.Clamp(moveInput.y, -1f, 0f));
                    anim.SetFloat(IsMovingDown, Mathf.Clamp(-moveInput.y, 0f, 1f));
                    anim.SetFloat(IsMovingDownBackwards, Mathf.Clamp(-moveInput.y, -1f, 0f));
                }
                else
                {
                    anim.SetFloat(IsMovingRight, 0f);
                    anim.SetFloat(IsMovingRightBackwards, 0f);
                    anim.SetFloat(IsMovingLeft, 0f);
                    anim.SetFloat(IsMovingLeftBackwards, 0f);
                    anim.SetFloat(IsMovingUp, 0f);
                    anim.SetFloat(IsMovingUpBackwards, 0f);
                    anim.SetFloat(IsMovingDown, 0f);
                    anim.SetFloat(IsMovingDownBackwards, 0f);
                }
            }

            if (player.GetButtonDown("Roll") && (sceneName != "Luci Room" && sceneName != "Luci Room Complete" && sceneName != "Luci Room Doll"))
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
        if (player.GetButtonDown("Action") && dialogueUI.IsOpen == false)
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
    
    bool IsPlayerMoving()
    {
        float distance = Vector3.Distance(previousPosition, currentPosition);
        return distance > 0.0f; // Adjust the threshold as per your needs
    }
}
