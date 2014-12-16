using UnityEngine;
using System.Collections;

public class PlayerInput : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {        
        // Fire a bullet at the crosshair
        // If the player clicks with the mouse or touchs their phone screen
        if (Input.GetButton("Fire1"))
        {
            // Mouse position in the world space
            Vector2 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            PlayerController.Instance.ShootBullet(worldPos);
        }
        else if (Input.touchSupported)
        {
            if (Input.touchCount > 0)
            {
                Vector2 worldPos = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);

                PlayerController.Instance.ShootBullet(worldPos);
            }
        }
    }
}
