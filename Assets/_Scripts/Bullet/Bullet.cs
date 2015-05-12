using UnityEngine;
using MemoryManagment;

public class Bullet : MonoBehaviour
{
    Rigidbody2D bulletRB;

    float defaultScale;
    float defaultMass;

    // Bullets are deactivated before first frame
    void Awake()
    {
        bulletRB = GetComponent<Rigidbody2D>();

        //Spawn(Vector2.zero, null);
        
        defaultScale = transform.localScale.x;
        defaultMass = bulletRB.mass;
    }

    void FixedUpdate()
    {
        bulletRB.velocity =
            bulletRB.velocity.normalized * BulletController.Instance.BULLET_SPEED;            
    }

    // When the bullet leaves the play area, remove from play
    void OnTriggerEnter2D(Collider2D other)
    {
        if (this.enabled)
        {
            if (other.CompareTag("KillBoundary"))
            {
                BulletController.Instance.StoreBullet(this.gameObject);
            }
        }
    }

    // Reset the bullet, required interface declaration
    public void Spawn(Vector2 spawnPosition, GameObjectPool pool)
    {
        //GetComponent<Rigidbody2D>().mass = transform.localScale.x;
        ChangeSize(defaultScale, defaultMass);
        transform.position = spawnPosition;        
        gameObject.SetActive(true);        
    }

    // Change the size and mass of the bullet
    public void ChangeSize(float newSize, float newMass)
    {
        transform.localScale = new Vector3(newSize, newSize, 1);
        bulletRB.mass = newMass;
        //GetComponent<Rigidbody2D>().mass = transform.localScale.x;
    }
}
