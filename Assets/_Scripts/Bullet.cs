using UnityEngine;
using System.Collections;
using MemoryManagment;

public class Bullet : MonoBehaviour
{
    // Bullets are deactivated before first frame
    void Awake()
    {
        Spawn(Vector2.zero, null);
    }

    // When the bullet leaves the play area, remove from play
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "KillBoundary")
        {
            gameObject.SetActive(false);
        }
    }

    // Reset the bullet, required interface declaration
    public void Spawn(Vector2 spawnPosition, GameObjectPool pool)
    {
        rigidbody2D.mass = transform.localScale.x;
        transform.position = spawnPosition;
        gameObject.SetActive(true);
    }

    // De-activate bullet for later re-use
    void OnDisable()
    {
        BulletController.Instance.StoreBullet(this.gameObject);
        Debug.Log("stored");
    }

    // Change the size and mass of the bullet
    public void ChangeSize(float newSize)
    {
        transform.localScale = new Vector3(newSize, newSize, 1);
        rigidbody2D.mass = transform.localScale.x;
    }
}
