using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagement : MonoBehaviour
{
    public static GameManagement instance;

    public int gameState = 0, currency1, currency2, currency3, roomsCleared = 0, rewardIndex;
    public bool cleared = false;

    void Awake()
    {
        if (instance == null)
        {
            // pierwsze uruchomienie gry, zapis instancji i ochrona przed zniszczeniem
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (SaveSystem.loadFromSave)
        {
            ApplySaveData();
            SaveSystem.loadFromSave = false;
        }
        else
        {
            InGameUI.instance.SetCurr1();
            InGameUI.instance.SetCurr2();
            InGameUI.instance.SetCurr3();
            InGameUI.instance.HideCurtain();
        }
    }

    private void ApplySaveData()
    {
        SaveData data = SaveSystem.Load();
        if (data != null)
        {
            // waluty i progres
            currency1 = data.currency1;
            currency2 = data.currency2;
            currency3 = data.currency3;
            roomsCleared = data.roomsCleared;

            // statystyki
            Player.instance.maxHealth = data.maxHealth;
            Player.instance.currentHealth = data.currentHealth;
            Player.instance.maxAmmo = data.maxAmmo;
            Player.instance.currentAmmo = data.currentAmmo;
            Player.instance.minHealing = data.minHealing;
            Player.instance.maxHealing = data.maxHealing;
            Player.instance.minReward = data.minReward;
            Player.instance.maxReward = data.maxReward;
            Player.instance.lifeSteal = data.lifeSteal;
            Player.instance.invincibilityTime = data.invincibilityTime;
            Player.instance.weaponDamage = data.weaponDamage;
            Player.instance.lifeStealChance = data.lifeStealChance;
            Player.instance.reloadSpeed = data.reloadSpeed;
            Player.instance.attackCooldown = data.attackCooldown;

            // odswierzenie UI
            InGameUI.instance.SetCurr1();
            InGameUI.instance.SetCurr2();
            InGameUI.instance.SetCurr3();
            Player.instance.healthBar.SetMaxValue(Player.instance.maxHealth);
            Player.instance.healthBar.SetHealth(Player.instance.currentHealth);
            InGameUI.instance.SetDisplayHP();
            InGameUI.instance.SetAmmo();

            // przeniesienie do odpowiedniego pokoju
            if (data.savedSceneIndex != 1)
            {
                cleared = true;
                SceneManager.LoadScene(data.savedSceneIndex);
            }
            else
            {
                // zapis byl w hubie wiêc podnosimy kurtyne
                InGameUI.instance.HideCurtain();
            }
        }
    }

    public bool EnoughMoney(int curr1, int curr2, int curr3)
    {
        if (curr1 <= currency1 && curr2 <= currency2 && curr3 <= currency3)
        {
            currency1 -= curr1;
            currency2 -= curr2;
            currency3 -= curr3;
            InGameUI.instance.SetCurr1();
            InGameUI.instance.SetCurr2();
            InGameUI.instance.SetCurr3();
            return true;
        }
        return false;
    }
}