using UnityEngine;

public class Building : MonoBehaviour
{
    private string buildingName;
    private int buildingIndex;

    private void Start()
    {
        buildingName = gameObject.name;
        if (buildingName == "Budynek1")
            buildingIndex = 1;
        else
            buildingIndex = 2;
    }

    private void OnTriggerEnter(Collider other)
    {
        InGameUI.instance.EnterPanelDisplay();
        InGameUI.instance.shopIndex = buildingIndex;
    }

    private void OnTriggerExit(Collider other)
    {
        InGameUI.instance.CloseShop();
        InGameUI.instance.EnterPanelHide();
    }
}
