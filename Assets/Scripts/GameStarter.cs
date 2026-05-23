using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStarter : MonoBehaviour
{
    private bool inRange = false;

    private void Update()
    {
        if (inRange && Input.GetKeyDown(KeyCode.Space))
        {
            Player.instance.SaveInfo();
            GameManagement.instance.roomsCleared = 0;
            GameManagement.instance.gameState = 1;
            SceneManager.LoadScene(Random.Range(2, 8));
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        InGameUI.instance.StartGameDisplay();
        inRange = true;
    }

    private void OnTriggerExit(Collider other)
    {
        InGameUI.instance.StartGameHide();
        inRange = false;
    }
}
