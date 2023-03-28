using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomCenter : MonoBehaviour
{
    public bool openWhenEnemiesCleared, noEnemies, guideDialogue;

    public List<GameObject> enemies = new List<GameObject>();

    public Room theRoom;

    // Start is called before the first frame update
    void Start()
    {
        if (openWhenEnemiesCleared)
        {
            theRoom.closeWhenEntered = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (enemies.Count > 0 && theRoom.roomActive && openWhenEnemiesCleared)
        {
            for (int i = 0; i < enemies.Count; i++)
            {
                if (enemies[i] == null)
                {
                    enemies.RemoveAt(i);
                    i--;
                }
            }

            if (enemies.Count == 0)
            {
                StartCoroutine(CameraShake.Instance.ShakeCamera());

                StartCoroutine(RoomComplete());

                
            }
        }

        if(DialogueUI.Instance.talkedToGuide)
        {
            guideDialogue = false;
        }

        if (enemies.Count == 0 && noEnemies && !guideDialogue)
        {
            theRoom.OpenDoors();
        }

    }

    private IEnumerator RoomComplete()
    {
        yield return new WaitForSeconds(1);
        AudioManager.Instance.PlaySfx(13);
        theRoom.OpenDoors();
    }
}



