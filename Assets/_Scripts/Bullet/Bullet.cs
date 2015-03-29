using UnityEngine;
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
        transform.position = spawnPosition;
        gameObject.SetActive(true);
    }

    // Change the size and mass of the bullet
    public void ChangeSize(float newSize)
    {
        transform.localScale = new Vector3(newSize, newSize, 1);
        //GetComponent<Rigidbody2D>().mass = transform.localScale.x;
    }
}
