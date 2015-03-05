using UnityEngine;
using System.Collections;

public class TurretBot : MonoBehaviour
{
    public Turret turret;

    // How fast they can fire each bullet
    [SerializeField] 
    float FireRate;
    // Cooldown between each burst of bullets
    [SerializeField]
    float BurstRate;
    // Number of bullets fired per burst
    [SerializeField]
    char BulletsPerBurst;

    void Start()
    {
        turret.Shield.ToggleShield(false);
        //turret.AmmoCapacity = -1;
        //turret.FireCooldown = 0;
    }

    // Update is called once per frame
    void Update()
    {

    }

    // Find target
    void UpdateTarget()
    {

    }
    // Fire burst fire

    public void Reset()
    {
        gameObject.SetActive(false);
    }
}
