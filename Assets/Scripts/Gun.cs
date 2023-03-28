using UnityEngine;
using UnityEngine.SceneManagement;

public class Gun : MonoBehaviour
{
    public GameObject bulletToFire;
    [SerializeField]
    private Transform firePointUp, firePointDown, firePointLeft, firePointRight;

    public float timeBetweenShots;
    private float _shotCounter;

    [SerializeField] private DialogueUI dialogueUI;
    private static readonly int IsMovingUp = Animator.StringToHash("isMovingUp");
    private static readonly int IsMovingDown = Animator.StringToHash("isMovingDown");
    private static readonly int IsMovingLeft = Animator.StringToHash("isMovingLeft");
    private static readonly int IsMovingRight = Animator.StringToHash("isMovingRight");
    public DialogueUI DialogueUI => dialogueUI;

    void Update()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;

        if (PlayerController.Instance.canMove && !PlayerController.Instance.animationOverride && !LevelManager.Instance.isPaused && !dialogueUI.IsOpen && sceneName != "Luci Room")
        {
            if (_shotCounter > 0)
            {
                _shotCounter -= Time.deltaTime;
            }
            else
            {
                if (Input.GetKey(KeyCode.UpArrow))
                {
                   if (PlayerController.Instance.anim.GetCurrentAnimatorStateInfo(0).IsName("Player_Idle_Right") || PlayerController.Instance.anim.GetCurrentAnimatorStateInfo(0).IsName("Player_Idle_Left") || PlayerController.Instance.anim.GetCurrentAnimatorStateInfo(0).IsName("Player_Idle") || PlayerController.Instance.anim.GetCurrentAnimatorStateInfo(0).IsName("isMovingUpBackwards"))
                   {
                        PlayerController.Instance.anim.SetBool(IsMovingUp, true);
                   }
                   Instantiate(bulletToFire, firePointUp.position, Quaternion.Euler(new Vector3(0,0,90)));
                   _shotCounter = timeBetweenShots;
                   AudioManager.Instance.PlaySfx(9);
                }
                else if (Input.GetKey(KeyCode.DownArrow))
                {
                    if (PlayerController.Instance.anim.GetCurrentAnimatorStateInfo(0).IsName("Player_Idle_Right") || PlayerController.Instance.anim.GetCurrentAnimatorStateInfo(0).IsName("Player_Idle_Left") || PlayerController.Instance.anim.GetCurrentAnimatorStateInfo(0).IsName("Player_Idle_Back") || PlayerController.Instance.anim.GetCurrentAnimatorStateInfo(0).IsName("isMovingDownBackwards"))
                    {
                        PlayerController.Instance.anim.SetBool(IsMovingDown, true);
                    }
                    Instantiate(bulletToFire, firePointDown.position, Quaternion.Euler(new Vector3(0, 0, -90)));
                    _shotCounter = timeBetweenShots;
                    AudioManager.Instance.PlaySfx(9);
                }
                else if (Input.GetKey(KeyCode.LeftArrow))
                {
                    if (PlayerController.Instance.anim.GetCurrentAnimatorStateInfo(0).IsName("Player_Idle_Right") || PlayerController.Instance.anim.GetCurrentAnimatorStateInfo(0).IsName("Player_Idle") || PlayerController.Instance.anim.GetCurrentAnimatorStateInfo(0).IsName("Player_Idle_Back") || PlayerController.Instance.anim.GetCurrentAnimatorStateInfo(0).IsName("isMovingLeftBackwards"))
                    {
                        PlayerController.Instance.anim.SetBool(IsMovingLeft, true);
                    }

                    Instantiate(bulletToFire, firePointLeft.position, Quaternion.Euler(new Vector3(0, 0, 180)));
                    _shotCounter = timeBetweenShots;
                    AudioManager.Instance.PlaySfx(9);
                }
                else if (Input.GetKey(KeyCode.RightArrow) )
                {
                    if (PlayerController.Instance.anim.GetCurrentAnimatorStateInfo(0).IsName("Player_Idle_Left") || PlayerController.Instance.anim.GetCurrentAnimatorStateInfo(0).IsName("Player_Idle") || PlayerController.Instance.anim.GetCurrentAnimatorStateInfo(0).IsName("Player_Idle_Back") || PlayerController.Instance.anim.GetCurrentAnimatorStateInfo(0).IsName("isMovingRightBackwards"))
                    {
                        PlayerController.Instance.anim.SetBool(IsMovingRight, true);
                    }
                    Instantiate(bulletToFire, firePointRight.position, Quaternion.Euler(new Vector3(0, 0, 0)));
                    _shotCounter = timeBetweenShots;
                    AudioManager.Instance.PlaySfx(9);
                }
            }
        }
    }
}
