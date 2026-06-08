using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Shop2 : MonoBehaviour
{
    [SerializeField] Button attackSpeedButton, weaponDamageButton, reloadSpeedButton, maxAmmoButton;
    [SerializeField] TMP_Text attackSpeedText, weaponDamageText, reloadSpeedText, maxAmmoText;

    private Color maxLvlColor = Color.red;
    private Color noMoneyColor = Color.gray;
    private Color normalColor = Color.white;

    private void Start()
    {
        if (attackSpeedButton != null) attackSpeedButton.onClick.AddListener(BuyAttackSpeed);
        if (weaponDamageButton != null) weaponDamageButton.onClick.AddListener(BuyWeaponDamage);
        if (reloadSpeedButton != null) reloadSpeedButton.onClick.AddListener(BuyReloadSpeed);
        if (maxAmmoButton != null) maxAmmoButton.onClick.AddListener(BuyMaxAmmo);
    }


    public void Activate()
    {
        RefreshUI();
    }

    private void RefreshUI()
    {
        UpdateShopButton(attackSpeedButton, attackSpeedText, Player.instance.attackCooldown <= 100, GameManagement.instance.EnoughMoney(800, 100, 50), "Buy Attack Speed");
        UpdateShopButton(weaponDamageButton, weaponDamageText, Player.instance.weaponDamage >= 80, GameManagement.instance.EnoughMoney(550, 150, 100), "Buy Weapon Damage");
        UpdateShopButton(reloadSpeedButton, reloadSpeedText, Player.instance.reloadSpeed <= 100, GameManagement.instance.EnoughMoney(700, 50, 150), "Buy Reload Speed");
        UpdateShopButton(maxAmmoButton, maxAmmoText, Player.instance.maxAmmo >= 50, GameManagement.instance.EnoughMoney(800, 150, 150), "Buy Max Ammo");
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

    private void BuyAttackSpeed()
    {
        if (Player.instance.attackCooldown > 100 && GameManagement.instance.EnoughMoney(800, 100, 50))
        {
            GameManagement.instance.SpendMoney(800, 100, 50);
            Player.instance.attackCooldown -= 25;
            InGameUI.instance.SetTextBuildingTwo();
            RefreshUI();
        }
    }

    private void BuyWeaponDamage()
    {
        if (Player.instance.weaponDamage < 80 && GameManagement.instance.EnoughMoney(550, 150, 100))
        {
            GameManagement.instance.SpendMoney(550, 150, 100);
            Player.instance.weaponDamage += 5;
            InGameUI.instance.SetTextBuildingTwo();
            RefreshUI();
        }
    }

    private void BuyReloadSpeed()
    {
        if (Player.instance.reloadSpeed > 100 && GameManagement.instance.EnoughMoney(700, 50, 150))
        {
            GameManagement.instance.SpendMoney(700, 50, 150);
            Player.instance.reloadSpeed -= 25;
            InGameUI.instance.SetTextBuildingTwo();
            RefreshUI();
        }
    }

    private void BuyMaxAmmo()
    {
        if (Player.instance.maxAmmo < 50 && GameManagement.instance.EnoughMoney(800, 150, 150))
        {
            GameManagement.instance.SpendMoney(800, 150, 150);
            Player.instance.maxAmmo += 5;
            Player.instance.currentAmmo = Player.instance.maxAmmo;
            InGameUI.instance.SetAmmo();
            InGameUI.instance.SetTextBuildingTwo();
            RefreshUI();
        }
    }
}
