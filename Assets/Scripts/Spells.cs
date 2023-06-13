using System;
using UnityEngine;
using UnityEngine.Serialization;

public class Spells : MonoBehaviour
{
    public float speed = 7.5f;
    public Rigidbody2D theRb;
    public GameObject impactEffect;
    public int damageToGive;
    public bool isFireSpell = false;
    public bool isWindSpell = false;
    public bool isIceSpell = false;
    public float timeBetweenSpells = 1f;
    
    public static Spells Instance;
    void Awake()
    {
        Instance = this;
        theRb = gameObject.GetComponent<Rigidbody2D>();
    }
    
    void Start()
    {
        if (isFireSpell)
        {
            FireSpell fireSpell = gameObject.AddComponent<FireSpell>();
            speed = fireSpell.fireSpeed;
            damageToGive = fireSpell.fireDamage;
            timeBetweenSpells = fireSpell.fireTimeBetweenSpells;
        }
        else if (isWindSpell)
        {
            WindSpell windSpell = gameObject.AddComponent<WindSpell>();
            speed = windSpell.windSpeed;
            damageToGive = windSpell.windDamage;
            timeBetweenSpells = windSpell.windTimeBetweenSpells;
        }
        else if (isIceSpell)
        {
            IceSpell iceSpell = gameObject.AddComponent<IceSpell>();
            speed = iceSpell.iceSpeed;
            damageToGive = iceSpell.iceDamage;
            timeBetweenSpells = iceSpell.iceTimeBetweenSpells;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isIceSpell)
        {
            if (Magic.Instance.spellMovingUp)
            { 
                theRb.velocity = transform.up * (speed * -1f);
            }
            else if (Magic.Instance.spellMovingDown)
            {
                theRb.velocity = transform.up * speed;
            }
            else
            {
                theRb.velocity = transform.right * speed;
            }
        }
        theRb.velocity = transform.right * speed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("NoSpell"))
        {
            var playerSpellTransform = transform;
            Instantiate(Magic.Instance.impactEffect, playerSpellTransform.position, playerSpellTransform.rotation);
            Destroy(gameObject);
            AudioManager.Instance.PlaySfx(3);
        }

        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<EnemyController>().DamageEnemy(damageToGive);
        }

        if (other.CompareTag("Boss"))
        {
            BossController.Instance.TakeDamage(damageToGive);

            var playerSpellTransform = transform;
            Instantiate(BossController.Instance.hitEffect, playerSpellTransform.position, playerSpellTransform.rotation);
        }
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }


}

public class FireSpell : Spells
{
    public float fireSpeed = 12f;
    public int fireDamage = 50;
    public float fireTimeBetweenSpells = 1f;
}

public class WindSpell : Spells
{
    public float windSpeed = 15f;
    public int windDamage = 5;
    public float windTimeBetweenSpells = 0.2f;
}

public class IceSpell : Spells
{
    public float iceSpeed = 5f;
    public int iceDamage = 20;
    public float iceTimeBetweenSpells = 2f;
}
