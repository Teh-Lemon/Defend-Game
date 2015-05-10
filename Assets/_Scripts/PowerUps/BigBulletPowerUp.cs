using UnityEngine;
using System.Collections;

public class BigBulletPowerUp : PowerUp
{
    public override void Activate()
    {
        PlayerController.Instance.BigBulletMode();
        base.Activate();
    }
}
