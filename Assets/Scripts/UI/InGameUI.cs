using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InGameUI : MonoBehaviour
{
    public static InGameUI instance;

    [SerializeField] GameObject deathPanel, pausePanel;         // deathPanel - panel z ele. UI na smierci, pausePanel - panel z ele. UI na menu pauzy
    [SerializeField] Button backToHub, continueButton, quitToHub, quitToMenu;      // przyciski na deathPanel i pausePanel; backToHub jest na smierci
    public TMP_Text currency1, currency2, currency3, ammo;          // tekst z iloscia danej waluty i ilosc amunicji
    [SerializeField] TMP_Text displayHP;                            // tekst z HP wyswietlanym na sliderze

    private bool isPaused = false;

    private void Awake()
    {
        instance = this;
        deathPanel.SetActive(false);
        pausePanel.SetActive(false);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))        // otwieranie / zamykanie menu pauzy
        {
            if (!isPaused)
                PauseMenu();

            else
                ResumeGame();
        }
    }

    // ustawianie wartosci pol tekstowych
    public void SetCurr1()
    {
        currency1.SetText(GameManagement.instance.currency1.ToString());
    }

    public void SetCurr2()
    {
        currency2.SetText(GameManagement.instance.currency2.ToString());
    }

    public void SetCurr3()
    {
        currency3.SetText(GameManagement.instance.currency3.ToString());
    }

    public void SetDisplayHP()
    {
        displayHP.SetText(Player.instance.currentHealth.ToString() + " / " + Player.instance.maxHealth);
    }

    public void SetAmmo()
    {
        ammo.SetText(Player.instance.currentAmmo.ToString() + " / " + Player.instance.maxAmmo);
    }
    
    // funkcje menu pauzy
    private void PauseMenu()    // pauzowanie gry
    {
        isPaused = true;
        Time.timeScale = 0f;
        pausePanel.SetActive(true);
        continueButton.onClick.AddListener(ResumeGame);
        quitToHub.onClick.AddListener(GoBack);
        quitToMenu.onClick.AddListener(Quit);
    }

    private void ResumeGame()       // wznawianie gry
    {
        isPaused = false;
        Time.timeScale = 1.0f;
        pausePanel.SetActive(false);
    }

    // funkcje na menu smierci
    public void GameOver()
    {
        deathPanel.SetActive(true);
        backToHub.onClick.AddListener(GoBack);
    }

    private void GoBack()
    {
        Player.instance.currentHealth = Player.instance.maxHealth;
        Player.instance.SaveInfo();
        PlayerPrefs.SetInt("GameState", 1);
        Time.timeScale = 1f;
        SceneManager.LoadScene(1);
    }

    private void Quit()
    {
        Player.instance.SaveInfo();
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
}
