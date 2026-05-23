using UnityEngine;
using UnityEngine.UI;

public class Shop2 : MonoBehaviour
{
    [SerializeField] HealthBar healthBar;
    [SerializeField] Button attackSpeedButton, weaponDamageButton, reloadSpeedButton, maxAmmoButton;

    public void Activate()
    {
        attackSpeedButton.onClick.AddListener(BuyAttackSpeed);
        weaponDamageButton.onClick.AddListener(BuyWeaponDamage);
        reloadSpeedButton.onClick.AddListener(BuyReloadSpeed);
        maxAmmoButton.onClick.AddListener(BuyMaxAmmo);
    }

    private void BuyAttackSpeed()
    {
        if (Player.instance.attackCooldown > 100 && GameManagement.instance.EnoughMoney(800, 100, 50))
        {
            Player.instance.attackCooldown -= 25;
            InGameUI.instance.SetTextBuildingTwo();
            // DO DODANIA: Przerobiæ zabezpieczenie ilosci kupowania (dodac jakas zmiane w UI gdy ulepszymy na maksa)
        }
        else
        {
            Debug.Log("Nie mozna kupic");
            // DO DODANIA: Jakas informacja na UI o niewystarczajacej ilosci kasy
        }
    }

    private void BuyWeaponDamage()
    {
        if (Player.instance.weaponDamage < 80 && GameManagement.instance.EnoughMoney(550, 150, 100))
        {
            Player.instance.weaponDamage += 5;
            InGameUI.instance.SetTextBuildingTwo();
        }
        else
        {
            Debug.Log("Nie mozna kupic");
            // DO DODANIA: Jakas informacja na UI o niewystarczajacej ilosci kasy
        }
    }

    private void BuyReloadSpeed()
    {
        if (Player.instance.reloadSpeed > 100 && GameManagement.instance.EnoughMoney(700, 50, 150))
        {
            Player.instance.reloadSpeed -= 25;
            InGameUI.instance.SetTextBuildingTwo();
        }
        else
        {
            Debug.Log("Nie mozna kupic");
            // DO DODANIA: Jakas informacja na UI o niewystarczajacej ilosci kasy
        }
    }

    private void BuyMaxAmmo()
    {
        if (Player.instance.maxAmmo < 50 && GameManagement.instance.EnoughMoney(800, 150, 150))
        {
            Player.instance.maxAmmo += 5;
            Player.instance.currentAmmo = Player.instance.maxAmmo;
            InGameUI.instance.SetAmmo();
            InGameUI.instance.SetTextBuildingTwo();
        }
        else
        {
            Debug.Log("Nie mozna kupic");
            // DO DODANIA: Jakas informacja na UI o niewystarczajacej ilosci kasy
        }
    }
}
