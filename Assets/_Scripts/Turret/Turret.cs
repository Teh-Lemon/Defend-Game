using UnityEngine;
using System.Collections;

public class Turret : CustomBehaviour
{
    #region Inspector Variables
    // Fire rate
    [SerializeField]
    float FireCooldown;
    // Max amount of bullets turret can hold (-1 for infinite)
    [SerializeField]
    public int AmmoCapacity;
    // How fast the turret's ammo recharges
    [SerializeField]
    float AmmoRefillCooldown;
    // Cost of each bullet
    [SerializeField]
    int BulletCost;
    // Cost of big bullet
    [SerializeField]
    int BigBulletCost;
    [SerializeField]
    Vector3 MuzzlePosition;

    // Shield
    [SerializeField]
    SpriteRenderer ShieldSprite;
    public TurretShield Shield;

    // Death animation
    [SerializeField]
    float DEATH_FLASH_DURATION;
    [SerializeField]
    float DEATH_FLASH_SPEED;
    [SerializeField]
    SpriteRenderer TURRET_BODY_SPRITE;
    #endregion

    // Is the cooldown period ready
    bool readyToFire;
    // How much ammo the turret has
    public int AmmoCount { get; set; }
    // Used to signal game over
    public bool IsAlive { get; set; }

    // Use this for initialization
    void Start()
    {
        IsAlive = false;
        //Shield.ToggleShield(false, false);

    }

    // Update is called once per frame
    void Update()
    {

    }

    // Called by the Meteor when colliding with player Body
    public void HitByMeteor()
    {
        //Debug.Log("Hit by meteor");
        if (Shield.IsOn)
        {
            Shield.ToggleShield(false, true);            
        }
        else
        {
            // Die
            StartCoroutine(FlashSprite(TURRET_BODY_SPRITE, true,
                    DEATH_FLASH_SPEED, DEATH_FLASH_DURATION));

            IsAlive = false;
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
        BulletController.Instance.Fire(transform.position + MuzzlePosition, target);

        // Set up cooldown timer
        StartCoroutine(StartFireCooldown());

        // Decrease ammo count
        if (bigBullet)
        {
            UpdateAmmo(-BigBulletCost);
        }
        else
        {
            UpdateAmmo(-BulletCost);
        }
    }

    // Is everything ready to shoot a bullet right now?
    bool CanShootBullet
    {
        get
        {
            // Cooldown period has expired
            // Either Player has enough ammo or ammo isn't required
            if (readyToFire &&
                (AmmoCount >= BulletCost || AmmoCapacity < 0))
            {
                return true;
            }
            else
            {
                return false;
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

        yield return new WaitForSeconds(FireCooldown);

        readyToFire = true;
    }

    // Continously refill the player's ammo as the game is running
    IEnumerator RefillAmmo()
    {
        while (GameStates.Current == GameStates.States.PLAYING)
        {
            yield return new WaitForSeconds(AmmoRefillCooldown);

            UpdateAmmo(1);
        }
    }

    // Update the player's ammo count and the HUD
    void UpdateAmmo(int ammoChange)
    {
        AmmoCount += ammoChange;

        AmmoCount = Mathf.Clamp(AmmoCount, 0, AmmoCapacity);        
    }

    /*
    // Signal game over and play death animation
    void Die()
    {
        
    }*/

    public void Reset()
    {
        IsAlive = true;
        readyToFire = true;
        AmmoCount = 50;
        StartCoroutine(RefillAmmo());
    }
}
