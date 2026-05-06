using UnityEngine;

public class UlicaDoorsAnimation : MonoBehaviour
{
    private Vector3 openOffset = new(0f, 0f, 2f);
    private float moveSpeed = 2f;

    private Vector3 closedPosition;
    private Vector3 openPosition;
    private bool isOpening = false;

    void Start()
    {
        closedPosition = transform.position;
        openPosition = closedPosition + openOffset;
        EnemyPool.roomCleared += OpenDoorsUlica;
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

    private void OpenDoorsUlica()
    {
        isOpening = true;
    }

    private void OnDestroy()
    {
        EnemyPool.roomCleared -= OpenDoorsUlica;
    }
}
