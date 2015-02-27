// Adds additional features to the default MonoBehaviour class

using UnityEngine;
using System.Collections;

public class CustomBehaviour : MonoBehaviour
{
    // Flash a sprite on/off at a given speed and duration (duration not accurate)
    public IEnumerator FlashSprite(SpriteRenderer sprite, bool endState, float speed, float duration)
    {
        float startTime = 0f;
        float endTime = 0f;

        // Loop the flashing effect for the length of the duration given
        for (float timer = 0; timer < duration; timer += (endTime - startTime))
        {
            startTime = Time.time;
            sprite.enabled = !sprite.enabled;
            yield return new WaitForSeconds(speed);
            endTime = Time.time;
        }

        sprite.enabled = endState;
    }
}
