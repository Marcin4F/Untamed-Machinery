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
        if (GameManagement.instance.cleared)
        {
            int index = SceneManager.GetActiveScene().buildIndex;
            Player.instance.SaveInfo();

            int roomsCleared = PlayerPrefs.GetInt("RoomsCleared", 0);
            roomsCleared++;
            PlayerPrefs.SetInt("RoomsCleared", roomsCleared);

            // poziom trudnoœci zwiêkszany co 5 pokoi
            int difficultyTier = roomsCleared / 5;
            int minIndex, maxIndex;

            // TODO: po pewnej ilosci pokoi ma byc pokoj z bossem
            // ---------------- UWAGA przy zmianie ilosci pokoi latwych trzeba zmienic wartosci w GameStarter ----------------
            // ---------------- TODO: przeniesc zaczynanie gry do tego skryptu ----------------
            switch (difficultyTier)
            {
                case 0: // pokoje numer 1-5 latwe
                    minIndex = 2;
                    maxIndex = 7; // losuje indeksy od 2 do 6
                    break;
                case 1: // pokoje numer 6-10 srednie
                    minIndex = 7;
                    maxIndex = 13; // losuje indeksy od 7 do 12
                    break;
                case 2: // pokoje numer 11-15 trudne
                    minIndex = 13;
                    maxIndex = 19; // losuje indeksy od 13 do 18
                    break;
                default: // pokoje numer 16+ bardzo trudne
                    minIndex = 19;
                    maxIndex = 26; // losuje indeksy od 19 do 25
                    break;
            }

            int newIndex = index;

            // TODO: limit by dany pokoj nie pojawial sie tak czesto
            while (index == newIndex)
            {
                newIndex = Random.Range(minIndex, maxIndex);
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
