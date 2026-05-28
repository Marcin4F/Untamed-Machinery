using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Cinemachine;

public class Player : MonoBehaviour
{
    public static Player instance;
    public HealthBar healthBar;

    public int maxHealth = 100, currentHealth = 100, maxAmmo = 20, currentAmmo = 20, minHealing = 10, maxHealing = 20, minReward = 80, maxReward = 200, lifeSteal = 2, invincibilityTime = 500,
        weaponDamage = 20, lifeStealChance = 0, reloadSpeed = 250, attackCooldown = 400;

    public bool alive = true;
    private bool invincibility = false;

    private WaitForSeconds invincibilityWait;

    Shooting shooting;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            // nie trzeba DontDestroyOnLoad bo robi to juz GameManagement na tym samym obiekcie
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        shooting = GetComponentInChildren<Shooting>();
        currentAmmo = maxAmmo;
        healthBar.SetMaxValue(maxHealth);
        healthBar.SetHealth(currentHealth);
        InGameUI.instance.SetDisplayHP();
        InGameUI.instance.SetAmmo();
        invincibilityWait = new WaitForSeconds(invincibilityTime / 1000.0f); // czas trwania w sekundach
    }

    // wlaczenie nasluchwania na zaladownianie nowej sceny
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // wylaczenie nasluchiwania
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // przy zmianie sceny
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // znalezienie punktu spawnu
        GameObject spawnPoint = GameObject.FindWithTag("SpawnPoint");

        if (spawnPoint != null)
        {
            // wylaczenie CharacterController bo blokuje fizyczne przenoszenie obiektu
            CharacterController cc = GetComponent<CharacterController>();
            if (cc != null) cc.enabled = false;

            transform.position = spawnPoint.transform.position;
            transform.rotation = spawnPoint.transform.rotation;

            if (cc != null) cc.enabled = true;
        }

        CinemachineCamera vcam = FindFirstObjectByType<CinemachineCamera>();
        if (vcam != null)
        {
            vcam.Follow = transform;
        }

        if (scene.buildIndex != 1)
        {
            InGameUI.instance.HideCurtain();
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && !shooting.isReloading && currentAmmo < maxAmmo)
        {
            StartCoroutine(shooting.Reloading());
        }
        else if (Input.GetKeyDown(KeyCode.U))       // DO TESTOW
        {
            GameManagement.instance.currency1 += 1000;
            InGameUI.instance.SetCurr1();
            GameManagement.instance.currency2 += 1000;
            InGameUI.instance.SetCurr2();
            GameManagement.instance.currency3 += 1000;
            InGameUI.instance.SetCurr3();
        }
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

    // TODO: optymalizacja (cos innego zamiast onTriggerStay 
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Damaging"))
            TakeDamage(5);
    }

    private IEnumerator InvincibilityFrames()       // klatki niesmiertelnosci (bez tego "ciagle" obrazenia natychmiastowo zabijaja)
    {
        // DO DODANIA efekt wizualny np. mruganie postaci na czerwono
        yield return invincibilityWait;
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
}
