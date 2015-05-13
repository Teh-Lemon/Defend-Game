// Adds additional features to the default MonoBehaviour class

using UnityEngine;
using System.Collections;

public class CustomBehaviour : MonoBehaviour
{
    protected bool IsFlashing;

    public CustomBehaviour()
    {
        IsFlashing = false;
    }

    // Flash a sprite on/off at a given speed and duration (duration not accurate)
    public IEnumerator FlashSprite(SpriteRenderer sprite, bool endState, float speed, float duration)
    {
        float startTime = 0f;
        float endTime = 0f;
        IsFlashing = true;

        // Loop the flashing effect for the length of the duration given
        for (float timer = 0; timer < duration; timer += (endTime - startTime))
        {
            startTime = Time.time;
            sprite.enabled = !sprite.enabled;
            yield return new WaitForSeconds(speed);
            endTime = Time.time;
        }

        sprite.enabled = endState;
        IsFlashing = false;
    }

    // Zeros out the object's velocity and angular velocity
    protected IEnumerator StopRigidBody2D(Rigidbody2D rb)
    {
        rb.isKinematic = true;
        yield return new WaitForFixedUpdate();
        rb.isKinematic = false;
    }

    /// <summary>
    /// Calls GetComponent. Use StopRigidBody2D(Rigidbody2D rb) if rigidbody2D is already cached
    /// </summary>
    /// <returns></returns>
    protected IEnumerator StopRigidBody2D()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.isKinematic = true;
        yield return new WaitForFixedUpdate();
        rb.isKinematic = false;
    }

    protected void SetTransparency(SpriteRenderer sprite, float newAlpha)
    {
        sprite.color = new Color(sprite.color.r,
            sprite.color.g, sprite.color.b, newAlpha);
    }   
}