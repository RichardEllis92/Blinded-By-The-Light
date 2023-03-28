using UnityEngine;
using UnityEngine.Serialization;

public class PlayerCanvas : MonoBehaviour
{
    [FormerlySerializedAs("Follow")] public Transform follow;

    private Camera _mainCamera;

    // Start is called before the first frame update
    void Start()
    {
        _mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        var screenPos = _mainCamera.WorldToScreenPoint(follow.position);

        transform.position = screenPos;
    }
}
