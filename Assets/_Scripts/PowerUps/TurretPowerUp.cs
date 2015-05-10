using UnityEngine;
using System.Collections;

public class TurretPowerUp : PowerUp
{
    public override void Activate()
    {
        TurretBotController.Instance.Spawn();

        base.Activate();
    }
}
