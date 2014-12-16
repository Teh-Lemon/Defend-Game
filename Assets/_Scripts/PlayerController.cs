using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    #region Variables
    // Used to update ammo count
    public static PlayerController Instance { get; set; }

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
    
    //public HUD hud;
    public SpriteRenderer SHIELD_SPRITE;
    //public BulletController bulletC;
    public PlayerShield Shield;

    // Is the cooldown period ready
    bool readyToFire;
    // How much ammo the player has
    int ammoCount;
    // Used to signal game over
    bool isAlive;
    #endregion

    // Use this for initialization
    void Awake()
    {
        Instance = this;
        readyToFire = true;
        ammoCount = 50;
        isAlive = true;
        StartCoroutine(RefillAmmo());
    }

    // Update is called once per frame
    void Update()
    {

    }

    // Called by the Meteor when colliding with player Body
    public void HitByMeteor()
    {
        if (Shield.IsOn)
        {
            Shield.ToggleShield(false, true);
        }
        else
        {
            Die();
        }
    }

    // Shoot a bullet from the player position to the target
    public void ShootBullet(Vector2 target, bool bigBullet = false)
    {
        // Only fire if ready to do so
        if (!CanShootBullet)
        {
            return;
        }

        // Shoot a bullet out from the muzzle at the target
        BulletController.Instance.Fire(transform.position, target);

        // Set up cooldown timer
        StartCoroutine(StartFireCooldown());

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
    bool CanShootBullet
    {
        get
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
    }

    // Not implemented
    bool CanShootBigBullet
    {
        get { return false; }
    }

    // Set timer before next bullet can be fired
    IEnumerator StartFireCooldown()
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

        HUD.Instance.UpdateAmmo(ammoCount, AMMO_CAPACITY);
    }

    // Signal game over and play death animation
    void Die()
    {
        isAlive = false;
    }
}
