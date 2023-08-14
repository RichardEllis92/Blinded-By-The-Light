using UnityEngine;

public class BossControlsManager : MonoBehaviour
{
    public GameObject demoEndScreen;
    public void ReturnToMainMenu()
    {
        demoEndScreen.SetActive(false);
        ControlsManager.Instance.ReturnToMainMenu();
    }
    
    public void ExitGame()
    {
        ControlsManager.Instance.ExitGame();
    }
}
