using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance { get; set; }


    #region Variables
    // Used to update ammo count

    [SerializeField] 
    Turret turret;
    #endregion

    // Use this for initialization
    void Awake()
    {
        Instance = this;

        // Set the custom mouse cursor
        Texture2D mouseCursor = Resources.Load<Texture2D>("Cursor");
        // Centre it
        Vector2 hotspot = Vector2.one * (mouseCursor.height / 2);
        Cursor.SetCursor(mouseCursor, hotspot, CursorMode.Auto);
    }

    // Update is called once per frame
    void Update()
    {
        switch (GameStates.Current)
        { 
            case GameStates.States.PLAYING:
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

                // Pausing the game
                // Paused button clicked
                if (Input.GetButtonDown("Pause"))
                {
                    GameController.Instance.ChangeState(GameStates.States.PAUSED);
                }

                // Update the player HUD
                HUD.Instance.UpdateAmmo(turret.AmmoCount, turret.AmmoCapacity);
                break;

            case GameStates.States.PAUSED:
                // Paused button clicked
                if (Input.GetButtonDown("Pause"))
                {
                    GameController.Instance.ChangeState(GameStates.States.PLAYING);
                }
                break;
        }


    }

    public void Reset()
    {        
        turret.Shield.ToggleShield(true);
        turret.Reset();
    }

    // Position of the player's main turret
    public Vector2 Position
    {
        get { return turret.gameObject.transform.position; }
    }

    public bool HasDied
    {
        get { return !turret.IsAlive; }
    }
}
