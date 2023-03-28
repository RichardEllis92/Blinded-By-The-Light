using UnityEngine;

public class BossBullet : MonoBehaviour
{
    public float speed;
    private Vector3 _direction;

    // Start is called before the first frame update
    void Start()
    {
        //direction = PlayerController.instance.transform.position - transform.position;
        //direction.Normalize();

        _direction = transform.right;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position =  speed * Time.deltaTime * _direction;

        if (!BossController.Instance.gameObject.activeInHierarchy)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealthController.Instance.DamagePlayer();
        }

        Destroy(gameObject);
        AudioManager.Instance.PlaySfx(3);
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
