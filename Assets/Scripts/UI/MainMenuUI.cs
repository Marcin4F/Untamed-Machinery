//using UnityEditor.Rendering.LookDev;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] Button play, quit;

    private void Start()
    {
        play.onClick.AddListener(StartGame);
        quit.onClick.AddListener(CloseGame);
    }

    void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    void CloseGame()
    {
        Debug.Log("Wyjscie");
        Application.Quit();
    }
}
