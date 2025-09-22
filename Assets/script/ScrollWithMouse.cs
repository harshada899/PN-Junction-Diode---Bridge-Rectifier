using UnityEngine;
using UnityEngine.UI;

public class ScrollWithMouse : MonoBehaviour
{
    public ScrollRect scrollRect;
    public float scrollSpeed = 0.1f; // Adjust for sensitivity

    void Update()
    {
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");

        if (scrollInput != 0f)
        {
            // Clamp the value between 0 and 1
            float newPos = Mathf.Clamp01(scrollRect.verticalNormalizedPosition + scrollInput * scrollSpeed);

            scrollRect.verticalNormalizedPosition = newPos;
        }
    }
}
