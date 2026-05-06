using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;

    int oldMaxValue;

    public void SetHealth(int health)     // ustawienie wartosci slidera
    {
        slider.value = health;
    }

    public void AddMaxValue(int valueToAdd)
    {
        oldMaxValue = (int) slider.maxValue;
        slider.maxValue = oldMaxValue + valueToAdd;
        SetHealth(Player.instance.currentHealth);
    }

    public void SetMaxValue(int maxValue)
    {
        slider.maxValue = maxValue;
        SetHealth(Player.instance.currentHealth);
    }
}
