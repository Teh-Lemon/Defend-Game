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

    // Relation of meteor mass to it's size
    [SerializeField]
    float MASS_SCALE_RATIO;
#endregion

    // Current state of meteor
    enum state
    {
        ACTIVE,
        EXPLODING
    }
    state currentState = state.ACTIVE;

    // Attract the meteor to the player when first spawned
    bool attractingToPlayer = false;

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

    void FixedUpdate()
    {
        if (attractingToPlayer)
        {
            /*
            // Push meteor towards player
            // Get the direction
            Vector2 forceToPlayer = PlayerController.Instance.Position;
            forceToPlayer -= new Vector2(transform.position.x, transform.position.y);
            // Get the force
            forceToPlayer = Vector2.Scale(forceToPlayer.normalized, new Vector2(FORCE_TO_PLAYER, 0));
            // Add the force
            GetComponent<Rigidbody2D>().AddForce(forceToPlayer, ForceMode2D.Force);
            //Debug.Log(forceToPlayer);*/
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
                if (other.tag == "KillBoundary")
                {
                    Destroy(gameObject);
                }
                // Play death animation if colliding with a turret
                else if (other.tag == "TurretBody")
                {
                    // Tell the turret it has been hit
                    other.GetComponentInParent<Turret>().HitByMeteor();
                    // Play animation and remove from play
                    StartCoroutine(Explode());
                }
                break;
        }
    }

    /*
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.tag == "Bullet")
        {
            attractingToPlayer = false;
            Debug.Log("Meteor hit by bullet");
        }
    }*/

    // Play death animation
    IEnumerator Explode()
    {
        currentState = state.EXPLODING;

        METEOR_SPRITE.color = new Color(METEOR_SPRITE.color.r,
            METEOR_SPRITE.color.g, METEOR_SPRITE.color.b, DEATH_ALPHA);

        // Start flashing
        StartCoroutine(FlashSprite(METEOR_SPRITE, true,
            DEATH_FLASH_SPEED, DEATH_FLASH_DURATION));
        // Freeze the meteor in position
        yield return new WaitForSeconds(DEATH_FLASH_DURATION);
        // Remove from play after animation is finished
        Destroy(gameObject);
    }

    // Reset the meteor, update it's starting position and size
    // Move towards the player
    public void Spawn(Vector2 newPosition, float newSize)
    {
        currentState = state.ACTIVE;
        gameObject.SetActive(true);
        attractingToPlayer = true;

        // Set up meteor
        UpdateSize(newSize);
        transform.position = newPosition;

        // Push meteor towards player
        // Get the direction
        Vector2 forceToPlayer = PlayerController.Instance.Position;
        forceToPlayer -= new Vector2(transform.position.x, transform.position.y);
        // Get the force
        forceToPlayer = Vector2.Scale(forceToPlayer.normalized, new Vector2(FORCE_TO_PLAYER, 0));
        // Add the force
        GetComponent<Rigidbody2D>().AddForce(forceToPlayer, ForceMode2D.Impulse);
        //Debug.Log(forceToPlayer);
    }
}
