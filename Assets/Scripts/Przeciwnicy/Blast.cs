using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using System.Collections;

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
        yield return new WaitForSeconds(1);

        for (int i = 0; i < muzzles.Length; i++)
        {
            muzzles[i].FireAShot(); 
        }
    }
}
