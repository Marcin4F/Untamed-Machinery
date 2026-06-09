using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using System.Collections;
using Unity.VisualScripting;

public class Blast : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    EnemyShooting[] muzzles;

    void Start()
    {
        muzzles = GetComponentsInChildren<EnemyShooting>();
    }

    // Update is called once per frame
    public IEnumerator Fire()
    {
        // yield return new WaitForSeconds(0.2f);

        for (int j = 0; j < 3; j++)
        {
            transform.Rotate(0, 15, 0);

            for (int i = 0; i < muzzles.Length; i++)
            {
                muzzles[i].FireAShot();
            }

            yield return new WaitForSeconds(0.2f);

            transform.Rotate(0, -15, 0);

            for (int i = 0; i < muzzles.Length; i++)
            {
                muzzles[i].FireAShot();
            }

            transform.Rotate(0, -15, 0);

            yield return new WaitForSeconds(0.2f);

            for (int i = 0; i < muzzles.Length; i++)
            {
                muzzles[i].FireAShot();
            }

            transform.Rotate(0, 15, 0);

            yield return new WaitForSeconds(0.2f);
        }
    }
}