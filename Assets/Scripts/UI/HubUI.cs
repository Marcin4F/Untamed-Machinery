using TMPro;
using UnityEngine;

public class HubUI : MonoBehaviour
{
    [SerializeField] GameObject building1Panel, building2Panel, building3Panel, enterTextPanel, startGamePanel;

    [SerializeField] TMP_Text maxHealthText, healingMinText, healingMaxText, healingLS, lifeStealText, invincibilityText, rewardMinText, rewardMaxText;
    [SerializeField] TMP_Text attackSpeedText, weaponDamageText, reloadSpeedText, maxAmmoText;

    void Start()
    {
        building1Panel.SetActive(false);
        building2Panel.SetActive(false);
        building3Panel.SetActive(false);
        enterTextPanel.SetActive(false);
        startGamePanel.SetActive(false);
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

    public void EnterTextDisplay()
    {
        enterTextPanel.SetActive(true);
    }

    public void EnterTextHide()
    {
        enterTextPanel.SetActive(false);
    }

    public void StartGameDisplay()
    {
        startGamePanel.SetActive(true);
    }

    public void StartGameHide()
    {
        startGamePanel.SetActive(false);
    }

    public void CloseBuilding()
    {
        building1Panel.SetActive(false);
        building2Panel.SetActive(false);
        building3Panel.SetActive(false);
    }

    public void SetTextBuildingOne()
    {
        maxHealthText.SetText(Player.instance.maxHealth.ToString());
        healingMinText.SetText("Min: " + Player.instance.minHealing.ToString());
        healingMaxText.SetText("Max: " + Player.instance.maxHealing.ToString());
        healingLS.SetText("LS: " + Player.instance.lifeSteal.ToString());
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
}
