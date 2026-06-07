using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Cinemachine;

public class Player : MonoBehaviour
{
    public static Player instance;
    public HealthBar healthBar;

    public int maxHealth = 100, currentHealth = 100, maxAmmo = 20, currentAmmo = 20, minHealing = 10, maxHealing = 20, minReward = 200, maxReward = 400, lifeSteal = 2, invincibilityTime = 500,
        weaponDamage = 20, lifeStealChance = 0, reloadSpeed = 250, attackCooldown = 400;

    public bool alive = true;
    private bool invincibility = false;

    private WaitForSeconds invincibilityWait;

    [SerializeField] GameObject shield;

    Shooting shooting;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
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
        invincibilityWait = new WaitForSeconds(invincibilityTime / 1000.0f);
        shield.SetActive(false);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        healthBar.SetHealth(currentHealth);
        shield.SetActive(false);
        currentAmmo = maxAmmo;

        InGameUI.instance.SetDisplayHP();
        InGameUI.instance.SetAmmo();
        InGameUI.instance.HideDeathPanel();

        GameObject spawnPoint = GameObject.FindWithTag("SpawnPoint");

        if (spawnPoint != null)
        {
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
        // Kept for PC testing
        if (Input.GetKeyDown(KeyCode.R)) MobileReload();
        if (Input.GetKeyDown(KeyCode.Q)) MobileShield();
        
        if (Input.GetKeyDown(KeyCode.U))
        {
            GameManagement.instance.currency1 += 1000;
            InGameUI.instance.SetCurr1();
            GameManagement.instance.currency2 += 1000;
            InGameUI.instance.SetCurr2();
            GameManagement.instance.currency3 += 1000;
            InGameUI.instance.SetCurr3();
        }
    }

    // Call this from a UI Button OnClick() event
    public void MobileReload()
    {
        if (!shooting.isReloading && currentAmmo < maxAmmo)
        {
            StartCoroutine(shooting.Reloading());
        }
    }

    // Call this from a UI Button OnClick() event
    public void MobileShield()
    {
        if (!shield.activeInHierarchy)
        {
            shield.SetActive(true);
            StartCoroutine(ShieldDuration());
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Damaging"))
            TakeDamage(5);
    }

    private IEnumerator InvincibilityFrames()
    {
        yield return invincibilityWait;
        invincibility = false;
    }

    public void TakeDamage(int damage)
    {
        if (!invincibility && damage > 0)
        {
            invincibility = true;
            currentHealth -= damage;

            healthBar.SetHealth(currentHealth);
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
            if (currentHealth > maxHealth)
                currentHealth = maxHealth;

            healthBar.SetHealth(currentHealth);
        }
        InGameUI.instance.SetDisplayHP();
    }

    IEnumerator ShieldDuration()
    {
        yield return new WaitForSeconds(5);
        shield.SetActive(false);
    }
}