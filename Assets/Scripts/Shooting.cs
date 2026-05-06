using System.Collections;
using Unity.Mathematics;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    [SerializeField] GameObject bullet;
    protected GameObject bulletInstance; //protected zamiast private zeby mozna bylo dziedziczyc
    protected Light muzzleFlash; // to samo

    Vector3 startPosition;
    quaternion startRotation;
    public bool shotReady = true, isReloading = false;

    void Start()
    {
        PlayerAnimation.firingGun += FireAShot;
        muzzleFlash = GetComponentInChildren<Light>();
        muzzleFlash.enabled = false;
    }

    void OnDestroy()
    {
        PlayerAnimation.firingGun -= FireAShot;
    }

    public virtual void FireAShot() // virtualna bo nadpisywana przez EnemyShooting
    {
        startPosition = transform.position;
        startRotation = transform.rotation;
        muzzleFlash.enabled = true;
        bulletInstance = Instantiate(bullet);
        bulletInstance.transform.SetPositionAndRotation(startPosition, startRotation);
        StartCoroutine(MuzzleFlashVisibility());
    }

    private IEnumerator MuzzleFlashVisibility()
    {
        yield return new WaitForSeconds(0.1f);
        muzzleFlash.enabled = false;
    }

    public IEnumerator ShootingCooldown()
    {
        yield return new WaitForSeconds(Player.instance.attackCooldown / 1000.0f);
        shotReady = true;
    }

    public IEnumerator Reloading()
    {
        isReloading = true;
        Player.instance.currentAmmo = 0;
        InGameUI.instance.SetAmmo();
        yield return new WaitForSeconds(Player.instance.reloadSpeed / 100.0f);
        Player.instance.currentAmmo = Player.instance.maxAmmo;
        InGameUI.instance.SetAmmo();
        shotReady = true;
        isReloading = false;
    }
}
