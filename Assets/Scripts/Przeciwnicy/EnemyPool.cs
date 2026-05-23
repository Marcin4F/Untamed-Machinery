using UnityEngine;

public class EnemyPool : MonoBehaviour
{
    public static EnemyPool instance;
    public delegate void RoomCleared();
    public static event RoomCleared roomCleared;

    bool given = false;

    private void Start()
    {
        instance = this;
        GameManagement.instance.cleared = false;
    }

    private void Update()
    {
        if (given)
        {
            return;
        }

        else if (GetComponentInChildren<Enemy>() == null)
        {
            Debug.Log("a");
            given = true;
            roomCleared?.Invoke();
            GameManagement.instance.cleared = true;
        }
    }
}
