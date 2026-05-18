using UnityEngine;

public class GameManagement : MonoBehaviour
{
    public static GameManagement instance;

    public int gameState = 0, currency1, currency2, currency3, roomsCleared = 0, rewardIndex;
    public bool cleared = false;

    void Awake()
    {
        if (instance == null)
        {
            // pierwsze uruchomienie gry, zapis instancji i ochrona przed zniszczeniem
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        InGameUI.instance.SetCurr1();
        InGameUI.instance.SetCurr2();
        InGameUI.instance.SetCurr3();
    }

    public bool EnoughMoney(int curr1, int curr2, int curr3)
    {
        if (curr1 <= currency1 && curr2 <= currency2 && curr3 <= currency3)
        {
            currency1 -= curr1;
            currency2 -= curr2;
            currency3 -= curr3;
            InGameUI.instance.SetCurr1();
            InGameUI.instance.SetCurr2();
            InGameUI.instance.SetCurr3();
            return true;
        }
        return false;
    }
}