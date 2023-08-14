using System;
using UnityEngine;
using Rewired;

public class PlayerAnimations : MonoBehaviour
{
    private Vector2 _moveInput;
    public Animator anim;
    [SerializeField] private DialogueUI dialogueUI;
    public int playerId = 0;
    private Player player;
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
    
    [System.NonSerialized] // Don't serialize this so the value is lost on an editor script recompile.
    private bool initialized;
    
    private enum MovementDirection
    {
        None,
        Up,
        Down,
        Left,
        Right
    }
    
    private MovementDirection currentDirection = MovementDirection.None;

    private void Initialize() {
        // Get the Rewired Player object for this player.
        player = ReInput.players.GetPlayer(playerId);
            
        initialized = true;
    }
    void Update()
    {
        if(!ReInput.isReady) return; // Exit if Rewired isn't ready. This would only happen during a script recompile in the editor.
        if(!initialized) Initialize(); // Reinitialize after a recompile in the editor
        GetMovementInput();
        Animations();
    }

    void GetMovementInput()
    {
        _moveInput.x = player.GetAxisRaw("Horizontal"); // get input by name or action id
        _moveInput.y = player.GetAxisRaw("Vertical");

        if (_moveInput.x == 0 && _moveInput.y == 0)
        {
            currentDirection = MovementDirection.None;
        }
        else if (_moveInput.x > 0)
        {
            currentDirection = MovementDirection.Right;
        }
        else if (_moveInput.x < 0)
        {
            currentDirection = MovementDirection.Left;
        }
        else if (_moveInput.y > 0)
        {
            currentDirection = MovementDirection.Up;
        }
        else if (_moveInput.y < 0)
        {
            currentDirection = MovementDirection.Down;
        }
    }

    void Animations()
    {
        _moveInput.x = player.GetAxisRaw("Horizontal"); // get input by name or action id
        _moveInput.y = player.GetAxisRaw("Vertical");

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

            if (Magic.Instance.multipleArrows)
            {
                if (_moveInput.x > 0)
                {
                    anim.SetBool(IsMovingRight, true);
                    anim.SetBool(IsMovingLeft, false);
                    anim.SetBool(IsMovingUp, false);
                    anim.SetBool(IsMovingDown, false);
                }
                else if (_moveInput.x < 0)
                {
                    anim.SetBool(IsMovingLeft, true);
                    anim.SetBool(IsMovingRight, false);
                    anim.SetBool(IsMovingUp, false);
                    anim.SetBool(IsMovingDown, false);
                }
                else if (_moveInput.y > 0)
                {
                    anim.SetBool(IsMovingUp, true);
                    anim.SetBool(IsMovingLeft, false);
                    anim.SetBool(IsMovingRight, false);
                    anim.SetBool(IsMovingDown, false);
                }
                else if (_moveInput.y < 0)
                {
                    anim.SetBool(IsMovingDown, true);
                    anim.SetBool(IsMovingUp, false);
                    anim.SetBool(IsMovingLeft, false);
                    anim.SetBool(IsMovingRight, false);
                }
                else
                {
                    anim.SetBool(IsMovingDown, false);
                    anim.SetBool(IsMovingUp, false);
                    anim.SetBool(IsMovingLeft, false);
                    anim.SetBool(IsMovingRight, false);
                }
                return;
            }

            if (_moveInput.x > 0 && _moveInput.y == 0 && !player.GetButton("Fire Left"))
            {
                anim.SetBool(IsMovingRight, true);
            }
                
            else if (_moveInput.x > 0 && player.GetButton("Fire Left"))
            {
                anim.SetBool(IsMovingRightBackwards, true);
                anim.SetBool(IsMovingRight, false);
            }
            else if (_moveInput.x > 0 && player.GetButton("Fire Right") && anim.GetCurrentAnimatorStateInfo(0).IsName("Player_Walk_Right_Backwards"))
            {
                anim.SetBool(IsMovingRight, true);
                anim.SetBool(IsMovingRightBackwards, false);
            }
            else if (_moveInput.x > 0 && player.GetButton("Fire Right"))
            {
                anim.SetBool(IsMovingRightBackwards, false);
            }
            else if (_moveInput is { y: 0, x: 0 } && player.GetButton("Fire Left"))
            {
                anim.Play("Player_Idle_Left");
            }
            else
            {
                anim.SetBool(IsMovingRight, false);
                anim.SetBool(IsMovingRightBackwards, false);
            }

            if (_moveInput.x < 0 &&  !player.GetButton("Fire Right"))
            {
                anim.SetBool(IsMovingLeft, true);
            }
            else if (_moveInput.x < 0 && player.GetButton("Fire Right"))
            {
                anim.SetBool(IsMovingLeftBackwards, true);
                anim.SetBool(IsMovingLeft, false);
            }
            else if (_moveInput.x < 0 && player.GetButton("Fire Right") && anim.GetCurrentAnimatorStateInfo(0).IsName("Player_Walk_Left_Backwards"))
            {
                anim.SetBool(IsMovingLeft, true);
                anim.SetBool(IsMovingLeftBackwards, false);
            }
            else if (_moveInput.x < 0 && player.GetButton("Fire Left"))
            {
                anim.SetBool(IsMovingLeftBackwards, false);
            }
            else if (_moveInput is { y: 0, x: 0 } && player.GetButton("Fire Right"))
            {
                anim.Play("Player_Idle_Right");
            }
            else
            {
                anim.SetBool(IsMovingLeft, false);
                anim.SetBool(IsMovingLeftBackwards, false);
            }

            if (_moveInput.y > 0 && (!player.GetButton("Fire Down")))
            {
                anim.SetBool(IsMovingUp, true);
            }
            else if (_moveInput.y > 0 && (player.GetButton("Fire Down")))
            {
                anim.SetBool(IsMovingUp, false);
                anim.SetBool(IsMovingUpBackwards, true);
            }
            else if (_moveInput.y > 0 && player.GetButton("Fire Down") && anim.GetCurrentAnimatorStateInfo(0).IsName("Player_Walk_Up_Backwards"))
            {
                anim.SetBool(IsMovingDown, true);
                anim.SetBool(IsMovingDownBackwards, false);
            }
            else if (_moveInput.y > 0 && player.GetButton("Fire Up"))
            {
                anim.SetBool(IsMovingUpBackwards, false);
            }
            else if (_moveInput is { y: 0, x: 0 } && (player.GetButton("Fire Down")))
            {
                anim.Play("Player_Idle");
            }
            else
            {
                anim.SetBool(IsMovingUp, false);
                anim.SetBool(IsMovingUpBackwards, false);
            }

            if (_moveInput.y < 0 && (!player.GetButton("Fire Up")))
            {
                anim.SetBool(IsMovingDown, true);
                //anim.SetBool("isMovingUpBackwards", false);
            }
            else if (_moveInput.y < 0 && (player.GetButton("Fire Up")))
            {
                anim.SetBool(IsMovingDown, false);
                anim.SetBool(IsMovingDownBackwards, true);
                //Play moving up backwards anim
            }
            else if (_moveInput.y < 0 && player.GetButton("Fire Up") && anim.GetCurrentAnimatorStateInfo(0).IsName("Player_Walk_Down_Backwards"))
            {
                anim.SetBool(IsMovingDown, true);
                anim.SetBool(IsMovingDownBackwards, false);
            }
            else if (_moveInput.y < 0 && player.GetButton("Fire Down"))
            {
                anim.SetBool(IsMovingDownBackwards, false);
            }
            else if (_moveInput is { y: 0, x: 0 } && (player.GetButton("Fire Up")))
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
