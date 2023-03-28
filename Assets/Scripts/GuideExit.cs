using UnityEngine;

public class GuideExit : MonoBehaviour
{
    public GameObject levelExit;

    void Update()
    {
        if (DialogueUI.Instance.talkedToFinalGuide != true) return;
        levelExit.SetActive(true);
        Destroy(gameObject);
    }
}
