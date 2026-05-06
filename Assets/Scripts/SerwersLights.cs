using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class SerwersLights : MonoBehaviour
{
    private Light redLight;

    private bool state = true, change = true;

    void Start()
    {
        redLight = GetComponent<Light>();
    }

    private void Update()
    {
        if (change)
        {
            change = false;
            state = !state;
            redLight.enabled = state;
            StartCoroutine(WaitTime());
        }
        
    }

    IEnumerator WaitTime()
    {
        yield return new WaitForSeconds(1);
        change = true;
    }
}
