using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    public float moveSpeed;
    private Vector2 moveInput;

    public Rigidbody2D theRB;

    public Transform gunArm;

    public Animator anim;

    /*public GameObject bulletToFire;
    public Transform firePoint;

    public float timeBetweenShots;
    private float shotCounter;*/

    public SpriteRenderer bodySR;

    private float activeMoveSpeed;
    public float dashSpeed = 8f, dashLength = 0.5f, dashCooldown = 1f, dashInvincibility = 0.5f;
    [HideInInspector]
    public float dashCounter;
    private float dashCoolCounter;

    public bool animationOverride = false;

    [HideInInspector]
    public bool canMove = true;

    [SerializeField] private DialogueUI dialogueUI;

    public DialogueUI DialogueUI => dialogueUI;

    public IInteractable Interactable { get; set; }

    private int experiencePoints = 0;

    private void Awake()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;
        instance = this;

        if(sceneName != "Luci Room")
        {
            DontDestroyOnLoad(gameObject);
        }  
    }

    // Start is called before the first frame update
    void Start()
    {
        //theCam = Camera.main;

        activeMoveSpeed = moveSpeed;
    }

    
    // Update is called once per frame
    void Update()
    {
        PlayerMove();
        StopPlayer();
        Interact();
    }

    void PlayerMove()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;

        if (dialogueUI.isOpen)
        {
            animationOverride = true;
            theRB.velocity = Vector3.zero;
            anim.Play("Player_Idle");
            anim.enabled = false;
            theRB.velocity = Vector3.zero;
        }
        else
        {
            animationOverride = false;
            anim.enabled = true;
        }

        if (canMove && !LevelManager.instance.isPaused && !dialogueUI.isOpen)
        {
            moveInput.x = Input.GetAxisRaw("Horizontal");
            moveInput.y = Input.GetAxisRaw("Vertical");

            moveInput.Normalize();

            theRB.velocity = moveInput * activeMoveSpeed;


            if (Input.GetKeyDown(KeyCode.Space) && sceneName != "Luci Room")
            {
                if (dashCoolCounter <= 0 && dashCounter <= 0)
                {
                    activeMoveSpeed = dashSpeed;
                    dashCounter = dashLength;

                    anim.SetTrigger("isRolling");

                    PlayerHealthController.instance.MakeInvincible(dashInvincibility);

                    AudioManager.instance.PlaySFX(6);
                }
            }

            if (dashCounter > 0)
            {
                dashCounter -= Time.deltaTime;
                if (dashCounter <= 0)
                {
                    activeMoveSpeed = moveSpeed;
                    dashCoolCounter = dashCooldown;
                }
            }

            if (dashCoolCounter > 0)
            {
                dashCoolCounter -= Time.deltaTime;
            }

            if (animationOverride)
            {
                return;
            }
            else
            {
                if (moveInput.x > 0 && (!Input.GetKeyDown(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.LeftArrow)))
                {
                    anim.SetBool("isMovingRight", true);
                }
                else if (anim.GetCurrentAnimatorStateInfo(0).IsName("Player_Walk_Right_Backwards") && Input.GetKey(KeyCode.RightArrow))
                {
                    anim.SetBool("isMovingRightBackwards", false);
                }
                else if (moveInput.x > 0 && (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKey(KeyCode.LeftArrow)))
                {
                    anim.SetBool("isMovingRightBackwards", true);
                    anim.SetBool("isMovingRight", false);
                }
                else
                {
                    anim.SetBool("isMovingRight", false);
                    anim.SetBool("isMovingRightBackwards", false);
                }
                if (moveInput.x < 0 && (!Input.GetKeyDown(KeyCode.RightArrow) && !Input.GetKey(KeyCode.RightArrow)))
                {
                    anim.SetBool("isMovingLeft", true);
                }
                else if (anim.GetCurrentAnimatorStateInfo(0).IsName("Player_Walk_Left_Backwards") && Input.GetKey(KeyCode.LeftArrow))
                {
                    anim.SetBool("isMovingLeftBackwards", false);
                }
                else if (moveInput.x < 0 && (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKey(KeyCode.RightArrow)))
                {
                    anim.SetBool("isMovingLeftBackwards", true);
                    anim.SetBool("isMovingLeft", false);
                }
                else
                {
                    anim.SetBool("isMovingLeft", false);
                    anim.SetBool("isMovingLeftBackwards", false);
                }
                if (moveInput.y > 0 && (!Input.GetKeyDown(KeyCode.DownArrow) && !Input.GetKey(KeyCode.DownArrow)))
                {
                    anim.SetBool("isMovingUp", true);
                }
                else if (anim.GetCurrentAnimatorStateInfo(0).IsName("Player_Walk_Up_Backwards") && Input.GetKey(KeyCode.UpArrow))
                {
                    anim.SetBool("isMovingUpBackwards", false);
                }
                else if (moveInput.y > 0 && (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKey(KeyCode.DownArrow)))
                {
                    anim.SetBool("isMovingUp", false);
                    anim.SetBool("isMovingUpBackwards", true);
                }
                else
                {
                    anim.SetBool("isMovingUp", false);
                    anim.SetBool("isMovingUpBackwards", false);
                }
                if (moveInput.y < 0 && (!Input.GetKeyDown(KeyCode.UpArrow) && !Input.GetKey(KeyCode.UpArrow)))
                {
                    anim.SetBool("isMovingDown", true);
                    //anim.SetBool("isMovingUpBackwards", false);
                }
                else if (anim.GetCurrentAnimatorStateInfo(0).IsName("Player_Walk_Down_Backwards") && Input.GetKey(KeyCode.DownArrow))
                {
                    anim.SetBool("isMovingDownBackwards", false);
                }
                else if (moveInput.y < 0 && (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKey(KeyCode.UpArrow)))
                {
                    anim.SetBool("isMovingDown", false);
                    anim.SetBool("isMovingDownBackwards", true);
                    //Play moving up backwards anim
                }
                else
                {
                    anim.SetBool("isMovingDown", false);
                    anim.SetBool("isMovingDownBackwards", false);
                }

            }
        }
    }

    void StopPlayer()
    {
        if (dialogueUI.isOpen)
        {
            anim.Play("Player_Idle");
            theRB.velocity = Vector3.zero;
        }
    }

    void Interact()
    {
        if (Input.GetKeyDown(KeyCode.E) && dialogueUI.isOpen == false)
        {

            if (Interactable != null)
            {
                Interactable.Interact(this);
            }

        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Level Exit")
        {
            anim.Play("Player_Idle");
            theRB.velocity = Vector3.zero;
        }
    }
    
}
