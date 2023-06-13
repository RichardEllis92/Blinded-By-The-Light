using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance;

    public float moveSpeed;

    public Transform target;

    public Camera mainCamera, bigMapCamera;

    public bool bigMapActive;

    public bool isBossRoom;
    private bool _isTargetNotNull;

    private void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        _isTargetNotNull = target != null;
        if (isBossRoom)
        {
            target = PlayerController.Instance.transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(target != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(target.position.x, target.position.y, transform.position.z), moveSpeed * Time.deltaTime);
        }

        if (!Input.GetKeyDown(KeyCode.M) || isBossRoom) return;
        if (!bigMapActive)
        {
            ActivateBigMap();
        }
        else
        {
            DeactivateBigMap();
        }
    }

    public void ChangeTarget(Transform newTarget)
    {
        target = newTarget;
    }

    private void ActivateBigMap()
    {
        if (!LevelManager.Instance.isPaused && !DialogueUI.Instance.IsOpen && !CheatSystemController.Instance.showConsole)
        {
            bigMapActive = true;

            bigMapCamera.enabled = true;
            mainCamera.enabled = false;

            PlayerController.Instance.canMove = false;

            Time.timeScale = 0f;

            UIController.Instance.mapDisplay.SetActive(false);
            UIController.Instance.activeSpells.SetActive(false);
            UIController.Instance.health.SetActive(false);
            UIController.Instance.hellBucks.SetActive(false);
            UIController.Instance.experience.SetActive(false);
            UIController.Instance.bigMapText.SetActive(true);
        }
        
    }

    private void DeactivateBigMap()
    {
        if (LevelManager.Instance.isPaused || DialogueUI.Instance.IsOpen) return;
        bigMapActive = false;

        bigMapCamera.enabled = false;
        mainCamera.enabled = true;

        PlayerController.Instance.canMove = true;

        Time.timeScale = 1f;

        UIController.Instance.mapDisplay.SetActive(true);
        UIController.Instance.activeSpells.SetActive(true);
        UIController.Instance.health.SetActive(true);
        UIController.Instance.hellBucks.SetActive(true);
        UIController.Instance.experience.SetActive(true);
        UIController.Instance.bigMapText.SetActive(false);

    }
}
