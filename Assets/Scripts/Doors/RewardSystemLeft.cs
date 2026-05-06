using UnityEngine;

public class RewardSystemLeft : MonoBehaviour
{
    public static RewardSystemLeft instance;

    public int rewardIndex;

    void Awake()
    {
        instance = this;
    }

    public void DrawReword()
    {
        rewardIndex = Random.Range(0, 3);
        if (rewardIndex == RewardSystemRight.instance.rewardIndex)
            rewardIndex++;
        RewardDisplayLeft.instance.SetDispaly(rewardIndex);
    }
}
