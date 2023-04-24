using UnityEngine;

public class SecretRoom : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && PlayerController.Instance.isRolling)
        {
            Destroy(gameObject);
        }
    }
}
