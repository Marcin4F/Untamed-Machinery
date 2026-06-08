using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Shop1 : MonoBehaviour
{
    [SerializeField] HealthBar healthBar;
    [SerializeField] Button maxHealth, healing, lifeSteal, invincibilityTime, rewardAmound;
    [SerializeField] TMP_Text healthText, healingText, lifeStealText, invincibilityText, rewardAText;

    private Color maxLvlColor = Color.red;
    private Color noMoneyColor = Color.gray;
    private Color normalColor = Color.white;

    private void Start()
    {
        if (maxHealth != null) maxHealth.onClick.AddListener(BuyMaxHealth);
        if (healing != null) healing.onClick.AddListener(BuyHealing);
        if (lifeSteal != null) lifeSteal.onClick.AddListener(BuyLifeSteal);
        if (invincibilityTime != null) invincibilityTime.onClick.AddListener(BuyInvincibilityFrames);
        if (rewardAmound != null) rewardAmound.onClick.AddListener(BuyRewardAmount);
    }

    public void Activate()
    {
        RefreshUI();
    }

    private void RefreshUI()
    {
        UpdateShopButton(maxHealth, healthText, Player.instance.maxHealth >= 300, GameManagement.instance.EnoughMoney(100, 150, 600), "Buy Max Health");
        UpdateShopButton(healing, healingText, Player.instance.lifeSteal >= 20, GameManagement.instance.EnoughMoney(150, 150, 550), "Buy Healing Amount");
        UpdateShopButton(lifeSteal, lifeStealText, Player.instance.lifeStealChance >= 50, GameManagement.instance.EnoughMoney(100, 350, 500), "Buy Life Steal %");
        UpdateShopButton(invincibilityTime, invincibilityText, Player.instance.invincibilityTime >= 750, GameManagement.instance.EnoughMoney(175, 200, 750), "Buy Invincibility Time");
        UpdateShopButton(rewardAmound, rewardAText, Player.instance.minReward >= 400, GameManagement.instance.EnoughMoney(200, 50, 900), "Buy Rewards Amount");
    }

    private void UpdateShopButton(Button btn, TMP_Text txt, bool isMaxLvl, bool hasEnoughMoney, string defaultText)
    {
        if (btn == null || txt == null) return;

        if (isMaxLvl)
        {
            btn.interactable = false;
            txt.SetText("MAX LVL");
            txt.color = maxLvlColor;
            btn.image.color = maxLvlColor;
        }
        else if (!hasEnoughMoney)
        {
            btn.interactable = false;
            txt.SetText("Not enough money");
            txt.color = noMoneyColor;
            btn.image.color = noMoneyColor;
        }
        else
        {
            btn.interactable = true;
            txt.SetText(defaultText);
            txt.color = normalColor;
            btn.image.color = normalColor;
        }
    }

    private void BuyMaxHealth()
    {
        if(Player.instance.maxHealth < 300 && GameManagement.instance.EnoughMoney(100, 150, 600))
        {
            GameManagement.instance.SpendMoney(100, 150, 600);
            Player.instance.maxHealth += 20;
            Player.instance.currentHealth += 20;
            healthBar.AddMaxValue(20);
            InGameUI.instance.SetDisplayHP();
            InGameUI.instance.SetTextBuildingOne();
            RefreshUI();
        }
    }

    private void BuyHealing()
    {
        if (Player.instance.lifeSteal < 20 && GameManagement.instance.EnoughMoney(150, 150, 550))
        {
            GameManagement.instance.SpendMoney(150, 150, 550);
            Player.instance.minHealing += 10;
            Player.instance.maxHealing += 10;
            Player.instance.lifeSteal += 2;
            InGameUI.instance.SetTextBuildingOne();
            RefreshUI();
        }
    }

    private void BuyLifeSteal()
    {
        if (Player.instance.lifeStealChance < 50 && GameManagement.instance.EnoughMoney(100, 350, 500))
        {
            GameManagement.instance.SpendMoney(100, 350, 500);
            Player.instance.lifeStealChance += 5;
            InGameUI.instance.SetTextBuildingOne();
            RefreshUI();
        }
    }

    private void BuyInvincibilityFrames()
    {
        if (Player.instance.invincibilityTime < 750 && GameManagement.instance.EnoughMoney(175, 200, 750))
        {
            GameManagement.instance.SpendMoney(175, 200, 750);
            Player.instance.invincibilityTime += 25;
            InGameUI.instance.SetTextBuildingOne();
            RefreshUI();
        }
    }

    private void BuyRewardAmount()
    {
        if (Player.instance.minReward < 400 && GameManagement.instance.EnoughMoney(200, 50, 900))
        {
            GameManagement.instance.SpendMoney(200, 50, 900);
            Player.instance.minReward += 25;
            Player.instance.maxReward += 25;
            InGameUI.instance.SetTextBuildingOne();
            RefreshUI();
        }
    }
}
