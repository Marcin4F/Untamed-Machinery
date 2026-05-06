using UnityEngine;

public class Arrow : MonoBehaviour
{
    private Vector3 offset = new(36.5f, 0f, 0f);
    private float moveSpeed = 1f;
    private Vector3 startPosition;
    private Vector3 endPosition;

    void Start()
    {
        startPosition = transform.position;
        endPosition = startPosition + offset;
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, endPosition, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, endPosition) < 0.01f)
        {
            Destroy(gameObject);
        }
    }
}
