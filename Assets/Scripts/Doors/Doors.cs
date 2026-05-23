using UnityEngine;
using UnityEngine.SceneManagement;

public class Doors : MonoBehaviour
{
    private int active;

    // tymczasowe pokazanie otwarte/zamkniete
    RewardSystemLeft rewardSystemLeft;
    RewardSystemRight rewardSystemRight;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Wejscie w kolizje");
        if (GameManagement.instance.cleared)
        {
            Debug.Log("if cleared");
            int index = SceneManager.GetActiveScene().buildIndex;
            GameManagement.instance.roomsCleared++;

            // poziom trudnoœci zwiêkszany co 5 pokoi
            int difficultyTier = GameManagement.instance.roomsCleared / 5;
            int minIndex, maxIndex;

            // TODO: po pewnej ilosci pokoi ma byc pokoj z bossem
            // ---------------- UWAGA przy zmianie ilosci pokoi latwych trzeba zmienic wartosci w GameStarter ----------------
            // ---------------- TODO: przeniesc zaczynanie gry do tego skryptu ----------------
            switch (difficultyTier)
            {
                case 0: // pokoje numer 1-5 latwe
                    minIndex = 2;
                    maxIndex = 8; // losuje indeksy od 2 do 7
                    break;
                case 1: // pokoje numer 6-10 srednie
                    minIndex = 8;
                    maxIndex = 14; // losuje indeksy od 8 do 13
                    break;
                case 2: // pokoje numer 11-15 trudne
                    minIndex = 14;
                    maxIndex = 20; // losuje indeksy od 14 do 19
                    break;
                default: // pokoje numer 16+ bardzo trudne
                    minIndex = 20;
                    maxIndex = 26; // losuje indeksy od 20 do 25
                    break;
            }

            int newIndex = index;
            Debug.Log("Stary indeks: " + index);
            // TODO: limit by dany pokoj nie pojawial sie tak czesto
            while (index == newIndex)
            {
                newIndex = Random.Range(minIndex, maxIndex);
            }
            Debug.Log("Nowy indeks: " + newIndex);
            if (active == 0)
                GameManagement.instance.rewardIndex = rewardSystemLeft.rewardIndex;
            else
                GameManagement.instance.rewardIndex = rewardSystemRight.rewardIndex;
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
