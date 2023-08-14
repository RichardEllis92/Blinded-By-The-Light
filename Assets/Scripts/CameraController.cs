using UnityEngine;
using UnityEngine.SceneManagement;
using Rewired;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance;

    public float moveSpeed;

    public Transform target;

    public Camera mainCamera, bigMapCamera;

    public bool bigMapActive;

    public bool isBossRoom;
    private bool _isTargetNotNull;
    
    public int playerId = 0;
    private Player player;
    [System.NonSerialized] // Don't serialize this so the value is lost on an editor script recompile.
    private bool initialized;

    private void Awake()
    {
        Instance = this;
    }
    
    private void Initialize() {
        // Get the Rewired Player object for this player.
        player = ReInput.players.GetPlayer(playerId);
            
        initialized = true;
    }
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        Scene currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;
        _isTargetNotNull = target != null;
        if (isBossRoom)
        {
            if (sceneName == "Boss")
            {
                target = BossController.Instance.transform;
            }
            else if (sceneName == "BossFail")
            {
                target = PlayerController.Instance.transform;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(!ReInput.isReady) return; // Exit if Rewired isn't ready. This would only happen during a script recompile in the editor.
        if(!initialized) Initialize(); // Reinitialize after a recompile in the editor
        
        if(target != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(target.position.x, target.position.y, transform.position.z), moveSpeed * Time.deltaTime);
        }

        if (isBossRoom) return;
        if (player.GetButtonDown("Toggle Map") && !bigMapActive)
        {
            ActivateBigMap();
        }
        else if (player.GetButtonDown("Toggle Map") && bigMapActive)
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
        Scene currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;
        
        if (!LevelManager.Instance.isPaused && !DialogueUI.Instance.IsOpen && !CheatSystemController.Instance.showConsole && sceneName != "Luci Room" && sceneName != "Luci Room Complete" && sceneName != "Luci Room Doll")
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
            UIController.Instance.levelTextObject.SetActive(false);
            UIController.Instance.bigMap.SetActive(true);
        }
        
    }

    private void DeactivateBigMap()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;

        if (!LevelManager.Instance.isPaused && !DialogueUI.Instance.IsOpen &&
            !CheatSystemController.Instance.showConsole && sceneName != "Luci Room" &&
            sceneName != "Luci Room Complete" && sceneName != "Luci Room Doll")
        {
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
            UIController.Instance.levelTextObject.SetActive(true);
            UIController.Instance.bigMap.SetActive(false);
        }
    }

    public void ChangeTargetToPlayer()
    {
        target = PlayerController.Instance.transform;
    }
}
