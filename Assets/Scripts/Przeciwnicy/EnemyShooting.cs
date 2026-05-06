//using System.Collections;
//using Unity.Mathematics;
//using UnityEngine;

//public class EnemyShooting : Shooting
//{
//    Enemy enemy;

//    void Start()
//    {
//        enemy = GetComponentInParent<Enemy>();
//        muzzleFlash = GetComponentInChildren<Light>();
//        muzzleFlash.enabled = false;

//        InvokeRepeating("FireAShot", 1.0f, 2f);
//    }

//    protected override void FireAShot()
//    {
//        if (!enemy.canShoot) return;
//        base.FireAShot();


//        // ponizej logika zwiazana z ustawieniem obrazen jakie zadaje Enemy
//        if (bulletInstance != null)
//        {
//            Bullet bulletScript = bulletInstance.GetComponent<Bullet>();
//            if (bulletScript != null)
//            {
//                bulletScript.SetShotDamage(enemy.shotDamage);
//            }
//        }
//    }
//}

/// stara wersja wyzej

using System.Collections;
using UnityEngine;

public class EnemyShooting : Shooting
{
    [SerializeField] int damage = 10;

    void Start()
    {
        muzzleFlash = GetComponentInChildren<Light>();
        if (muzzleFlash == null)
            muzzleFlash = GetComponentInParent<Light>();
        if (muzzleFlash != null)
            muzzleFlash.enabled = false;
    }

    public override void FireAShot()
    {
        base.FireAShot();

        // ponizej logika zwiazana z ustawieniem obrazen jakie zadaje Enemy
        if (bulletInstance != null)
        {
            // ta linia dlatego ze w gore lecialy pociski nwm czy problem z exportem z blender a czy jak
            bulletInstance.transform.forward = -1 * transform.up;
            //

            Bullet bulletScript = bulletInstance.GetComponent<Bullet>();
            if (bulletScript != null)
            {
                bulletScript.SetShotDamage(damage);
            }
        }
    }
}
