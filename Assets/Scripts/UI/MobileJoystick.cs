using UnityEngine;
using UnityEngine.EventSystems;

public class MobileJoystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    private RectTransform background;
    private RectTransform handle;
    private Vector2 inputVector;

    public float Horizontal { get { return inputVector.x; } }
    public float Vertical { get { return inputVector.y; } }

    private void Start()
    {
        background = GetComponent<RectTransform>();
        handle = transform.GetChild(0).GetComponent<RectTransform>();
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 position;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(background, eventData.position, eventData.pressEventCamera, out position))
        {
            // Factor in the RectTransform's pivot so the visual center is ALWAYS treated as (0,0)
            position.x = (position.x / background.sizeDelta.x) + background.pivot.x - 0.5f;
            position.y = (position.y / background.sizeDelta.y) + background.pivot.y - 0.5f;

            // Multiply by 2 to map the -0.5 to 0.5 range to a clean -1 to 1 range for movement
            inputVector = new Vector2(position.x * 2, position.y * 2);
            
            // Clamp the magnitude so the handle doesn't get dragged outside the background circle
            inputVector = (inputVector.magnitude > 1.0f) ? inputVector.normalized : inputVector;

            // Move the handle UI physically
            handle.anchoredPosition = new Vector2(
                inputVector.x * (background.sizeDelta.x / 2), 
                inputVector.y * (background.sizeDelta.y / 2)
            );
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // Trigger the drag calculation the exact moment the finger touches the screen
        OnDrag(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // Snap the handle back to the center and stop moving the player when the finger lifts
        inputVector = Vector2.zero;
        handle.anchoredPosition = Vector2.zero;
    }
}