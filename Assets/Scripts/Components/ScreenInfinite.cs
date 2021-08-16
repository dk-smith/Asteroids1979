using UnityEngine;

// Реализация зацикливания экрана. 
namespace Components
{
    public class ScreenInfinite : MonoBehaviour
    {

        void FixedUpdate()
        {
            Vector2 screenPosition = Camera.main.WorldToScreenPoint(transform.position);
            bool change = false;
            if (screenPosition.x < 0 || screenPosition.x > Screen.width)
            {
                screenPosition.x = Mathf.Max(screenPosition.x + 1, Screen.width) - Mathf.Min(screenPosition.x + 1, Screen.width);
                change = true;
            }
            if (screenPosition.y < 0 || screenPosition.y > Screen.height)
            {
                screenPosition.y = Mathf.Max(screenPosition.y + 1, Screen.height) - Mathf.Min(screenPosition.y + 1, Screen.height);
                change = true;
            }
            if (change)
            {
                screenPosition = Camera.main.ScreenToWorldPoint(screenPosition);
                transform.position = screenPosition;
            }
        }
    }
}
