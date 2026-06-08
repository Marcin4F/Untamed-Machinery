using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class Blast : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    EnemyShooting[] muzzles;

    void Start()
    {
        muzzles = GetComponentsInChildren<EnemyShooting>();
    }

    // Update is called once per frame
    public void Fire()
    {
        Debug.Log("Fired bullets: " + muzzles.Length);
        for (int i = 0; i < muzzles.Length; i++)
        {
            muzzles[i].FireAShot(); 
        }
    }
}
