using UnityEngine;
using System.Collections;
using MemoryManagment; // ObjectPool

public class BulletController : MonoBehaviour
{
    public static BulletController Instance { get; set; }

    // Bullet movement speed
    public float BULLET_SPEED;
    // Bullet prefab
    public GameObject BULLET_PREFAB;

    GameObjectPool bulletPool;

    // Use this for initialization
    void Awake()
    {
        Instance = this;
        bulletPool = new GameObjectPool(100, BULLET_PREFAB, gameObject);
    }

    public void Fire(Vector2 spawnPos, Vector2 target)
    {
        // Recycle from an old bullet
        GameObject bulletGO = bulletPool.New();
        if (bulletGO != null)
        {
            // Reset the bullet variables
            var bulletScript = bulletGO.GetComponent<Bullet>();
            bulletScript.Spawn(spawnPos, bulletPool);

            // Fire the bullet towards the target
            Vector2 direction = target - new Vector2(spawnPos.x, spawnPos.y);
            bulletGO.rigidbody2D.velocity = direction.normalized * BULLET_SPEED;
        }
        else
        {
            Debug.Log("Bullet pool returned null bulletGO");
        }
    }

    // Place bullet back into pool for later re-use
    // Called by the bullet when it deactivates itself
    public void StoreBullet(GameObject bullet)
    {
        if (bulletPool != null)
        {
            bulletPool.Store(bullet);
        }
    }
}
