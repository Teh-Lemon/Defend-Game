using UnityEngine;
using System.Collections;

public class AmmoRefillPowerUp : PowerUp
{
    public override void Activate()
    {
        PlayerController.Instance.RefillAmmo();
        base.Activate();
    }
}
