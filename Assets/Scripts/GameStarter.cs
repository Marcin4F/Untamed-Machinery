using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStarter : MonoBehaviour
{
    [SerializeField] HubUI hubUI;

    private bool inRange = false;

    private void Update()
    {
        if (inRange && Input.GetKeyDown(KeyCode.Space))
        {
            Player.instance.SaveInfo();
            PlayerPrefs.SetInt("GameState", 1);
            SceneManager.LoadScene(Random.Range(2, 7));
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        hubUI.StartGameDisplay();
        inRange = true;
    }

    private void OnTriggerExit(Collider other)
    {
        hubUI.StartGameHide();
        inRange = false;
    }
}
