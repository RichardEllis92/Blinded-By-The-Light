using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpell : MonoBehaviour
{
    public float speed;
    private Vector3 _direction;

    // Start is called before the first frame update
    private float _speed = 5f; // or whatever speed you want the enemy to move at
    private Vector2 _predictedPosition;

    void Start()
    {
        // Calculate the predicted position of the player
        _predictedPosition = PlayerController.Instance.transform.position +
                             (PlayerController.Instance.transform.position - transform.position).magnitude *
                             (Vector3)PlayerController.Instance.theRb.velocity.normalized;

        // Normalize the direction vector
        Vector2 direction = _predictedPosition - (Vector2)transform.position;
        direction.Normalize();

        // Set the initial velocity of the enemy
        GetComponent<Rigidbody2D>().velocity = direction * _speed;
    }
    

    // Update is called once per frame
    void Update()
    {
        transform.position += _direction * (speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            PlayerHealthController.Instance.DamagePlayer();
        }
        AudioManager.Instance.PlaySfx(3);
        
        if (other.CompareTag("EnemySpell"))
        {
            return;
        }
        Destroy(gameObject);
        
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
