using UnityEngine;
using System.Collections;

public class Turret : MonoBehaviour
{
    // Fire rate
    public float FireCooldown;
    // Max amount of bullets turret can hold (-1 for infinite)
    public int AmmoCapacity;
    // How fast the turret's ammo recharges
    public float AmmoRefillCooldown;
    // Cost of each bullet
    public int BulletCost;
    // Cost of big bullet
    public int BigBulletCost;

    // Shield
    public SpriteRenderer ShieldSprite;
    public TurretShield Shield;

    // Is the cooldown period ready
    bool readyToFire;
    // How much ammo the turret has
    int ammoCount;
    // Used to signal game over
    bool isAlive;

    // Use this for initialization
    void Start()
    {
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
                (ammoCount >= BulletCost || AmmoCapacity < 0))
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
        ammoCount += ammoChange;

        ammoCount = Mathf.Clamp(ammoCount, 0, AmmoCapacity);

        HUD.Instance.UpdateAmmo(ammoCount, AmmoCapacity);
    }

    // Signal game over and play death animation
    void Die()
    {
        isAlive = false;
    }
}
