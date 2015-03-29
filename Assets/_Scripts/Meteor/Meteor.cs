using UnityEngine;
using System.Collections;

public class Meteor : CustomBehaviour
{
#region Inspector Variables
    // Death animation
    // Sprite to flash
    [SerializeField]
    SpriteRenderer METEOR_SPRITE;
    // How long the animation lasts
    [SerializeField]
    float DEATH_FLASH_DURATION;
    // How fast the animation flashes
    [SerializeField]
    float DEATH_FLASH_SPEED;
    [SerializeField]
    float DEATH_ALPHA;

    // How strong the meteor is attracted to the player on spawn
    [SerializeField]
    float FORCE_TO_PLAYER;
    [SerializeField]
    float ANGULAR_FORCE_TO_PLAYER;

    // Relation of meteor mass to it's size
    [SerializeField]
    float MASS_SCALE_RATIO;
#endregion

    // Current state of meteor, used to handle behaviour while "dying"
    enum state
    {
        ACTIVE,
        EXPLODING
    }
    state currentState = state.ACTIVE;

    // Is the meteor a big meteor? Affects conditions when to remove from play
    bool type = false;

// Functions
    void Update()
    {
        switch (currentState)
        {
            case state.ACTIVE:
                break;

            case state.EXPLODING:
                // Freeze the meteor in place as the death animation plays
                GetComponent<Rigidbody2D>().velocity = Vector3.zero;
                break;
        }
    }

    // Change the size and mass of the meteor
    void UpdateSize(float newScale)
    {
        transform.localScale = new Vector3(newScale, newScale, 1);
        GetComponent<Rigidbody2D>().mass = newScale * MASS_SCALE_RATIO;
    }

    // Collision events
    void OnTriggerEnter2D(Collider2D other)
    {
        // Only run if the meteor is in play and active
        switch (currentState)
        {
            case state.ACTIVE:
                // Remove meteor from play once it has left the screen
                if (other.CompareTag("KillBoundary"))
                {
                    MeteorController.Instance.StoreMeteor(this.gameObject);
                }
                // Play death animation if colliding with a turret
                else if (other.CompareTag("TurretBody"))
                {
                    // Tell the turret it has been hit
                    other.GetComponentInParent<Turret>().HitByMeteor();
                    // Play animation and remove from play
                    StartCoroutine(Explode());
                }
                break;
        }
    }    

    // Play death animation
    IEnumerator Explode()
    {
        currentState = state.EXPLODING;

        SetTransparency(DEATH_ALPHA);

        // Start flashing
        StartCoroutine(FlashSprite(METEOR_SPRITE, true,
            DEATH_FLASH_SPEED, DEATH_FLASH_DURATION));
        // Freeze the meteor in position
        yield return new WaitForSeconds(DEATH_FLASH_DURATION);
        // Remove from play after animation is finished
        MeteorController.Instance.StoreMeteor(this.gameObject);
    }

    // Reset the meteor, update it's starting position and size
    // Move towards the player
    public GameObject Spawn(Vector2 newPosition, float newSize, bool isBig)
    {
        currentState = state.ACTIVE;
        gameObject.SetActive(true);

        // Set up meteor
        UpdateSize(newSize);
        transform.position = newPosition;
        SetTransparency(1.0f);
        type = isBig;

        // Push meteor towards player
        // Get the direction
        Vector2 forceToPlayer = PlayerController.Instance.Position;
        forceToPlayer -= new Vector2(transform.position.x, transform.position.y);
        // Get the force
        forceToPlayer = Vector2.Scale(forceToPlayer.normalized, new Vector2(FORCE_TO_PLAYER, 0));
        // Add the force
        GetComponent<Rigidbody2D>().AddForce(forceToPlayer, ForceMode2D.Impulse);
        //Debug.Log(forceToPlayer);

        // Rotate the meteor towards the player
        if (transform.position.x > 0)
        {
            GetComponent<Rigidbody2D>().AddTorque(ANGULAR_FORCE_TO_PLAYER, ForceMode2D.Impulse);
        }
        else
        {
            GetComponent<Rigidbody2D>().AddTorque(-ANGULAR_FORCE_TO_PLAYER, ForceMode2D.Impulse);
        }

        return this.gameObject;
    }

    // Change the transparency of the meteor sprite
    void SetTransparency(float newTrans)
    {
            METEOR_SPRITE.color = new Color(METEOR_SPRITE.color.r,
        METEOR_SPRITE.color.g, METEOR_SPRITE.color.b, newTrans);        
    }
}
