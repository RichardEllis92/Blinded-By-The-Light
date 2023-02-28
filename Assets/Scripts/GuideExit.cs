using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuideExit : MonoBehaviour
{
    public GameObject levelExit;

    void Update()
    {
        if (DialogueUI.instance.talkedToFinalGuide == true)
        {
            levelExit.SetActive(true);
            Destroy(gameObject);
        }
    }
}
