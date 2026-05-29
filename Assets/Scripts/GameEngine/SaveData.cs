using System;

[Serializable]
public class SaveData
{
    // zmienne z GameManagement
    public int currency1;
    public int currency2;
    public int currency3;
    public int roomsCleared;
    public int savedSceneIndex; // indeks sceny na ktorej zapisujemy
    public int gameState;

    // zmienne z Player
    public int maxHealth;
    public int currentHealth;
    public int maxAmmo;
    public int currentAmmo;
    public int minHealing;
    public int maxHealing;
    public int minReward;
    public int maxReward;
    public int lifeSteal;
    public int invincibilityTime;
    public int weaponDamage;
    public int lifeStealChance;
    public int reloadSpeed;
    public int attackCooldown;
}