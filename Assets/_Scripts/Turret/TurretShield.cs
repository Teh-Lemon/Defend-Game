using UnityEngine;

public class TurretShield : CustomBehaviour
{
    [SerializeField] 
    SpriteRenderer SHIELD_SPRITE;
    [SerializeField]
    float SHIELD_FLASH_DURATION;
    [SerializeField]
    float SHIELD_FLASH_SPEED;

    // Is the shield activated
    public bool IsOn { get; set; }

    // Use this for initialization
    void Start()
    {
        //ToggleShield(true);
    }

    public void ToggleShield(bool turnOn, bool flash = false)
    {
        if (turnOn)
        {
            IsOn = true;
            SHIELD_SPRITE.enabled = true;
            //Debug.Log("foo " + this.gameObject);
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
            //Debug.Log("boo");
        }
    }
}
