using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Gun : MonoBehaviour
{
    public GameObject bulletToFire;
    [SerializeField]
    private Transform firePointUp, firePointDown, firePointLeft, firePointRight;

    public float timeBetweenShots;
    private float shotCounter;

    [SerializeField] private DialogueUI dialogueUI;
    public DialogueUI DialogueUI => dialogueUI;

    void Update()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;

        if (PlayerController.instance.canMove && !PlayerController.instance.animationOverride && !LevelManager.instance.isPaused && !dialogueUI.isOpen && sceneName != "Luci Room")
        {
            if (shotCounter > 0)
            {
                shotCounter -= Time.deltaTime;
            }
            else
            {
                if (Input.GetKey(KeyCode.UpArrow))
                {
                   if (PlayerController.instance.anim.GetCurrentAnimatorStateInfo(0).IsName("Player_Idle_Right") || PlayerController.instance.anim.GetCurrentAnimatorStateInfo(0).IsName("Player_Idle_Left") || PlayerController.instance.anim.GetCurrentAnimatorStateInfo(0).IsName("Player_Idle") || PlayerController.instance.anim.GetCurrentAnimatorStateInfo(0).IsName("isMovingUpBackwards"))
                    {
                        PlayerController.instance.anim.SetBool("isMovingUp", true);
                    }
                    Instantiate(bulletToFire, firePointUp.position, Quaternion.Euler(new Vector3(0,0,90)));
                    shotCounter = timeBetweenShots;
                    AudioManager.instance.PlaySFX(9);
                }
                else if (Input.GetKey(KeyCode.DownArrow))
                {
                    if (PlayerController.instance.anim.GetCurrentAnimatorStateInfo(0).IsName("Player_Idle_Right") || PlayerController.instance.anim.GetCurrentAnimatorStateInfo(0).IsName("Player_Idle_Left") || PlayerController.instance.anim.GetCurrentAnimatorStateInfo(0).IsName("Player_Idle_Back") || PlayerController.instance.anim.GetCurrentAnimatorStateInfo(0).IsName("isMovingDownBackwards"))
                    {
                        PlayerController.instance.anim.SetBool("isMovingDown", true);
                    }
                    Instantiate(bulletToFire, firePointDown.position, Quaternion.Euler(new Vector3(0, 0, -90)));
                    shotCounter = timeBetweenShots;
                    AudioManager.instance.PlaySFX(9);
                }
                else if (Input.GetKey(KeyCode.LeftArrow))
                {
                    if (PlayerController.instance.anim.GetCurrentAnimatorStateInfo(0).IsName("Player_Idle_Right") || PlayerController.instance.anim.GetCurrentAnimatorStateInfo(0).IsName("Player_Idle") || PlayerController.instance.anim.GetCurrentAnimatorStateInfo(0).IsName("Player_Idle_Back") || PlayerController.instance.anim.GetCurrentAnimatorStateInfo(0).IsName("isMovingLeftBackwards"))
                    {
                        PlayerController.instance.anim.SetBool("isMovingLeft", true);
                    }

                    Instantiate(bulletToFire, firePointLeft.position, Quaternion.Euler(new Vector3(0, 0, 180)));
                    shotCounter = timeBetweenShots;
                    AudioManager.instance.PlaySFX(9);
                }
                else if (Input.GetKey(KeyCode.RightArrow) )
                {
                    if (PlayerController.instance.anim.GetCurrentAnimatorStateInfo(0).IsName("Player_Idle_Left") || PlayerController.instance.anim.GetCurrentAnimatorStateInfo(0).IsName("Player_Idle") || PlayerController.instance.anim.GetCurrentAnimatorStateInfo(0).IsName("Player_Idle_Back") || PlayerController.instance.anim.GetCurrentAnimatorStateInfo(0).IsName("isMovingRightBackwards"))
                    {
                        PlayerController.instance.anim.SetBool("isMovingRight", true);
                    }
                    Instantiate(bulletToFire, firePointRight.position, Quaternion.Euler(new Vector3(0, 0, 0)));
                    shotCounter = timeBetweenShots;
                    AudioManager.instance.PlaySFX(9);
                }
            }
        }
    }
}
