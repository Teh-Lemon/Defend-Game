using UnityEngine;
using System.Collections;

public class PlayerShield : CustomBehaviour
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
}
