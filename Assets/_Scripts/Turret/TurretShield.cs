using UnityEngine;

public class TurretShield : CustomBehaviour
{
    #region Inspector variables
    // Sprite used
    [SerializeField] 
    SpriteRenderer SHIELD_SPRITE;
    // Death animation
    // How long the death animation plays for 
    [SerializeField]
    float SHIELD_FLASH_DURATION;
    // How fast the shield flashes while dying
    [SerializeField]
    float SHIELD_FLASH_SPEED;

    // How fast the shield pulses
    [SerializeField]
    float PULSE_RATE;
    // How small/big the shield pulses
    [SerializeField]
    float PULSE_SCALE_GROWTH;
    // Default size
    [SerializeField]
    float DEFAULT_SCALE;
    #endregion

    // Is the shield activated
    public bool IsOn { get; set; }

    void Update()
    {
        // Pulse shield
        // Take the default size, scale it up and down using a sine graph
        // Move along the sine graph using the current time
        // Scale the time with the pulse rate to change the speed of the pulsing
        // Multiply with the scale growth to affect how large the pulse is
        float newScale = (DEFAULT_SCALE + 
            (Mathf.Sin(Time.time * PULSE_RATE) * PULSE_SCALE_GROWTH));

        // Possibly need to make sure z doesn't change if bugs with sprite
        // ordering occur
        transform.localScale = Vector3.one * newScale;
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
                StartCoroutine(FlashSprite(SHIELD_SPRITE, false,
                    SHIELD_FLASH_SPEED, SHIELD_FLASH_DURATION));
            }

            SHIELD_SPRITE.enabled = false;
        }
    }
}
