using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomCenter : MonoBehaviour
{
    public static RoomCenter instance;

    public bool openWhenEnemiesCleared, noEnemies, guideDialogue;

    public List<GameObject> enemies = new List<GameObject>();

    public Room theRoom;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
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
                StartCoroutine(CameraShake.instance.ShakeCamera());

                StartCoroutine(RoomComplete());

                
            }
        }

        if(DialogueUI.instance.talkedToGuide == true)
        {
            guideDialogue = false;
        }

        if (enemies.Count == 0 && noEnemies && !guideDialogue)
        {
            theRoom.OpenDoors();
        }

    }

    public IEnumerator RoomComplete()
    {
        yield return new WaitForSeconds(1);
        AudioManager.instance.PlaySFX(13);
        theRoom.OpenDoors();
    }
}



