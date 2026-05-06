using UnityEngine;

public class Blades : MonoBehaviour
{
    private Vector3 rotationAxis = Vector3.forward;
    private float rotationSpeed = 720f, moveSpeed = 8f;
    private Vector3 pom, localPointA, targetLocalPoint, localPointB;

    private void Start()
    {
        localPointA = transform.localPosition;
        localPointB = transform.localPosition + new Vector3(-12, 0, 0);
        targetLocalPoint = localPointB;
    }

    void Update()
    {
        transform.Rotate(rotationAxis.normalized * rotationSpeed * Time.deltaTime);

        transform.localPosition = Vector3.MoveTowards(transform.localPosition, targetLocalPoint, moveSpeed * Time.deltaTime);

        // Zmiana kierunku po dotarciu
        if (Vector3.Distance(transform.localPosition, targetLocalPoint) < 0.01f)
        {
            targetLocalPoint = (targetLocalPoint == localPointA) ? localPointB : localPointA;
        }
    }
}
