using UnityEngine;

public class RewardSystemRight : MonoBehaviour
{
    public static RewardSystemRight instance;

    public int rewardIndex;
    private int currentReward, rewardAmount;

    void Start()
    {
        instance = this;

        EnemyPool.roomCleared += GetReward;

        rewardIndex = Random.Range(0, 4);       // losowanie indeksu nagrody (UWAGA: zwiekszyc zakres przy dodawaniu kolejnych nagrod)
        RewardDisplayRight.instance.SetDispaly(rewardIndex);
        RewardSystemLeft.instance.DrawReword();
    }

    private void GetReward()
    {
        if (GameManagement.instance.gameState > 0)
        {
            if (GameManagement.instance.gameState == 1)
            {
                currentReward = Random.Range(1, 4);
                GameManagement.instance.gameState = 2;
            }
            else currentReward = GameManagement.instance.rewardIndex;

            switch (currentReward)
            {
                case 0:
                    rewardAmount = Random.Range(-Player.instance.minHealing, -Player.instance.maxHealing);
                    Player.instance.TakeDamage(rewardAmount);
                    break;
                case 1:
                    rewardAmount = Random.Range(Player.instance.minReward, Player.instance.maxReward);
                    GameManagement.instance.currency1 += rewardAmount;
                    InGameUI.instance.SetCurr1();
                    break;
                case 2:
                    rewardAmount = Random.Range(Player.instance.minReward, Player.instance.maxReward);
                    GameManagement.instance.currency2 += rewardAmount;
                    InGameUI.instance.SetCurr2();
                    break;
                case 3:
                    rewardAmount = Random.Range(Player.instance.minReward, Player.instance.maxReward);
                    GameManagement.instance.currency3 += rewardAmount;
                    InGameUI.instance.SetCurr3();
                    break;
                default:
                    Debug.LogError("Reward number exceeded. Around line 57 in 'RewardSystemR'");
                    break;
            }
        }
    }

    private void OnDestroy()
    {
        EnemyPool.roomCleared -= GetReward;
    }
}
