using UnityEngine;
using System.Collections;

public class Turret : CustomBehaviour
{
    #region Inspector Variables
    // Fire rate
    [SerializeField]
    public float FireCooldown;
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
    //[SerializeField]
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
    [SerializeField]
    SpriteRenderer TURRET_MUZZLE_SPRITE;
    [SerializeField]
    CircleCollider2D BodyCollider;
    [SerializeField]
    AudioSource HitAudio;

    // Big Bullet Mode
    // Muzzle overlay activated when in big bullet mode
    [SerializeField]
    SpriteRenderer MuzzleFlashSpr;
    #endregion

    // Is the cooldown period ready
    bool readyToFire;
    // How much ammo the turret has
    public int AmmoCount { get; set; }
    // Used to signal game over
    public bool IsAlive { get; set; }
    // Is the turret firing big bullets
    bool bigBullet;
    public bool BigBullet
    {
        get { return bigBullet; }
        set
        {
            bigBullet = value;
            MuzzleFlashSpr.enabled = value;
        }
    }

    CameraScript mainCamera;


    void Awake()
    {
        IsAlive = false;
        bigBullet = false;
    }

    // Use this for initialization
    void Start()
    {        
        mainCamera = Camera.main.GetComponent<CameraScript>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    // Called by the Meteor when colliding with player Body
    public void HitByMeteor()
    {
        HitAudio.Play();

        StartCoroutine(mainCamera.Shake());
        //mainCamera.ResetPosition();

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

    public void ShootBullets(Vector2 target, int totalBullets)
    {
        ShootBullet(target);
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
        //BulletController.Instance.Fire(transform.position + MuzzlePosition, target);
        BulletController.Instance.Fire(MuzzlePosition, target, BigBullet);

        // Set up cooldown timer
        StartCoroutine(StartFireCooldown());

        // Decrease ammo count
        UpdateAmmo(-BulletCost);
        
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
        if (AmmoCapacity > 0)
        {
            while (GameController.Instance.InGame)
            {
                yield return new WaitForSeconds(AmmoRefillCooldown);

                UpdateAmmo(1);
            }
        }
    }

    // Update the player's ammo count and the HUD
    void UpdateAmmo(int ammoChange)
    {
        AmmoCount += ammoChange;

        AmmoCount = Mathf.Clamp(AmmoCount, 0, AmmoCapacity);        
    }

    public void Reset()
    {
        IsAlive = true;
        readyToFire = true;
        AmmoCount = AmmoCapacity / 2;
        //MuzzlePosition
        MuzzlePosition = transform.FindChild("Muzzle").position;
        ToggleVisibility(true);
        StartCoroutine(RefillAmmo());
        MuzzleFlashSpr.enabled = false;
        bigBullet = false;
    }

    public float GetHeight()
    {
        return TURRET_BODY_SPRITE.bounds.size.y;
    }

    public void ToggleVisibility(bool on)
    {
        if (on)
        {
            TURRET_BODY_SPRITE.enabled = true;
            TURRET_MUZZLE_SPRITE.enabled = true;
        }
        else
        {
            TURRET_BODY_SPRITE.enabled = false;
            TURRET_MUZZLE_SPRITE.enabled = false;;
        }
    }

    public CircleCollider2D GetCollider()
    {
        return BodyCollider;
    }

    public IEnumerator FlashMuzzle(float length, float speed)
    {
        float totalTime = 0.0f;
        float startTime = 0.0f;

        // Flash
        while (totalTime < length)
        {
            startTime = Time.time;

            MuzzleFlashSpr.enabled = !MuzzleFlashSpr.enabled;
            Debug.Log("flash " + MuzzleFlashSpr.enabled);

            yield return new WaitForSeconds(speed);            
            // Keep track for how long the flashing has been running
            totalTime += (Time.time - startTime);
            Debug.Log("total time " + totalTime);
        }

        if (!bigBullet)
        {
            MuzzleFlashSpr.enabled = false;
        }
    }
}
