using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance { get; set; }

    #region Variables
    // Used for big bullet mode power up    
    float bigBulletTimer;
    // Is the warning currently playing
    bool bigBulletWarnOn;

    [SerializeField] 
    Turret turret;

    // How long the big bullet power up lasts
    [SerializeField]
    float BigBulletLength;
    // How much time is left when the warning is triggered
    [SerializeField]
    float BigBulletWarning;
    // Warning flash speed
    [SerializeField]
    float BigBulletFlashSpeed;
    #endregion

    // Use this for initialization
    void Awake()
    {
        Instance = this;

#if !UNITY_ANDROID
        // Set the custom mouse cursor
        Texture2D mouseCursor = Resources.Load<Texture2D>("Cursor");
        // Centre it
        Vector2 hotspot = Vector2.one * (mouseCursor.height / 2);
        Cursor.SetCursor(mouseCursor, hotspot, CursorMode.Auto);        
#endif
    }

    // Update is called once per frame
    void Update()
    {
        switch (GameStates.Current)
        {
            case GameStates.States.MENU:
                if (Input.GetButton("Cancel"))
                {
                    HUD.Instance.ShowOptionsMenu(false);
                }
                break;

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

                // Disable big bullet after the power up expires
                if (turret.BigBullet)
                {
                    bigBulletTimer += Time.deltaTime;

                    // Start warning the player 1 sec before the power up expires
                    if (bigBulletTimer > (BigBulletLength - BigBulletWarning)
                        && !bigBulletWarnOn)
                    {
                        bigBulletWarnOn = true;
                        StartCoroutine(turret.FlashMuzzle(BigBulletWarning, BigBulletFlashSpeed));
                    }

                    // Disable big bullet mode when timer is up
                    if (bigBulletTimer > BigBulletLength)
                    {
                        //Debug.Log("BB OFF");
                        turret.BigBullet = false;
                    }
                }
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

    // Reset the turret to default spawned state
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

    // Has the player's turret died
    public bool HasDied
    {
        get { return !turret.IsAlive; }
    }

    // Re-activate the player's shield
    public void RestoreShield()
    {
        turret.Shield.ToggleShield(true);
    }

    // Set the player's current ammo to full
    public void FillUpAmmo()
    {
        turret.AmmoCount = turret.AmmoCapacity;
    }

    // Turns the big bullet mode on/off
    public void BigBulletMode(bool on = true)
    {
        if (on)
        {
            bigBulletTimer = 0;            
            turret.BigBullet = true;
            bigBulletWarnOn = false;
        }
        else
        {            
            turret.BigBullet = false;
            bigBulletWarnOn = false;
        }
    }

    // Is the player's shield currently active
    public bool HasShield
    {
        get { return turret.Shield.IsOn; }
    }
}
