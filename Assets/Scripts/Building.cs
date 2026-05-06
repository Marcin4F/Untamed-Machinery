using UnityEngine;

public class Building : MonoBehaviour
{
    [SerializeField] HubUI hubUI;
    private Shop1 shop1;
    private Shop2 shop2;

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
                    hubUI.OpenBuilding(1);
                    shop1 = GetComponent<Shop1>();
                    shop1.Activate();
                    break;
                case "Budynek2":
                    hubUI.OpenBuilding(2);
                    shop2 = GetComponent<Shop2>();
                    shop2.Activate();
                    break;
                case "Budynek3":
                    hubUI.OpenBuilding(3);
                    break;
                default:
                    Debug.LogError("Building tringger enter");
                    break;
            }
            hubUI.EnterTextHide();
        }

        else if (open && inRange && (Input.GetKeyDown(KeyCode.Space)))
        {
            open = false;
            hubUI.CloseBuilding();
            hubUI.EnterTextDisplay();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        inRange = true;
        hubUI.EnterTextDisplay();
    }

    private void OnTriggerExit(Collider other)
    {
        inRange = false;
        open = false;
        hubUI.CloseBuilding();
        hubUI.EnterTextHide();
    }
}
