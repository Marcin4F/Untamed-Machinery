using UnityEngine;
using System.Collections;

public class SpawnArrow : MonoBehaviour
{
    [SerializeField] GameObject arrows;

    Vector3 startPosition;

    private void Start()
    {
        NewArrow();
    }

    void NewArrow()
    {
        startPosition = transform.position;
        GameObject arrowsInstance = Instantiate(arrows);
        arrowsInstance.transform.position = startPosition;
        StartCoroutine(arrowsCooldown());
    }

    IEnumerator arrowsCooldown()
    {
        yield return new WaitForSeconds(0.5f);
        NewArrow();
    }
}
