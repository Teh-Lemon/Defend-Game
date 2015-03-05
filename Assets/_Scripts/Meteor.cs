using UnityEngine;
using System.Collections;

public class Meteor : CustomBehaviour
{
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

    // Relation of meteor mass to it's size
    [SerializeField]
    float MASS_SCALE_RATIO;

    // Current state of meteor
    enum state
    {
        ACTIVE,
        EXPLODING
    }
    state currentState = state.ACTIVE;

    void Update()
    {
        switch (currentState)
        {
            case state.ACTIVE:
                break;

            case state.EXPLODING:
                GetComponent<Rigidbody2D>().velocity = Vector3.zero;
                break;
        }
    }

    void UpdateSize(float newScale)
    {
        transform.localScale = new Vector3(newScale, newScale, 1);
        GetComponent<Rigidbody2D>().mass = newScale * MASS_SCALE_RATIO;
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
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

    }

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

    public void Reset()
    {
        currentState = state.ACTIVE;
    }
}
