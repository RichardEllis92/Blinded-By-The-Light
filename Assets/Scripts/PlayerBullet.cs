using UnityEngine;
using UnityEngine.Serialization;

public class PlayerBullet : MonoBehaviour
{
    public float speed = 7.5f;
    [FormerlySerializedAs("theRB")] public Rigidbody2D theRb;

    public GameObject impactEffect;

    public int damageToGive = 50;

    // Update is called once per frame
    void Update()
    {
        theRb.velocity = transform.right * speed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        
        if(!other.CompareTag("NoBullet"))
        {
            var playerBulletTransform = transform;
            Instantiate(impactEffect, playerBulletTransform.position, playerBulletTransform.rotation);
            Destroy(gameObject);
            AudioManager.Instance.PlaySfx(3);
        }

        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<EnemyController>().DamageEnemy(damageToGive);
        }

        if(other.CompareTag("Boss"))
        {
            BossController.Instance.TakeDamage(damageToGive);

            var playerBulletTransform = transform;
            Instantiate(BossController.Instance.hitEffect, playerBulletTransform.position, playerBulletTransform.rotation);
        }
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
