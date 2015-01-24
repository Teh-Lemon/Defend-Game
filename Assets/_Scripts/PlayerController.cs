using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    #region Variables
    // Used to update ammo count
    public static PlayerController Instance { get; set; }

    [SerializeField] Turret turret;
    #endregion

    // Use this for initialization
    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        turret.Shield.ToggleShield(true);
    }

    // Update is called once per frame
    void Update()
    {
        // Fire a bullet at the crosshair
        // If the player clicks with the mouse
        if (Input.GetButton("Fire1"))
        {
            // Mouse position in the world space
            Vector2 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            turret.ShootBullet(worldPos);
        }
        // or touchs their phone screen
        else if (Input.touchSupported)
        {
            if (Input.touchCount > 0)
            {
                Vector2 worldPos = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);

                turret.ShootBullet(worldPos);
            }
        }
    }
}
