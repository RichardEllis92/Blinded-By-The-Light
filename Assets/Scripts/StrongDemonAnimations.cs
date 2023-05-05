using System.Collections;
using UnityEngine;

public class StrongDemonAnimations : MonoBehaviour
{
    public Animator anim;
    public string playerTag = "Player";
    public float attackRange = 1f;
    
    private Vector3 lastPos;
    
    private static readonly int MovingUp = Animator.StringToHash("IsMovingUp");
    private static readonly int MovingDown = Animator.StringToHash("IsMovingDown");
    private static readonly int MovingLeft = Animator.StringToHash("IsMovingLeft");
    private static readonly int MovingRight = Animator.StringToHash("IsMovingRight");
    private static readonly int IsAttacking = Animator.StringToHash("IsAttacking");

    IEnumerator UpdateAnimation()
    {
        Vector3 currentPos = transform.position;
        Vector3 movementDirection = currentPos - lastPos;
        bool isMovingUp = false;
        bool isMovingDown = false;
        bool isMovingLeft = false;
        bool isMovingRight = false;

        if (Mathf.Abs(movementDirection.x) > Mathf.Abs(movementDirection.y))
        {
            if (movementDirection.x > 0.1)
            {
                isMovingRight = true;
            }
            else
            {
                isMovingLeft = true;
            }
        }
        else
        {
            if (movementDirection.y > 0.1)
            {
                isMovingUp = true;
            }
            else
            {
                isMovingDown = true;
            }
        }

        if (movementDirection == Vector3.zero)
        {
            anim.SetBool(MovingUp, false);
            anim.SetBool(MovingDown, false);
            anim.SetBool(MovingLeft, false);
            anim.SetBool(MovingRight, false);
        }
        else
        {
            anim.SetBool(MovingUp, isMovingUp);
            anim.SetBool(MovingDown, isMovingDown);
            anim.SetBool(MovingLeft, isMovingLeft);
            anim.SetBool(MovingRight, isMovingRight);
        }
        

        // If the object is not moving, set all the bools to false
        
        
        // Check if the enemy is touching the player
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, attackRange);
        bool isTouchingPlayer = false;
        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag(playerTag))
            {
                isTouchingPlayer = true;
                break;
            }
        }
        anim.SetBool(IsAttacking, isTouchingPlayer);

        lastPos = currentPos;

        // Wait for a short period before updating the animation again
        yield return new WaitForSeconds(0.1f); // Adjust the delay time to a suitable amount

        // Restart the coroutine to update the animation again
        StartCoroutine(UpdateAnimation());
    }

    void Start()
    {
        StartCoroutine(UpdateAnimation());
    }
}