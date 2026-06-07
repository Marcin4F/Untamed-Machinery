using UnityEngine;

public class Boss : MonoBehaviour
{
    private void OnDestroy()
    {
        int rewardAmount;
        switch (GameManagement.instance.rewardIndex)
        {
            case 0:
                rewardAmount = Random.Range(-Player.instance.minHealing, -Player.instance.maxHealing);
                Player.instance.TakeDamage(rewardAmount);
                break;
            case 1:
                rewardAmount = Random.Range(Player.instance.minReward, Player.instance.maxReward) * 10;
                GameManagement.instance.currency1 += rewardAmount;
                InGameUI.instance.SetCurr1();
                break;
            case 2:
                rewardAmount = Random.Range(Player.instance.minReward, Player.instance.maxReward) * 10;
                GameManagement.instance.currency2 += rewardAmount;
                InGameUI.instance.SetCurr2();
                break;
            case 3:
                rewardAmount = Random.Range(Player.instance.minReward, Player.instance.maxReward) * 10;
                GameManagement.instance.currency3 += rewardAmount;
                InGameUI.instance.SetCurr3();
                break;
            default:
                Debug.LogError("Reward number exceeded. Around line 57 in 'RewardSystemR'");
                break;
        }

        InGameUI.instance.GameWon();
    }
}
