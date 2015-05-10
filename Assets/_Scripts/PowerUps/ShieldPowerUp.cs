using UnityEngine;
using System.Collections;

public class ShieldPowerUp : PowerUp
{
    public override void Activate()
    {
        PlayerController.Instance.RestoreShield();

        base.Activate();
    }
}
