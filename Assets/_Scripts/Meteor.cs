using UnityEngine;
using System.Collections;

public class Meteor : MonoBehaviour
{
    // Relation of meteor mass to it's size
    public float MASS_SCALE_RATIO;

    void UpdateSize(float newScale)
    {
        transform.localScale = new Vector3(newScale, newScale, 1);
        rigidbody2D.mass = newScale * MASS_SCALE_RATIO;
    }

    
    void OnTriggerEnter2D(Collider2D other)
    {
        // Remove meteor from play once it has left the screen
        if (other.tag == "KillBoundary")
        {
            Destroy(gameObject);
        }            
        // Play death animation if colliding with a turret
        else if (other.tag == "Turret")
        {
            other.GetComponentInParent<Turret>().HitByMeteor();
            Explode();
        }
    }

    // Play death animation (unfinished)
    void Explode()
    {
        Destroy(gameObject);
    }
}
