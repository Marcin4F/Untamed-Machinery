using UnityEngine;

public class EnemyPool : MonoBehaviour
{
    public static EnemyPool instance;
    public delegate void RoomCleared();
    public static event RoomCleared roomCleared;

    bool given = false;

    [SerializeField] private GameObject enemiesContainer;

    private void Start()
    {
        instance = this;
        if (GameManagement.instance.cleared)
        {
            given = true;

            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (given)
        {
            return;
        }

        else if (GetComponentInChildren<Enemy>() == null)
        {
            // TODO DZWIEK: sukces
            given = true;
            roomCleared?.Invoke();
            GameManagement.instance.cleared = true;
        }
    }
}
