using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    public int healAmount = 1;

    public float waitToBeCollected = 0.5f;
    
    // Update is called once per frame
    void Update()
    {
        if(waitToBeCollected > 0)
        {
            waitToBeCollected -= Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player") && waitToBeCollected <= 0)
        {
            PlayerHealthController.Instance.HealPlayer(healAmount);

            Destroy(gameObject);
            AudioManager.Instance.PlaySfx(5);
        }
    }
}
