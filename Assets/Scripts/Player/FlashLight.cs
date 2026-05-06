using UnityEngine;

public class FlashLight : MonoBehaviour
{
    private Light flashLight;
    
    private bool state = false;
    
    void Start()
    {
        flashLight = GetComponent<Light>();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.L))
        {
            state = !state;
            flashLight.enabled = state;
        }
    }
}
