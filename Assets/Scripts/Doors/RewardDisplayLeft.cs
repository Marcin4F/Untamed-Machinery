using UnityEngine;

public class RewardDisplayLeft : MonoBehaviour
{
    public static RewardDisplayLeft instance;
    private Renderer display;

    [SerializeField] Material curr1, curr2, curr3, healing;
    private void Awake()
    {
        instance = this;
        display = GetComponent<Renderer>();
    }

    public void SetDispaly(int reward)
    {
        switch (reward)
        {
            case 0:
                display.material = healing;
                break;
            case 1:
                display.material = curr1;
                break;
            case 2:
                display.material = curr2;
                break;
            case 3:
                display.material = curr3;
                break;
            default:
                Debug.LogError("RewardDisplayLeft");
                break;
        }
    }
}
