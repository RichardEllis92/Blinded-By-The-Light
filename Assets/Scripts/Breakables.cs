using UnityEngine;

public class Breakables : MonoBehaviour
{
    public bool shouldDropItem;
    public GameObject[] itemsToDrop;
    public float itemDropPercent;
    private bool _itemDropped;

    private void Smash()
    {
        Destroy(gameObject);

        AudioManager.Instance.PlaySfx(0);

        if (shouldDropItem && !_itemDropped)
        {
            _itemDropped = true;
            float dropChance = Random.Range(0f, 100f);

            if (dropChance <= itemDropPercent)
            {
                int randomIndex = Random.Range(0, itemsToDrop.Length);
                var breakablesTransform = transform;
                Instantiate(itemsToDrop[randomIndex], breakablesTransform.position, breakablesTransform.rotation);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (PlayerController.Instance.dashCounter > 0)
            {
                Smash();
            }
        }

        if (other.CompareTag("PlayerSpell") || other.CompareTag("IceSpell") || other.CompareTag("WindSpell"))
        {
            Smash();
        }
    }
}


