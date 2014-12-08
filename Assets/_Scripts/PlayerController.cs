﻿using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    #region Variables
    // Fire rate
    public float FIRE_COOLDOWN;
    // Max amount of bullets player can hold
    public int AMMO_CAPACITY;
    // How fast the player's ammo recharges
    public float AMMO_REFILL_COOLDOWN;
    // Cost of each bullet
    public int BULLET_COST;
    // Cost of big bullet
    public int BIG_BULLET_COST;
    // Bullet movement speed
    public float BULLET_SPEED;

    // Bullet prefab
    public GameObject BULLET_PREFAB;
    // Used to update ammo count
    public HUD hud;
    // Used to find out current game state
    public GameController gameC;
    public SpriteRenderer SHIELD_SPRITE;

    // Is the cooldown period ready
    bool readyToFire;
    // How much ammo the player has
    int ammoCount;
    //public int AmmoCount { get { return ammoCount; } }    
    // Whether the shield is alive
    bool shieldOn;
    // Used to signal game over
    bool isAlive;
    #endregion

    // Use this for initialization
    void Start()
    {
        readyToFire = true;
        ammoCount = 50;
        ToggleShield(true);
        isAlive = true;
        StartCoroutine(RefillAmmo());
    }

    // Update is called once per frame
    void Update()
    {
        //  Fire a bullet at the crosshair
        if (Input.GetButton("Fire1") && CanShootBullet())
        {
            // Mouse position in the world space
            Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            ShootBullet(mouseWorldPos);
        }
    }

    // Detect collision with meteors
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Meteor")
        {
            if (shieldOn)
            {
                ToggleShield(false);
            }
            else
            {
                Die();
            }
        }
    }

    // Shoot a bullet from the player position to the target
    void ShootBullet(Vector2 target, bool bigBullet = false)
    {
        Vector2 direction = target - new Vector2(transform.position.x, transform.position.y);

        // Spawn and fire the bullet
        GameObject newBullet = Instantiate(BULLET_PREFAB, transform.position, Quaternion.identity) as GameObject;
        newBullet.rigidbody2D.velocity = direction.normalized * BULLET_SPEED;

        // Set up cooldown timer
        StartCoroutine(StartCooldown());

        // Decrease ammo count
        if (bigBullet)
        {
            UpdateAmmo(-BIG_BULLET_COST);
        }
        else
        {
            UpdateAmmo(-BULLET_COST);
        }
    }

    // Is everything ready to shoot a bullet right now?
    bool CanShootBullet(bool bigBullet = false)
    {
        // Cooldown period has expired
        // Player has the ammo
        if (!readyToFire || ammoCount < BULLET_COST)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    // Set timer before next bullet can be fired
    IEnumerator StartCooldown()
    {
        readyToFire = false;

        yield return new WaitForSeconds(FIRE_COOLDOWN);

        readyToFire = true;
    }

    // Continously refill the player's ammo as the game is running
    IEnumerator RefillAmmo()
    {
        while (GameStates.Current == GameStates.States.PLAYING)
        {
            yield return new WaitForSeconds(AMMO_REFILL_COOLDOWN);

            UpdateAmmo(1);
        }
    }

    // Update the player's ammo count and the HUD
    void UpdateAmmo(int ammoChange)
    {
        ammoCount += ammoChange;

        ammoCount = Mathf.Clamp(ammoCount, 0, AMMO_CAPACITY);

        hud.UpdateAmmo(ammoCount, AMMO_CAPACITY);
    }


    // Turn the shield on or off
    void ToggleShield(bool turnOn)
    {
        if (turnOn)
        {
            shieldOn = true;
            SHIELD_SPRITE.enabled = true;
        }
        else
        {
            shieldOn = false;
            SHIELD_SPRITE.enabled = false;
        }
    }

    // Signal game over and play death animation
    void Die()
    {
        isAlive = false;
    }
}