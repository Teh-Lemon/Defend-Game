using UnityEngine;
using System.Collections;

public class Meteor : CustomBehaviour
{
#region Inspector Variables
    // Death animation
    // Sprite to flash
    [SerializeField]
    SpriteRenderer Sprite;
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
        SETUP,
        ACTIVE,
        EXPLODING,
        STORED
    }
    state currentState = state.STORED;

    // Is the meteor a big meteor? Affects conditions when to remove from play

    GameStates.MeteorTypes type = GameStates.MeteorTypes.NORMAL;

    Rigidbody2D rb;

    // Functions
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

// Functions
    void Update()
    {
        switch (currentState)
        {
            case state.ACTIVE:                
                break;

            case state.EXPLODING:
                // Freeze the meteor in place as the death animation plays
                rb.velocity = Vector3.zero;
                break;
        }
    }

    // Change the size and mass of the meteor
    void UpdateSize(float newScale)
    {
        transform.localScale = new Vector3(newScale, newScale, 1);
        rb.mass = newScale * MASS_SCALE_RATIO;
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
                    StartCoroutine(RemoveFromPlay());
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

        SetTransparency(Sprite, DEATH_ALPHA);

        // Start flashing
        StartCoroutine(FlashSprite(Sprite, true,
            DEATH_FLASH_SPEED, DEATH_FLASH_DURATION));
        // Freeze the meteor in position
        yield return new WaitForSeconds(DEATH_FLASH_DURATION);
        // Remove from play after animation is finished
        //Debug.Log("Exploding");
        StartCoroutine(RemoveFromPlay());
    }

    IEnumerator RemoveFromPlay()
    {
        if (currentState != state.STORED)
        {
            //Debug.Log("Removing from play");
            currentState = state.STORED;
            // Reset the meteors angular velocity and velocity
            rb.isKinematic = true;
            yield return new WaitForFixedUpdate();
            rb.isKinematic = false;

            MeteorController.Instance.StoreMeteor(this.gameObject);
        }
    }

    // Reset the meteor, update it's starting position and size
    // Move towards the player
    public GameObject Spawn(Vector2 newPosition, float newSize, GameStates.MeteorTypes newType)
    {
        currentState = state.SETUP;

        // Set up meteor
        transform.position = newPosition;
        UpdateSize(newSize);
        Sprite.enabled = true;
        SetTransparency(Sprite, 1.0f);
        type = newType;
        

        gameObject.SetActive(true);        

        // Push meteor towards player
        // Get the direction
        Vector2 forceToPlayer = PlayerController.Instance.Position;
        forceToPlayer -= new Vector2(transform.position.x, transform.position.y);
        // Get the force
        // Direction horizontally * (Force, adjusted with meteor mass)
        forceToPlayer = Vector2.Scale(forceToPlayer.normalized
            , new Vector2(FORCE_TO_PLAYER * rb.mass, 0));
        // Add the force
        rb.AddForce(forceToPlayer, ForceMode2D.Impulse);

        // Rotate the meteor towards the player
        if (transform.position.x > 0)
        {
            rb.AddTorque(ANGULAR_FORCE_TO_PLAYER, ForceMode2D.Impulse);
        }
        else
        {
            rb.AddTorque(-ANGULAR_FORCE_TO_PLAYER, ForceMode2D.Impulse);
        }

        currentState = state.ACTIVE;

        return this.gameObject;
    }

    /*
    // Change the transparency of the meteor sprite
    void SetTransparency(float newTrans)
    {
            Sprite.color = new Color(Sprite.color.r,
        Sprite.color.g, Sprite.color.b, newTrans);         
    }
     * */
}
