using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyController : MonoBehaviour
{
    public Rigidbody2D theRB;
    public float moveSpeed;
    public bool isFrozen;
    private float _frozenTimeRemaining;
    
    [Header("Chase Player")]
    public bool shouldChasePlayer;
    public float rangeToChasePlayer;
    private Vector3 _moveDirection;

    [Header("Run Away")]
    public bool shouldRunAway;
    public float runawayRange;

    [Header("Wandering")]
    public bool shouldWander;
    public float wanderLength, pauseLength;
    private float _wanderCounter, _pauseCounter;
    private Vector3 _wanderDirection;

    [Header("Patrolling")]
    public bool shouldPatrol;
    public Transform[] patrolPoints;
    private int _currentPatrolPoint;

    [Header("Shooting")]
    public bool shouldShoot;
    public GameObject spell;
    public Transform firePoint;
    public float fireRate;
    private float _fireCounter;
    public float shootRange;

    [Header("Variables")]
    public SpriteRenderer theBody;
    public Animator anim;
    public int health = 150;
    public GameObject[] deathSplatters;
    public GameObject hitEffect;

    [Header("Item Drop")]
    public bool shouldDropItem;
    public GameObject[] itemsToDrop;
    public float itemDropPercent;
    private static readonly int IsMoving = Animator.StringToHash("isMoving");

    public float separationDistanceThreshold = 2.0f;
    public float separationMoveSpeed = 1.0f;


    // Start is called before the first frame update
    private void Start()
    {
        if(shouldWander)
        {
            _pauseCounter = Random.Range(pauseLength * 0.75f, pauseLength * 1.25f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        EnemySeparation();
        
        if (theBody.isVisible && PlayerController.Instance.gameObject.activeInHierarchy)
        {
            if(isFrozen)
            {
                _frozenTimeRemaining -= Time.deltaTime;
                if (_frozenTimeRemaining <= 0)
                {
                    isFrozen = false;
                }
                else
                {
                    theRB.velocity = Vector2.zero;
                    // Stop the enemy's animation
                    anim.speed = 0;

                    // Wait for 1 second before unfreezing the enemy
                    StartCoroutine(UnfreezeAfterDelay(1f));
                    return;
                }  
            }

            IEnumerator UnfreezeAfterDelay(float delay)
            {
                yield return new WaitForSeconds(delay);

                // Unfreeze the enemy
                isFrozen = false;

                // Resume the enemy's animation
                anim.speed = 1;
            }
            
            _moveDirection = Vector3.zero;
            
            if (shouldChasePlayer)
            {
                // Calculate the vector from the enemy to the player
                Vector3 playerDirection = PlayerController.Instance.transform.position - transform.position;

                // Check if the player is within range to chase
                if (playerDirection.magnitude < rangeToChasePlayer)
                {
                    // Move directly towards the player's position
                    _moveDirection = playerDirection.normalized;
                }
            }
            else
            {
                if (shouldWander)
                {
                    if (_wanderCounter > 0)
                    {
                        _wanderCounter -= Time.deltaTime;

                        //move the enemy
                        _moveDirection = _wanderDirection;

                        if (_wanderCounter <= 0)
                        {
                            _pauseCounter = Random.Range(pauseLength * .75f, pauseLength * 1.25f);
                        }
                    }

                    if (_pauseCounter > 0)
                    {
                        _pauseCounter -= Time.deltaTime;

                        if (_pauseCounter <= 0)
                        {
                            _wanderCounter = Random.Range(wanderLength * .75f, wanderLength * 1.25f);

                            _wanderDirection = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0f);
                        }
                    }
                }

                if (shouldPatrol)
                {
                    _moveDirection = patrolPoints[_currentPatrolPoint].position - transform.position;

                    if (Vector3.Distance(transform.position, patrolPoints[_currentPatrolPoint].position) < .2f)
                    {
                        _currentPatrolPoint++;
                        if (_currentPatrolPoint >= patrolPoints.Length)
                        {
                            _currentPatrolPoint = 0;
                        }
                    }
                }
            }

            if (shouldRunAway && Vector3.Distance(transform.position, PlayerController.Instance.transform.position) < runawayRange)
            {
                _moveDirection = transform.position - PlayerController.Instance.transform.position;
            }


            /*else
            {
                moveDirection = Vector3.zero;
            } */


            _moveDirection.Normalize();

            theRB.velocity = _moveDirection * moveSpeed;


            if (shouldShoot && Vector2.Distance(transform.position, PlayerController.Instance.transform.position) < shootRange)
            {
                _fireCounter -= Time.deltaTime;

                if (_fireCounter <= 0)
                {
                    _fireCounter = fireRate;

                    // Calculate the predicted position of the player
                    Vector2 predictedPlayerPosition = PlayerController.Instance.transform.position +
                                                      (PlayerController.Instance.transform.position - transform.position).magnitude *
                                                      (Vector3)PlayerController.Instance.theRb.velocity.normalized;

                    // Aim at the predicted position and shoot
                    Vector2 shootDirection = predictedPlayerPosition - (Vector2)firePoint.position;
                    float angle = Mathf.Atan2(shootDirection.y, shootDirection.x) * Mathf.Rad2Deg;
                    Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                    Instantiate(spell, firePoint.position, rotation);
                    Debug.DrawLine(firePoint.position, predictedPlayerPosition, Color.green, 0.5f);
                    AudioManager.Instance.PlaySfx(10);
                }
            }


        }
        else
        {
            theRB.velocity = Vector2.zero;
        }

        anim.SetBool(IsMoving, _moveDirection != Vector3.zero);
    }

    private static Vector2 op_Addition()
    {
        throw new NotImplementedException();
    }

    public void DamageEnemy(int damage)
    {
        health -= damage;

        var enemyControllerTransform = transform;
        Instantiate(hitEffect, enemyControllerTransform.position, enemyControllerTransform.rotation);

        if (health > 0) return;
        Destroy(gameObject);

        AudioManager.Instance.PlaySfx(1);

        int selectedSplatter = Random.Range(0, deathSplatters.Length);

        int rotation = Random.Range(0, 4) * 90;

        Instantiate(deathSplatters[selectedSplatter], transform.position, Quaternion.Euler(0f, 0f, rotation));

        //drop items

        if (!shouldDropItem) return;
        var dropChance = Random.Range(0f, 100f);

        if (!(dropChance <= itemDropPercent)) return;
        var randomItem = Random.Range(0, itemsToDrop.Length);
        
        Instantiate(itemsToDrop[randomItem], enemyControllerTransform.position, enemyControllerTransform.rotation);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("IceSpell"))
        {
            if (Random.value > 0.75)
            {
                isFrozen = true;
                _frozenTimeRemaining = 1f;
            }
        }
        
        if (other.CompareTag("WindSpell"))
        {
            Vector2 knockbackDirection = (transform.position - other.transform.position).normalized;
            theRB.AddForce(knockbackDirection * 2000f);
        }
    }

    private void EnemySeparation()
    {
        // Get all enemies in the scene
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        // Loop through each enemy and move them away from each other if they are too close
        for (int i = 0; i < enemies.Length; i++) {
            for (int j = i + 1; j < enemies.Length; j++) {
                Vector3 offset = enemies[i].transform.position - enemies[j].transform.position;
                float distance = offset.magnitude;

                if (distance < separationDistanceThreshold) {
                    Vector3 direction = offset.normalized;
                    enemies[i].transform.position += direction * separationMoveSpeed * Time.deltaTime;
                    enemies[j].transform.position -= direction * separationMoveSpeed * Time.deltaTime;
                }
            }
        }
    }
}
