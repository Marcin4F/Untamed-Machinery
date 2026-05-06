using UnityEngine;

public class DoorsAnimation : MonoBehaviour
{
    private Vector3 openOffset = new (2f, 0f, 0f);
    private float moveSpeed = 2f;

    private Vector3 closedPosition;
    private Vector3 openPosition;
    private bool isOpening = false;

    void Start()
    {
        closedPosition = transform.position;
        openPosition = closedPosition + openOffset;
        EnemyPool.roomCleared += OpenDoors;
    }

    // Update is called once per frame
    void Update()
    {
        if (isOpening)
        {
            transform.position = Vector3.MoveTowards(transform.position, openPosition, moveSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, openPosition) < 0.01f)
            {
                transform.position = openPosition;
                isOpening = false;
            }
        }
    }

    private void OpenDoors()
    {
        isOpening = true;
    }

    private void OnDestroy()
    {
        EnemyPool.roomCleared -= OpenDoors;
    }
}
