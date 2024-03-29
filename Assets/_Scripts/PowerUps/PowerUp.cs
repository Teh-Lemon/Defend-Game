﻿using UnityEngine;
using System.Collections;

public class PowerUp : MonoBehaviour
{
    // How fast the power up moves across the screen
    float moveSpeed = 0.0f;
    // How fast the power up moves up and down
    float floatSpeed = 0.0f;
    // How far the power up moves up and down
    float floatDeviation = 0.0f;

    // Which direction to move in, set on spawn
    int direction = 0;

    // Update is called once per frame
    void Update()
    {
        // Float the power up across the stage
        float velX = moveSpeed * direction * Time.deltaTime;
        float velY = (
            (Mathf.Sin(Time.time * floatSpeed) * floatDeviation));

        transform.Translate(velX, velY, 0.0f);
    }

    // Collision Handling
    void OnTriggerEnter2D(Collider2D other)
    {
        if (this.enabled)
        {
            // If hit by bullet, remove bullet and activate power up
            if (other.CompareTag("Bullet"))
            {
                BulletController.Instance.StoreBullet(other.gameObject);
                Activate();
            }
            // If left the stage, disable power up
            else if (other.CompareTag("PUpBoundary"))
            {
                gameObject.SetActive(false);
            }
        }
    }

    // Remove the power up and turn on it's power
    public virtual void Activate()
    {
        //PowerUpController.Instance.PlayActivateSound();
        AudioController.Instance.Play(AudioController.Sounds.POWERUP);
        gameObject.SetActive(false);
    }

    // Assign the movement properties
    public void SetUp(float MoveSpeed, float FloatSpeed, float FloatDeviation)
    {
        moveSpeed = MoveSpeed;
        floatSpeed = FloatSpeed;
        floatDeviation = FloatDeviation;
    }

    // Set the direction on spawn
    void OnEnable()
    {        
        if (transform.position.x > 0)
        {
            direction = -1;
        }
        else
        {
            direction = 1;
        }
    }
}
