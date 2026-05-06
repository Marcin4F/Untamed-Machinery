using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Shop1 : MonoBehaviour
{
    [SerializeField] HealthBar healthBar;
    [SerializeField] Button maxHealth, healing, lifeSteal, rewardAmound, invincibilityTime;

    [SerializeField] HubUI hubUI;

    public void Activate()
    {
        maxHealth.onClick.AddListener(BuyMaxHealth);
        healing.onClick.AddListener(BuyHealing);
        lifeSteal.onClick.AddListener(BuyLifeSteal);
        rewardAmound.onClick.AddListener(BuyRewardAmount);
        invincibilityTime.onClick.AddListener(BuyInvincibilityFrames);
    }

    private void BuyMaxHealth()
    {
        if(Player.instance.maxHealth < 300)
        {
            if(GameManagement.instance.EnoughMoney(100, 150, 600))
            {
                Player.instance.maxHealth += 20;
                Player.instance.currentHealth += 20;
                healthBar.AddMaxValue(20);
                InGameUI.instance.SetDisplayHP();
                PlayerPrefs.SetInt("maxHealth", Player.instance.maxHealth);
                hubUI.SetTextBuildingOne();
            }
        }
        else
        {
            Debug.Log("Nie mozna kupic");
            // DO DODANIA: Jakas informacja na UI o niewystarczajacej ilosci kasy
        }
    }

    private void BuyHealing()
    {
        if (Player.instance.lifeSteal < 20 && GameManagement.instance.EnoughMoney(150, 150, 550))
        {
            Player.instance.minHealing += 10;
            Player.instance.maxHealing += 10;
            Player.instance.lifeSteal += 2;
            PlayerPrefs.SetInt("minHealing", Player.instance.minHealing);
            PlayerPrefs.SetInt("maxHealing", Player.instance.maxHealing);
            PlayerPrefs.SetInt("lifeSteal", Player.instance.lifeSteal);
            hubUI.SetTextBuildingOne();
        }
        else
        {
            Debug.Log("Nie mozna kupic");
            // DO DODANIA: Jakas informacja na UI o niewystarczajacej ilosci kasy
        }
    }

    private void BuyLifeSteal()
    {
        if (Player.instance.lifeStealChance < 50 && GameManagement.instance.EnoughMoney(100, 350, 500))
        {
            Player.instance.lifeStealChance += 5;
            PlayerPrefs.SetInt("lifeStealChance", Player.instance.lifeStealChance);
            hubUI.SetTextBuildingOne();
        }
        else
        {
            Debug.Log("Nie mozna kupic");
            // DO DODANIA: Jakas informacja na UI o niewystarczajacej ilosci kasy
        }
    }

    private void BuyInvincibilityFrames()
    {
        if (Player.instance.invincibilityTime < 750 && GameManagement.instance.EnoughMoney(175, 200, 750))
        {
            Player.instance.invincibilityTime += 25;
            PlayerPrefs.SetInt("invincibilityTime", Player.instance.invincibilityTime);
            hubUI.SetTextBuildingOne();
        }
        else
        {
            Debug.Log("Nie mozna kupic");
            // DO DODANIA: Jakas informacja na UI o niewystarczajacej ilosci kasy
        }
    }

    private void BuyRewardAmount()
    {
        if (Player.instance.minReward < 280 && GameManagement.instance.EnoughMoney(200, 50, 900))
        {
            Player.instance.minReward += 25;
            Player.instance.maxReward += 25;
            PlayerPrefs.SetInt("minReward", Player.instance.minReward);
            PlayerPrefs.SetInt("maxReward", Player.instance.maxReward);
            hubUI.SetTextBuildingOne();
        }
        else
        {
            Debug.Log("Nie mozna kupic");
            // DO DODANIA: Jakas informacja na UI o niewystarczajacej ilosci kasy
        }
    }
}
