using UnityEngine;
using System.Collections;

public class PlayerShield : MonoBehaviour
{
    public SpriteRenderer SHIELD_SPRITE;
    public float SHIELD_FLASH_DURATION;
    public float SHIELD_FLASH_SPEED;

    // Is the shield activated
    public bool IsOn { get; set; }

    // Use this for initialization
    void Start()
    {
        ToggleShield(true);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ToggleShield(bool turnOn, bool flash = false)
    {
        if (turnOn)
        {
            IsOn = true;
            SHIELD_SPRITE.enabled = true;
        }
        else
        {
            IsOn = false;

            if (flash)
            {
                StartCoroutine(FlashSprite(SHIELD_SPRITE, false, SHIELD_FLASH_SPEED, SHIELD_FLASH_DURATION));
            }

            SHIELD_SPRITE.enabled = false;
        }
    }

    // Flash a sprite on/off at a given speed and duration (duration not accurate)
    IEnumerator FlashSprite(SpriteRenderer sprite, bool endState, float speed, float duration)
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
