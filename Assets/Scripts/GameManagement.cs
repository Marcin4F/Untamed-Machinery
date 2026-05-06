using UnityEngine;

public class GameManagement : MonoBehaviour
{
    public static GameManagement instance;

    public int gameState = 0, currency1, currency2, currency3;
    public bool cleared = false;

    void Start()
    {
        instance = this;
        gameState = PlayerPrefs.GetInt("GameState");        // status gry - potrzebne do wczytywania zapisanych danych o graczu

        if (gameState == 0)      // jezeli gameState = 0, to nie wczytujemy czesci parametrow
        {
            PlayerPrefs.SetInt("GameState", 1);
        }
        else if (gameState == 1)
        {
            PlayerPrefs.SetInt("GameState", 2);
        }    

        currency1 = PlayerPrefs.GetInt("Currency1");        // wczytanie ilosci posiadanej waluty z playerPrefsow
        InGameUI.instance.SetCurr1();
        currency2 = PlayerPrefs.GetInt("Currency2");
        InGameUI.instance.SetCurr2();
        currency3 = PlayerPrefs.GetInt("Currency3");
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

    void OnApplicationQuit()
    {
        PlayerPrefs.DeleteAll();
    }
}