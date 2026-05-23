using UnityEngine;

public class Building : MonoBehaviour
{
    private string buildingIndex;
    private bool inRange = false, open = false;

    private void Start()
    {
        buildingIndex = gameObject.name;
    }

    private void Update()
    {
        if (inRange && !open && Input.GetKeyDown(KeyCode.Space))        // otwieranie menu budynku
        {
            open = true;
            switch (buildingIndex)
            {
                case "Budynek1":
                    InGameUI.instance.OpenBuilding(1);
                    Shop1 shop1 = GetComponent<Shop1>();
                    shop1.Activate();
                    break;
                case "Budynek2":
                    InGameUI.instance.OpenBuilding(2);
                    Shop2 shop2 = GetComponent<Shop2>();
                    shop2.Activate();
                    break;
                case "Budynek3":
                    InGameUI.instance.OpenBuilding(3);
                    break;
                default:
                    Debug.LogError("Building tringger enter");
                    break;
            }
            InGameUI.instance.EnterTextHide();
        }

        else if (open && inRange && (Input.GetKeyDown(KeyCode.Space)))
        {
            open = false;
            InGameUI.instance.CloseBuilding();
            InGameUI.instance.EnterTextDisplay();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        inRange = true;
        InGameUI.instance.EnterTextDisplay();
    }

    private void OnTriggerExit(Collider other)
    {
        inRange = false;
        open = false;
        InGameUI.instance.CloseBuilding();
        InGameUI.instance.EnterTextHide();
    }
}
