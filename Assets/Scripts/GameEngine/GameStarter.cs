using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStarter : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        InGameUI.instance.StartGameDisplay();
    }

    private void OnTriggerExit(Collider other)
    {
        InGameUI.instance.StartGameHide();
    }
}
