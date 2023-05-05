using System;
using System.Collections;
using UnityEngine;

public class EyeDemonAnimations : MonoBehaviour
{
    public static EyeDemonAnimations Instance;
    public Animator anim;
    
    private Vector3 lastPos;
    
    private static readonly int MovingUp = Animator.StringToHash("MovingUp");
    public bool movingUp;

    private void Awake()
    {
        Instance = this;
    }

    IEnumerator UpdateAnimation()
    {
        Vector3 currentPos = transform.position;
        bool isMovingUp = currentPos.y > lastPos.y;
        anim.SetBool(MovingUp, isMovingUp);
        movingUp = isMovingUp;
        Debug.Log(isMovingUp ? "going up" : "going down");
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
