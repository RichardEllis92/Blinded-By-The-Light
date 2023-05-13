using System;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    private Vector2 _moveInput;
    public Animator anim;
    [SerializeField] private DialogueUI dialogueUI;
    public DialogueUI DialogueUI => dialogueUI;
    
    private static readonly int IsMovingRight = Animator.StringToHash("isMovingRight");
    private static readonly int IsMovingRightBackwards = Animator.StringToHash("isMovingRightBackwards");
    private static readonly int IsMovingLeft = Animator.StringToHash("isMovingLeft");
    private static readonly int IsMovingLeftBackwards = Animator.StringToHash("isMovingLeftBackwards");
    private static readonly int IsMovingUp = Animator.StringToHash("isMovingUp");
    private static readonly int IsMovingUpBackwards = Animator.StringToHash("isMovingUpBackwards");
    private static readonly int IsMovingDown = Animator.StringToHash("isMovingDown");
    private static readonly int IsMovingDownBackwards = Animator.StringToHash("isMovingDownBackwards");

    private bool _canShoot;
    
    private bool spellMovingUp;
    private bool spellMovingDown;
    private bool spellMovingLeft;
    private bool spellMovingRight;

    private bool multipleArrows;
    

    void Update()
    {
        Animations();
    }

    void Animations()
    {
        _moveInput.x = Input.GetAxisRaw("Horizontal");
        _moveInput.y = Input.GetAxisRaw("Vertical");

        if (Magic.Instance._shotCounter > 0)
        {
            _canShoot = false;
        }
        else
        {
            _canShoot = true;
        }

        if (PlayerController.Instance.canMove && !LevelManager.Instance.isPaused && !dialogueUI.IsOpen && !CheatSystemController.Instance.showConsole)
        {
            if (Magic.Instance.multipleArrows) { return;}
            if (_moveInput.x > 0 && _moveInput.y == 0 && !Input.GetKey(KeyCode.LeftArrow))
            {
                anim.SetBool(IsMovingRight, true);
            }
                
            else if (_moveInput.x > 0 && Input.GetKey(KeyCode.LeftArrow))
            {
                anim.SetBool(IsMovingRightBackwards, true);
                anim.SetBool(IsMovingRight, false);
            }
            else if (_moveInput.x > 0 && Input.GetKey(KeyCode.RightArrow) && anim.GetCurrentAnimatorStateInfo(0).IsName("Player_Walk_Right_Backwards"))
            {
                anim.SetBool(IsMovingRight, true);
                anim.SetBool(IsMovingRightBackwards, false);
            }
            else if (_moveInput.x > 0 && Input.GetKey(KeyCode.RightArrow))
            {
                anim.SetBool(IsMovingRightBackwards, false);
            }
            else if (_moveInput is { y: 0, x: 0 } && Input.GetKey(KeyCode.LeftArrow))
            {
                anim.Play("Player_Idle_Left");
            }
            else
            {
                anim.SetBool(IsMovingRight, false);
                anim.SetBool(IsMovingRightBackwards, false);
            }

            if (_moveInput.x < 0 &&  !Input.GetKey(KeyCode.RightArrow))
            {
                anim.SetBool(IsMovingLeft, true);
            }
            else if (_moveInput.x < 0 && Input.GetKey(KeyCode.RightArrow))
            {
                anim.SetBool(IsMovingLeftBackwards, true);
                anim.SetBool(IsMovingLeft, false);
            }
            else if (_moveInput.x < 0 && Input.GetKey(KeyCode.RightArrow) && anim.GetCurrentAnimatorStateInfo(0).IsName("Player_Walk_Left_Backwards"))
            {
                anim.SetBool(IsMovingLeft, true);
                anim.SetBool(IsMovingLeftBackwards, false);
            }
            else if (_moveInput.x < 0 && Input.GetKey(KeyCode.LeftArrow))
            {
                anim.SetBool(IsMovingLeftBackwards, false);
            }
            else if (_moveInput is { y: 0, x: 0 } && Input.GetKey(KeyCode.RightArrow))
            {
                anim.Play("Player_Idle_Right");
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
            else if (_moveInput.y > 0 && (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKey(KeyCode.DownArrow)))
            {
                anim.SetBool(IsMovingUp, false);
                anim.SetBool(IsMovingUpBackwards, true);
            }
            else if (_moveInput.y > 0 && Input.GetKey(KeyCode.DownArrow) && anim.GetCurrentAnimatorStateInfo(0).IsName("Player_Walk_Up_Backwards"))
            {
                anim.SetBool(IsMovingDown, true);
                anim.SetBool(IsMovingDownBackwards, false);
            }
            else if (_moveInput.y > 0 && Input.GetKey(KeyCode.UpArrow))
            {
                anim.SetBool(IsMovingUpBackwards, false);
            }
            else if (_moveInput is { y: 0, x: 0 } && (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKey(KeyCode.DownArrow)))
            {
                anim.Play("Player_Idle");
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
            else if (_moveInput.y < 0 && (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKey(KeyCode.UpArrow)))
            {
                anim.SetBool(IsMovingDown, false);
                anim.SetBool(IsMovingDownBackwards, true);
                //Play moving up backwards anim
            }
            else if (_moveInput.y < 0 && Input.GetKey(KeyCode.UpArrow) && anim.GetCurrentAnimatorStateInfo(0).IsName("Player_Walk_Down_Backwards"))
            {
                anim.SetBool(IsMovingDown, true);
                anim.SetBool(IsMovingDownBackwards, false);
            }
            else if (_moveInput.y < 0 && Input.GetKey(KeyCode.DownArrow))
            {
                anim.SetBool(IsMovingDownBackwards, false);
            }
            else if (_moveInput is { y: 0, x: 0 } && (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKey(KeyCode.UpArrow)))
            {
                anim.Play("Player_Idle_Back");
            }
            else
            {
                anim.SetBool(IsMovingDown, false);
                anim.SetBool(IsMovingDownBackwards, false);
            }
        }
    }
}
