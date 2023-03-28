using UnityEngine;

public class Ending : MonoBehaviour
{
    public GameObject endingScreen;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            endingScreen.SetActive(true);
        }  
    }
}
