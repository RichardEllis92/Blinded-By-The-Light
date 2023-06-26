using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.Serialization;
using UnityEngine.Video;

public class BossLevelController : MonoBehaviour
{
    public static BossLevelController Instance;
    
    public GameObject boss;
    public float walkSpeed = 1.0f;
    public float walkDuration = 10.0f;
    
    [SerializeField]
    private Animator bossAnimator;
    
    [SerializeField]
    private Animator playerAnimator;

    public bool bossStarted;

    private static readonly int PlayerIdle = Animator.StringToHash("Player_Idle");
    private static readonly int DollIdleBack = Animator.StringToHash("Doll_Idle_Back");

    [SerializeField] private DialogueObject beforeBoss;
    [SerializeField] private DialogueObject beforePlayerSummon;
    [SerializeField] private DialogueObject afterPlayerSummon;
    
    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private GameObject player;
    public GameObject bossHealth;
    public GameObject playerHealth;
    public GameObject currentSpell;

    public Transform bossStart;
    public Transform playerStart;

    public GameObject videoCanvas;
    public VideoPlayer videoPlayer;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        StartCoroutine(LevelStart());
    }

    private IEnumerator LevelStart()
    {
        if (!BossIntroComplete.Instance.bossIntroComplete)
        {
            // Cache the boss's initial position
            Vector3 initialPosition = boss.transform.position;

            // Move the boss forward for the specified duration
            float elapsedTime = 0f;
            while (elapsedTime < walkDuration)
            {
                boss.transform.Translate(Vector2.up * walkSpeed * Time.deltaTime);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            // Boss has finished walking
            bossAnimator.SetTrigger(PlayerIdle);

            yield return new WaitForSeconds(1f);
        
            //Start Dialogue
            DialogueUI.Instance.ShowDialogue(beforeBoss);
            while (dialogueBox.activeSelf)
            {
                yield return null;
            }
            AudioManager.Instance.levelMusic.Stop();
            videoPlayer.Prepare();
            videoCanvas.SetActive(true);
            videoPlayer.Play();
            yield return new WaitForSeconds((float)videoPlayer.length);
            videoCanvas.SetActive(false);
            AudioManager.Instance.levelMusic.Play();
            DialogueUI.Instance.ShowDialogue(beforePlayerSummon);
            while (dialogueBox.activeSelf)
            {
                yield return null;
            }
            player.SetActive(true);
            DialogueUI.Instance.ShowDialogue(afterPlayerSummon);
            while (dialogueBox.activeSelf)
            {
                yield return null;
            }
            // Add any additional actions or logic here
            while (dialogueBox.activeSelf)
            {
                yield return null;
            }

            //PlayerController.Instance.transform.position = startPoint.position;
            PlayerController.Instance.canMove = true;
            CameraController.Instance.ChangeTargetToPlayer();
            bossHealth.SetActive(true);
            playerHealth.SetActive(true);
            currentSpell.SetActive(true);
            UIController.Instance.bossHealthBar.maxValue = BossController.Instance.currentHealth;
            UIController.Instance.bossHealthBar.value = BossController.Instance.currentHealth;
            bossStarted = true;
        }
        else
        {
            playerAnimator.SetTrigger(DollIdleBack);
            PlayerController.Instance.canMove = true;
            bossHealth.SetActive(true);
            UIController.Instance.bossHealthBar.maxValue = BossController.Instance.currentHealth;
            UIController.Instance.bossHealthBar.value = BossController.Instance.currentHealth;
            CameraController.Instance.ChangeTargetToPlayer();
            bossStarted = true;
        }
        

    }

}
