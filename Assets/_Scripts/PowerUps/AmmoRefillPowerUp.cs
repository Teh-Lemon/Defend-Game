using UnityEngine;
using System.Collections;

public class AmmoRefillPowerUp : PowerUp
{
    public override void Activate()
    {
        PlayerController.Instance.FillUpAmmo();
        base.Activate();
    }
}
