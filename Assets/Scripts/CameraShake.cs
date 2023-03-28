using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance;

    Transform _camTrans;

    public float shakeTime;
    public float shakeRange;
    private Vector3 _originalPosition, _currentPosition;

    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        if (Camera.main != null) _camTrans = Camera.main.transform;
        _originalPosition = _camTrans.position;
    }

    void Update()
    {
        //camTrans = Camera.main.transform;
        _currentPosition = _camTrans.position;
    }

    public IEnumerator ShakeCamera()
    {
        float elapsedTime = 0;

        while (elapsedTime < shakeTime)
        {
            Vector3 pos = _currentPosition + Random.insideUnitSphere * shakeRange;

            pos.z = _originalPosition.z;

            _camTrans.position = pos;

            elapsedTime += Time.deltaTime;

            yield return null;
        }
        _camTrans.position = _currentPosition;
    }

}
