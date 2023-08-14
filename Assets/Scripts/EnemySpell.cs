using UnityEngine;

public class EnemySpell : MonoBehaviour
{
    public static EnemySpell Instance;
    public float speed;
    private Vector3 _direction;
    private Rigidbody2D _rigidbody;
    private bool _isPaused;

    private float _storedSpeed;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _storedSpeed = speed; 
        
        Vector2 direction = (PlayerController.Instance.transform.position - transform.position).normalized;
        
        _rigidbody.velocity = direction * speed;
        _direction = direction;
    }

    private void Update()
    {
        if (LevelManager.Instance != null && LevelManager.Instance.isPaused)
        {
            if (!_isPaused)
            {
                _rigidbody.velocity = Vector2.zero;
                _isPaused = true;
            }
        }
        else
        {
            if (_isPaused)
            {
                _rigidbody.velocity = _direction * speed;
                _isPaused = false;
            }

            transform.position += _direction * (speed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
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

    public void SetPaused(bool isPaused)
    {
        _isPaused = isPaused;

        if (_isPaused)
        {
            _rigidbody.velocity = Vector2.zero;
        }
        else
        {
            _rigidbody.velocity = _direction * speed;
        }
    }
}