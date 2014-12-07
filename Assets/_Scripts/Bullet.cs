using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour
{
    void Start()
    {
        rigidbody2D.mass = transform.localScale.x;
        Debug.Log(rigidbody2D.mass);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "KillBoundary")
        {
            Destroy(gameObject);
        }
    }
}
