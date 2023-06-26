using System.Collections;
using UnityEngine;

public class BossAnimations : MonoBehaviour
{
    public Animator anim;
    
    private Vector3 lastPos;
    
    private static readonly int MovingUp = Animator.StringToHash("isMovingUp");
    private static readonly int MovingDown = Animator.StringToHash("isMovingDown");
    private static readonly int MovingLeft = Animator.StringToHash("isMovingLeft");
    private static readonly int MovingRight = Animator.StringToHash("isMovingRight");
    private static readonly int PlayerMagic = Animator.StringToHash("PlayerMagic");

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

        if (!BossController.Instance.bossIsDead)
        {
            if (movementDirection == Vector3.zero)
            {
                anim.SetBool(MovingUp, false);
                anim.SetBool(MovingDown, false);
                anim.SetBool(MovingLeft, false);
                anim.SetBool(MovingRight, false);
                if(BossLevelController.Instance.bossStarted)
                    anim.SetBool(PlayerMagic, true);
            }
            else
            {
                anim.SetBool(MovingUp, isMovingUp);
                anim.SetBool(MovingDown, isMovingDown);
                anim.SetBool(MovingLeft, isMovingLeft);
                anim.SetBool(MovingRight, isMovingRight);
                anim.SetBool(PlayerMagic, false);
            }
        
            lastPos = currentPos;

            // Wait for a short period before updating the animation again
            yield return new WaitForSeconds(0.1f); // Adjust the delay time to a suitable amount

            // Restart the coroutine to update the animation again
            StartCoroutine(UpdateAnimation());
        }
        
    }

    void Start()
    {
        StartCoroutine(UpdateAnimation());
    }
}