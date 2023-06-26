using UnityEngine;
using System.Collections;

public class BossController : MonoBehaviour
{
    public static BossController Instance;

    public BossAction[] actions;
    private int _currentAction;
    private float _actionCounter;

    private float _shotCounter;
    private Vector2 _moveDirection;
    public Rigidbody2D theRb;

    public int currentHealth;

    public GameObject deathEffect, levelExit, hitEffect, levelExit2;

    public BossSequence[] sequences;
    public int currentSequence;

    [SerializeField] private DialogueUI dialogueUI;
    [SerializeField] private DialogueObject bossDead;
    public DialogueUI DialogueUI => dialogueUI;

    public GameObject endDemoScreen;
    
    [SerializeField]
    private Animator bossAnimator;
    
    private static readonly int PlayerIdle = Animator.StringToHash("Player_Idle");
    [SerializeField] private GameObject dialogueBox;

    public bool bossIsDead;

    private void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        actions = sequences[currentSequence].actions;

        _actionCounter = actions[_currentAction].actionLength;
    }

    // Update is called once per frame
    void Update()
    {
        if (!dialogueUI.IsOpen && !dialogueBox.activeSelf && LevelManager.Instance.isPaused == false && BossLevelController.Instance.bossStarted)
        {
            if (_actionCounter > 0)
            {
                _actionCounter -= Time.deltaTime;

                //handle movement
                _moveDirection = Vector2.zero;

                if (actions[_currentAction].shouldMove)
                {
                    if (actions[_currentAction].shouldChasePlayer)
                    {
                        _moveDirection = PlayerController.Instance.transform.position - transform.position;
                        _moveDirection.Normalize();
                    }

                    if (actions[_currentAction].moveToPoints && Vector3.Distance(transform.position, actions[_currentAction].pointToMoveTo.position) > 0.5f)
                    {
                        _moveDirection = actions[_currentAction].pointToMoveTo.position - transform.position;
                        _moveDirection.Normalize();
                    }
                }

                theRb.velocity = _moveDirection * actions[_currentAction].moveSpeed;

                //handle shooting

                if (actions[_currentAction].shouldShoot)
                {
                    _shotCounter -= Time.deltaTime;
                    if (_shotCounter <= 0)
                    {
                        _shotCounter = actions[_currentAction].timeBetweenShots;

                        foreach (Transform t in actions[_currentAction].shotPoints)
                        {
                            Instantiate(actions[_currentAction].itemToShoot, t.position, t.rotation);
                            AudioManager.Instance.PlaySfx(9);
                        }
                    }
                }

            }
            else
            {
                _currentAction++;
                if (_currentAction >= actions.Length)
                {
                    _currentAction = 0;
                }

                _actionCounter = actions[_currentAction].actionLength;
            }
        }
    }

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;

        if (currentHealth <= 0 && !bossIsDead)
        {
            StartCoroutine(EndGame());
        }
        else
        {
            if(currentHealth <= sequences[currentSequence].endSequenceHealth && currentSequence < sequences.Length - 1)
            {
                currentSequence++;
                actions = sequences[currentSequence].actions;
                _currentAction = 0;
                _actionCounter = actions[_currentAction].actionLength;
            }
        }

        UIController.Instance.bossHealthBar.value = currentHealth;
    }
    private IEnumerator EndGame()
    {
        bossIsDead = true;
        UIController.Instance.bossHealthBar.gameObject.SetActive(false);
        bossAnimator.Play(PlayerIdle);
        bossAnimator.enabled = false;
        theRb.velocity = Vector2.zero;
        DialogueUI.Instance.ShowDialogue(bossDead);
            
        while (dialogueBox.activeSelf)
        {
            yield return null;
        }
        endDemoScreen.SetActive(true);
        gameObject.SetActive(false);
    }
}



[System.Serializable]
public class BossAction
{
    [Header("Action")]
    public float actionLength;

    public bool shouldMove;
    public bool shouldChasePlayer;
    public float moveSpeed;
    public bool moveToPoints;
    public Transform pointToMoveTo;

    public bool shouldShoot;
    public GameObject itemToShoot;
    public float timeBetweenShots;
    public Transform[] shotPoints;

}

[System.Serializable]
public class BossSequence
{
    [Header("Sequence")]
    public BossAction[] actions;

    public int endSequenceHealth;
}