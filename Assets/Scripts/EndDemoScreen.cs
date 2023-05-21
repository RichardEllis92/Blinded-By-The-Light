using UnityEngine;

public class EndDemoScreen : MonoBehaviour
{
    public GameObject endDemoScreen;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            endDemoScreen.SetActive(true);
            LevelManager.Instance.isPaused = true;
        }
    }
}
