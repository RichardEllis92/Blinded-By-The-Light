using UnityEngine;

public class Breakables : MonoBehaviour
{
    //public GameObject[] brokenPieces;
    //public int maxPieces = 5;

    public bool shouldDropItem;
    public GameObject[] itemsToDrop;
    public float itemDropPercent;
    private void Smash()
    {
        Destroy(gameObject);

        AudioManager.Instance.PlaySfx(0);

        //show broken pieces
        /*
        int piecesToDrop = Random.Range(1, maxPieces);

        for (int i = 0; i < piecesToDrop; i++)
        {
            int randomPiece = Random.Range(0, brokenPieces.Length);

            var breakablesTransform = transform;
            Instantiate(brokenPieces[randomPiece], breakablesTransform.position, breakablesTransform.rotation);
        }
        */
        //drop items

        if (shouldDropItem)
        {
            float dropChance = Random.Range(0f, 100f);

            if (dropChance <= itemDropPercent)
            {
                int randomItem = Random.Range(0, itemsToDrop.Length);

                var breakablesTransform = transform;
                Instantiate(itemsToDrop[randomItem], breakablesTransform.position, breakablesTransform.rotation);
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            if(PlayerController.Instance.dashCounter > 0)
            {
                Smash();
            }
        }

        if(other.CompareTag("PlayerBullet") || other.CompareTag("IceSpell") || other.CompareTag("WindSpell"))
        {
            Smash();
        }
    }
}
