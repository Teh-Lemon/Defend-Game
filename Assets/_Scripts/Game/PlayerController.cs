﻿using UnityEngine;

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
        //Reset();
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

                // Update the player HUD
                HUD.Instance.UpdateAmmo(turret.AmmoCount, turret.AmmoCapacity);

                // Game Over when the player's main turret dies
                if (!turret.IsAlive)
                {
                    GameController.Instance.ChangeState(GameStates.States.GAME_OVER);
                    //Debug.Log("turret died");
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
}