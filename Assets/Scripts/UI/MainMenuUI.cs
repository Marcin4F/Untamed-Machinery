//using UnityEditor.Rendering.LookDev;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [Header("Ekrany Menu (Panels)")]
    [SerializeField] GameObject noSavePanel;    // NIE ma zapisu gry
    [SerializeField] GameObject hasSavePanel;   // jest zapis gry

    [Header("Przyciski - Ekran BEZ Zapisu")]
    [SerializeField] Button startGameButton;
    [SerializeField] Button quitNoButton;

    [Header("Przyciski - Ekran Z Zapisem")]
    [SerializeField] Button newGameButton;
    [SerializeField] Button continueButton;
    [SerializeField] Button quitHasButton;

    private void Start()
    {
        startGameButton.onClick.AddListener(StartGame);
        newGameButton.onClick.AddListener(StartGame);
        continueButton.onClick.AddListener(ContinueGame);
        quitNoButton.onClick.AddListener(CloseGame);
        quitHasButton.onClick.AddListener(CloseGame);
        
        bool hasSave = SaveSystem.HasSave();
        if (hasSave)
        {
            hasSavePanel.SetActive(true);
            noSavePanel.SetActive(false);
        }
        else
        {
            hasSavePanel.SetActive(false);
            noSavePanel.SetActive(true);
        }
    }

    void StartGame()
    {
        SaveSystem.loadFromSave = false;
        SceneManager.LoadScene(1);
    }

    void ContinueGame()
    {
        SaveSystem.loadFromSave = true;
        SceneManager.LoadScene(1);
    }

    void CloseGame()
    {
        Application.Quit();
    }
}
