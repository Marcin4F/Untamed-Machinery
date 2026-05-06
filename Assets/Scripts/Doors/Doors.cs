using UnityEngine;
using UnityEngine.SceneManagement;

public class Doors : MonoBehaviour
{
    private int index, newIndex, active;

    // tymczasowe pokazanie otwarte/zamkniete
    RewardSystemLeft rewardSystemLeft;
    RewardSystemRight rewardSystemRight;

    private void OnTriggerEnter(Collider other)
    {
        if (GameManagement.instance.cleared)
        {
            index = SceneManager.GetActiveScene().buildIndex;
            newIndex = index;
            Player.instance.SaveInfo();

            // DO DODANIA: limit by dany pokoj nie pojawial sie tak czesto
            while (index == newIndex)
            {
                newIndex = Random.Range(2, 26);      // ustawic odpowiednie indeksy scen (pierwsza liczba to najmniejszy indeks sceny z pokojem, druga to najwiekszy + 1)
            }
            if (active == 0)
                PlayerPrefs.SetInt("RewardIndex", rewardSystemLeft.rewardIndex);
            else
                PlayerPrefs.SetInt("RewardIndex", rewardSystemRight.rewardIndex);
            SceneManager.LoadScene(newIndex);
        }
    }

    private void Start()
    {
        rewardSystemRight = GetComponent<RewardSystemRight>();
        rewardSystemLeft = GetComponent<RewardSystemLeft>();

        if (rewardSystemLeft != null)
            active = 0;

        if (rewardSystemRight != null)
            active = 1;
    }
}
