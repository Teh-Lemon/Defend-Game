using UnityEngine;
using System.Collections;
using MemoryManagment;

public class Bullet : MonoBehaviour
{
    // Bullet stores itself back into the pool for later re-use on deactivation
    public GameObjectPool ownPool;

    // Bullets are deactivated before first frame
    void Awake()
    {
        Reset(Vector2.zero, null);
    }

    // When the bullet leaves the play area, remove from play
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "KillBoundary")
        {
            Disable();
        }
    }

    // Reset the bullet, required interface declaration
    public void Reset(Vector2 spawnPosition, GameObjectPool pool)
    {
        rigidbody2D.mass = transform.localScale.x;
        transform.position = spawnPosition;
        ownPool = pool;
        gameObject.SetActive(true);
    }

    // De-activate bullet for later re-use
    void Disable()
    {
        if (ownPool != null)
        {
            ownPool.Store(this.gameObject);
        }
    }

    // Change the size and mass of the bullet
    public void ChangeSize(float newSize)
    {
        transform.localScale = new Vector3(newSize, newSize, 1);
        rigidbody2D.mass = transform.localScale.x;
    }
}
