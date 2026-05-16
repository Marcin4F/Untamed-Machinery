using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player instance;
    public HealthBar healthBar;

    public int maxHealth = 100, currentHealth = 100, maxAmmo = 20, currentAmmo = 20, minHealing = 10, maxHealing = 20, minReward = 80, maxReward = 200, lifeSteal = 2, invincibilityTime = 500,
        weaponDamage = 20, lifeStealChance = 0, reloadSpeed = 250, attackCooldown = 400;

    public bool alive = true;
    private bool invincibility = false;
    Shooting shooting;

    void Start()
    {
        instance = this;
        shooting = GetComponentInChildren<Shooting>();

        if(GameManagement.instance.gameState == 0)      // jezeli gameState = 0, to nie wczytujemy czesci parametrow
        {
            currentHealth = maxHealth;
            healthBar.SetHealth(currentHealth);

            PlayerPrefs.SetInt("maxHealth", maxHealth);
            PlayerPrefs.SetInt("weaponDamage", weaponDamage);
            PlayerPrefs.SetInt("reloadSpeed", reloadSpeed);
            PlayerPrefs.SetInt("attackCooldown", attackCooldown);
            PlayerPrefs.SetInt("invincibilityTime", invincibilityTime);
            PlayerPrefs.SetInt("lifeStealChance", lifeStealChance);
            PlayerPrefs.SetInt("maxAmmo", maxAmmo);
            PlayerPrefs.SetInt("minHealing", minHealing);
            PlayerPrefs.SetInt("maxHealing", maxHealing);
            PlayerPrefs.SetInt("minReward", minReward);
            PlayerPrefs.SetInt("maxReward", maxReward);
            PlayerPrefs.SetInt("lifeSteal", lifeSteal);
        }
        else                    // wczytanie parametrow z poprzedniego pokoju
        {
            GetInfo();
            healthBar.SetHealth(currentHealth);
        }

        currentAmmo = maxAmmo;
        InGameUI.instance.SetDisplayHP();
        InGameUI.instance.SetAmmo();
        healthBar.SetMaxValue(maxHealth);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(shooting.Reloading());
        }
        //else if (Input.GetKeyDown(KeyCode.U))       // DO TESTOW
        //{
        //    GameManagement.instance.currency1 += 1000;
        //    InGameUI.instance.SetCurr1();
        //    GameManagement.instance.currency2 += 1000;
        //    InGameUI.instance.SetCurr2();
        //    GameManagement.instance.currency3 += 1000;
        //    InGameUI.instance.SetCurr3();
        //}
    }

    // DOSTAWANIE OBRAZEN OD PRZECIWNIKOW PRZENIESC NA ICH SKRYPTY
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("Enemy"))
        {
            if (hit.gameObject.TryGetComponent<Enemy>(out var enemy))
            {
                // TakeDamage(enemy.maleDamage);
            }
            else
                Debug.LogError("No 'Enemy' component on object with 'Enemy' tag");
        }
        else if (hit.gameObject.CompareTag("Damaging"))
            TakeDamage(5);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Damaging"))
            TakeDamage(5);
    }

    private IEnumerator InvincibilityFrames()       // klatki niesmiertelnosci (bez tego "ciagle" obrazenia natychmiastowo zabijaja)
    {
        // DO DODANIA efekt wizualny np. mruganie postaci na czerwono
        yield return new WaitForSeconds(invincibilityTime/1000.0f);      // czas trwania w sekundach
        invincibility = false;
    }

    public void TakeDamage(int damage)       // otrzymywanie obrazen (dajac za parametr wartosc ujemna dziala jako leczenie)
    {
        if (!invincibility && damage > 0)
        {
            invincibility = true;
            currentHealth -= damage;

            healthBar.SetHealth(currentHealth);     // zmiana poziomu paska hp
            StartCoroutine(InvincibilityFrames());

            if (currentHealth <= 0)
            {
                Time.timeScale = 0f;
                InGameUI.instance.GameOver();
                alive = false;
                SaveInfo();
                
            }
        }

        else if (damage < 0)
        {
            currentHealth -= damage;
            if (currentHealth > maxHealth)      // hp nie moze byc wieksze do max hp
                currentHealth = maxHealth;

            healthBar.SetHealth(currentHealth);
        }
        InGameUI.instance.SetDisplayHP();
    }

    public void GetInfo()
    {
        maxHealth = PlayerPrefs.GetInt("maxHealth");
        weaponDamage = PlayerPrefs.GetInt("weaponDamage");
        reloadSpeed = PlayerPrefs.GetInt("reloadSpeed");
        attackCooldown = PlayerPrefs.GetInt("attackCooldown");
        currentHealth = PlayerPrefs.GetInt("PlayerHealth");
        invincibilityTime = PlayerPrefs.GetInt("invincibilityTime");
        lifeStealChance = PlayerPrefs.GetInt("lifeStealChance");
        maxAmmo = PlayerPrefs.GetInt("maxAmmo");
        minHealing = PlayerPrefs.GetInt("minHealing");
        maxHealing = PlayerPrefs.GetInt("maxHealing");
        minReward = PlayerPrefs.GetInt("minReward");
        maxReward = PlayerPrefs.GetInt("maxReward");
        lifeSteal = PlayerPrefs.GetInt("lifeSteal");
    }

    public void SaveInfo()              // zapis informacji do plikow
    {
        PlayerPrefs.SetInt("PlayerHealth", currentHealth);
        PlayerPrefs.SetInt("Currency1", GameManagement.instance.currency1);
        PlayerPrefs.SetInt("Currency2", GameManagement.instance.currency2);
        PlayerPrefs.SetInt("Currency3", GameManagement.instance.currency3);
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.DeleteAll();
    }
}
