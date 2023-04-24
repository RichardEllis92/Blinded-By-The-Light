using UnityEngine;

public class HellBuckPickup : MonoBehaviour
{
    public int hellBuckValue = 1;

    public float waitToBeCollected;

    // Update is called once per frame
    void Update()
    {
        if (waitToBeCollected > 0)
        {
            waitToBeCollected -= Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && waitToBeCollected <= 0)
        {
            LevelManager.Instance.GetHellBucks(hellBuckValue);

            Destroy(gameObject);
            AudioManager.Instance.PlaySfx(4);
        }
    }
}
