using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InGameUI : MonoBehaviour
{
    public static InGameUI instance;

    [Header("Ekrany (Panels)")]
    [SerializeField] GameObject deathPanel;
    [SerializeField] GameObject pausePanelNoSave;       // pauza bez mozliwosci zapisu
    [SerializeField] GameObject pausePanelWithSave;     // pauza z mozliwoscia zapisu

    [Header("Przyciski - Œmieræ")]
    [SerializeField] Button backToHub;

    [Header("Przyciski - Pauza")]
    [SerializeField] Button pauseButton;

    [Header("Przyciski - Pauza (Brak Zapisu)")]
    [SerializeField] Button continueNoButton;
    [SerializeField] Button quitMenuNoButton;

    [Header("Przyciski - Pauza (Z Zapisem)")]
    [SerializeField] Button continueSaveButton;
    [SerializeField] Button saveButton, quitMenuSaveButton;


    [Header("Ekran £adowania")]
    [SerializeField] GameObject loadingCurtain;

    [Header("Zasoby")]
    public TMP_Text currency1;
    public TMP_Text currency2, currency3, ammo;             // tekst z iloscia danej waluty i ilosc amunicji
    [SerializeField] TMP_Text displayHP;                    // tekst z HP wyswietlanym na sliderze

    [Header("Hub UI Elements")]
    [SerializeField] GameObject building1Panel;
    [SerializeField] GameObject building2Panel, building3Panel, enterShopPanel, startGamePanel;
    [SerializeField] TMP_Text maxHealthText, healingMinText, healingMaxText, healingLS, lifeStealText, invincibilityText, rewardMinText, rewardMaxText;
    [SerializeField] TMP_Text attackSpeedText, weaponDamageText, reloadSpeedText, maxAmmoText;
    [SerializeField] TMP_Text shopButtonText;
    [SerializeField] Button startGameButton, openCloseShopButton;

    [Header("Elementy sklepu")]
    [SerializeField] Shop1 shop1;
    [SerializeField] Shop2 shop2;
    public int shopIndex = 1;

    private bool isPaused = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(transform.root.gameObject);
        }
        else if (instance != this)
        {
            Destroy(transform.root.gameObject);
            return;
        }

        if (deathPanel != null) deathPanel.SetActive(false);
        if (pausePanelNoSave != null) pausePanelNoSave.SetActive(false);
        if (pausePanelWithSave != null) pausePanelWithSave.SetActive(false);

        // hub
        if (building1Panel != null) building1Panel.SetActive(false);
        if (building2Panel != null) building2Panel.SetActive(false);
        if (building3Panel != null) building3Panel.SetActive(false);
        if (enterShopPanel != null) enterShopPanel.SetActive(false);
        if (startGamePanel != null) startGamePanel.SetActive(false);
        if (pauseButton != null) { pauseButton.onClick.RemoveAllListeners(); pauseButton.onClick.AddListener(TogglePause); }
    }

    // ------- metody in-game -------
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


    // ------- funkcje menu pauzy -------
    private void TogglePause()
    {
        if (!isPaused)
            PauseMenu();

        else
            ResumeGame();
    }

    private void PauseMenu()    // pauzowanie gry
    {
        isPaused = true;
        Time.timeScale = 0f;
        int currentScene = SceneManager.GetActiveScene().buildIndex;

        // sprawdzenie czy mozna zapisac gre
        bool isHub = (currentScene == 1);
        bool isCleared = GameManagement.instance.cleared;
        bool canSave = (isHub || isCleared);

        if (canSave)
        {
            pausePanelWithSave.SetActive(true);
            pausePanelNoSave.SetActive(false);

            // logika przyciskow plus czyszczenie poprzedniej zeby 2 razy sie nie wykonywala
            if (continueSaveButton != null) { continueSaveButton.onClick.RemoveAllListeners(); continueSaveButton.onClick.AddListener(ResumeGame); }
            if (saveButton != null) { saveButton.onClick.RemoveAllListeners(); saveButton.onClick.AddListener(SaveGameToJSON); }
            if (quitMenuSaveButton != null) { quitMenuSaveButton.onClick.RemoveAllListeners(); quitMenuSaveButton.onClick.AddListener(Quit); }
        }
        else
        {
            pausePanelWithSave.SetActive(false);
            pausePanelNoSave.SetActive(true);

            if (continueNoButton != null) { continueNoButton.onClick.RemoveAllListeners(); continueNoButton.onClick.AddListener(ResumeGame); }
            if (quitMenuNoButton != null) { quitMenuNoButton.onClick.RemoveAllListeners(); quitMenuNoButton.onClick.AddListener(Quit); }
        }
    }

    private void ResumeGame()       // wznawianie gry
    {
        isPaused = false;
        Time.timeScale = 1.0f;
        pausePanelNoSave.SetActive(false);
        pausePanelWithSave.SetActive(false);
    }

    private void SaveGameToJSON()
    {
        SaveData data = new SaveData();

        // pobranie danych swiata
        data.savedSceneIndex = SceneManager.GetActiveScene().buildIndex;
        data.currency1 = GameManagement.instance.currency1;
        data.currency2 = GameManagement.instance.currency2;
        data.currency3 = GameManagement.instance.currency3;
        data.roomsCleared = GameManagement.instance.roomsCleared;
        data.gameState = GameManagement.instance.gameState;

        // pobieranie statystyk gracza
        data.maxHealth = Player.instance.maxHealth;
        data.currentHealth = Player.instance.currentHealth;
        data.maxAmmo = Player.instance.maxAmmo;
        data.currentAmmo = Player.instance.currentAmmo;
        data.minHealing = Player.instance.minHealing;
        data.maxHealing = Player.instance.maxHealing;
        data.minReward = Player.instance.minReward;
        data.maxReward = Player.instance.maxReward;
        data.lifeSteal = Player.instance.lifeSteal;
        data.invincibilityTime = Player.instance.invincibilityTime;
        data.weaponDamage = Player.instance.weaponDamage;
        data.lifeStealChance = Player.instance.lifeStealChance;
        data.reloadSpeed = Player.instance.reloadSpeed;
        data.attackCooldown = Player.instance.attackCooldown;

        // zapis
        SaveSystem.Save(data);
        Debug.Log("Zapisano poprawnie z poziomu InGameUI!");

        ResumeGame();
    }

    private void Quit()
    {
        Time.timeScale = 1f;

        if (Player.instance != null) Destroy(Player.instance.gameObject);
        if (GameManagement.instance != null) Destroy(GameManagement.instance.gameObject);
        Destroy(transform.root.gameObject);

        SceneManager.LoadScene(0);
    }

    // ------- menu smierci -------
    public void GameOver()
    {
        deathPanel.SetActive(true);
        if (backToHub != null) { backToHub.onClick.RemoveAllListeners(); backToHub.onClick.AddListener(GoBack); }
    }

    private void GoBack()
    {
        Player.instance.currentHealth = Player.instance.maxHealth;
        GameManagement.instance.gameState = 1;
        Player.instance.alive = true;
        Time.timeScale = 1f;
        SceneManager.LoadScene(1);
    }

    public void HideDeathPanel()
    {
        if (deathPanel != null)
            deathPanel.SetActive(false);
    }

    // ------- metody ekranu laodwania -------
    public void ShowCurtain()
    {
        if (loadingCurtain != null) loadingCurtain.SetActive(true);
    }

    public void HideCurtain()
    {
        if (loadingCurtain != null) loadingCurtain.SetActive(false);
    }

    // ------- metody hub -------
    public void SetTextBuildingOne()
    {
        maxHealthText.SetText(Player.instance.maxHealth.ToString());
        healingMinText.SetText("Min: " + Player.instance.minHealing.ToString());
        healingMaxText.SetText("Max: " + Player.instance.maxHealing.ToString());
        healingLS.SetText("LifeSteal: " + Player.instance.lifeSteal.ToString());
        lifeStealText.SetText(Player.instance.lifeStealChance.ToString() + "%");
        invincibilityText.SetText((Player.instance.invincibilityTime / 1000.0f).ToString());
        rewardMinText.SetText("Min: " + Player.instance.minReward.ToString());
        rewardMaxText.SetText("Max: " + Player.instance.maxReward.ToString());
    }

    public void SetTextBuildingTwo()
    {
        attackSpeedText.SetText((Player.instance.attackCooldown / 1000.0f).ToString());
        weaponDamageText.SetText(Player.instance.weaponDamage.ToString());
        reloadSpeedText.SetText((Player.instance.reloadSpeed / 100.0f).ToString());
        maxAmmoText.SetText(Player.instance.maxAmmo.ToString());
    }

    public void OpenBuilding(int index)     // otwieranie menu danego budynku
    {
        switch (index)
        {
            case 1:
                building1Panel.SetActive(true);
                SetTextBuildingOne();
                break;
            case 2:
                building2Panel.SetActive(true);
                SetTextBuildingTwo();
                break;
            case 3:
                building3Panel.SetActive(true);
                break;
            default:
                Debug.LogError("Open building Panel");
                break;
        }
    }

    public void EnterPanelDisplay()
    {
        enterShopPanel.SetActive(true);
        if (openCloseShopButton != null) { openCloseShopButton.onClick.RemoveAllListeners(); openCloseShopButton.onClick.AddListener(OpenShop); }
    }

    public void EnterPanelHide()
    {
        enterShopPanel.SetActive(false);
    }

    private void OpenShop()
    {
        shopButtonText.SetText("Close shop");
        if (openCloseShopButton != null) { openCloseShopButton.onClick.RemoveAllListeners(); openCloseShopButton.onClick.AddListener(CloseShop); }
        OpenBuilding(shopIndex);
        if (shopIndex == 1)
            shop1.Activate();
        else
            shop2.Activate();
    }

    public void CloseShop()
    {
        shopButtonText.SetText("Open shop");
        if (openCloseShopButton != null) { openCloseShopButton.onClick.RemoveAllListeners(); openCloseShopButton.onClick.AddListener(OpenShop); }
        building1Panel.SetActive(false);
        building2Panel.SetActive(false);
        building3Panel.SetActive(false);
    }

    public void StartGameDisplay()
    {
        startGamePanel.SetActive(true);
        if (startGameButton != null) { startGameButton.onClick.RemoveAllListeners(); startGameButton.onClick.AddListener(GameStarter); }
    }

    public void StartGameHide()
    {
        startGamePanel.SetActive(false);
    }

    private void GameStarter()
    {
        GameManagement.instance.roomsCleared = 0;
        GameManagement.instance.gameState = 1;
        InGameUI.instance.StartGameHide();
        SceneManager.LoadScene(Random.Range(2, 8));
        //SceneManager.LoadScene(26);
    }
}
