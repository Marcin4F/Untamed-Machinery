using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Cinemachine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public static Player instance;
    public HealthBar healthBar;

    public int maxHealth = 100, currentHealth = 100, maxAmmo = 20, currentAmmo = 20, minHealing = 10, maxHealing = 20, minReward = 200, maxReward = 400, lifeSteal = 2, invincibilityTime = 500,
        weaponDamage = 20, lifeStealChance = 0, reloadSpeed = 250, attackCooldown = 400;

    public bool alive = true;
    private bool invincibility = false;

    private WaitForSeconds invincibilityWait;

    private int damagingLayer;

    [SerializeField] GameObject shield;

    [Header("Shield Settings")]
    [SerializeField] float shieldDurationTime = 5.0f;
    [SerializeField] float shieldCooldown = 5.0f;
    private bool canShield = true;
    
    private Image shieldButtonImage;

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
        UpdateInvincibilityTimer();
        damagingLayer = LayerMask.NameToLayer("Damaging");
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

        GameObject reloadObj = GameObject.Find("ReloadButton");
        if (reloadObj != null)
        {
            Button reloadBtn = reloadObj.GetComponent<Button>();
            reloadBtn.onClick.RemoveAllListeners(); 
            reloadBtn.onClick.AddListener(MobileReload);
        }

        GameObject shieldObj = GameObject.Find("ShieldButton");
        if (shieldObj != null)
        {
            Button shieldBtn = shieldObj.GetComponent<Button>();
            shieldBtn.onClick.RemoveAllListeners();
            shieldBtn.onClick.AddListener(MobileShield);
            
            shieldButtonImage = shieldObj.GetComponent<Image>();
        }

        GameObject dashObj = GameObject.Find("DashButton");
        if (dashObj != null)
        {
            Button dashBtn = dashObj.GetComponent<Button>();
            dashBtn.onClick.RemoveAllListeners();
            dashBtn.onClick.AddListener(MobileDash);
        }
    }

    public void MobileReload()
    {
        if (!shooting.isReloading && currentAmmo < maxAmmo)
        {
            StartCoroutine(shooting.Reloading());
        }
    }

    public void MobileShield()
    {
        if (canShield && !shield.activeInHierarchy)
        {
            StartCoroutine(ShieldRoutine());
        }
    }

    public void MobileDash()
    {
        if (!alive) return; 

        PlayerAnimation playerAnim = GetComponent<PlayerAnimation>();
        if (playerAnim != null)
        {
            playerAnim.StartDash();
        }
    }

    public void UpdateInvincibilityTimer()
    {
        invincibilityWait = new WaitForSeconds(invincibilityTime / 1000.0f);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == damagingLayer)
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

    private IEnumerator ShieldRoutine()
    {
        canShield = false;
        shield.SetActive(true);

        if (shieldButtonImage != null)
        {
            Color c = shieldButtonImage.color;
            c.a = 10f / 255f; 
            shieldButtonImage.color = c;
        }

        yield return new WaitForSeconds(shieldDurationTime);

        shield.SetActive(false);

        yield return new WaitForSeconds(shieldCooldown);

        if (shieldButtonImage != null)
        {
            Color c = shieldButtonImage.color;
            c.a = 40f / 255f; 
            shieldButtonImage.color = c;
        }

        canShield = true;
    }
}