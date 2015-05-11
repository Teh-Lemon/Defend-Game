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
    // How fast to fade from off to on
    [SerializeField]
    float FADE_SPEED;

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
    bool isFading;

    void Awake()
    {
        isFading = false;
    }

    float fadeTimeTotal = 0.0f;
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

        // Fading
        if (isFading)
        {
            fadeTimeTotal += Time.deltaTime;

            // Cap the time
            if (fadeTimeTotal > FADE_SPEED)
            {
                fadeTimeTotal = FADE_SPEED;
            }

            float t = fadeTimeTotal / FADE_SPEED;
            // Ease out
            //t = Mathf.Sin(t * Mathf.PI * 0.5f);
            // Smoothstep http://en.wikipedia.org/wiki/Smoothstep
            //t = t * t * (3f - 2f * t);            
            // Smootherstep
            t = t * t * t * (t * (6f * t - 15f) + 10f);

            // New alpha
            // Fade in
            float newA = Mathf.Lerp(0.0f, 1.0f, t);

            if (SHIELD_SPRITE.color.a >= 1.0f)
            {
                isFading = false;
            }           

            Helper.SetTransparency(SHIELD_SPRITE, newA);
        }
    }

    public void ToggleShield(bool turnOn, bool flash = false)
    {
        if (turnOn)
        {
            // Quick disable/re-enable to stop the flashing coroutine
            if (IsFlashing)
            {
                gameObject.SetActive(false);
                gameObject.SetActive(true);
            }

            // Don't play fade animation if shield is already on
            if (!IsOn)
            {
                Helper.SetTransparency(SHIELD_SPRITE, 0.0f);
                fadeTimeTotal = 0.0f;
                isFading = true;
            }

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
