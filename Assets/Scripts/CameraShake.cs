using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake instance;

    Transform camTrans;

    public float shakeTime;
    public float shakeRange;
    Vector3 originalPosition, currentPosition;

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        camTrans = Camera.main.transform;
        originalPosition = camTrans.position;
    }

    void Update()
    {
        //camTrans = Camera.main.transform;
        currentPosition = camTrans.position;
    }

    public IEnumerator ShakeCamera()
    {
        float elapsedTime = 0;

        while (elapsedTime < shakeTime)
        {
            Vector3 pos = currentPosition + Random.insideUnitSphere * shakeRange;

            pos.z = originalPosition.z;

            camTrans.position = pos;

            elapsedTime += Time.deltaTime;

            yield return null;
        }
        camTrans.position = currentPosition;
    }

}
