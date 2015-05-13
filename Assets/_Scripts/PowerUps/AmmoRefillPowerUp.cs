using UnityEngine;
using System.Collections;

public class AmmoRefillPowerUp : PowerUp
{
    [SerializeField]
    ParticleSystem flashEffect;

    public override void Activate()
    {
        PlayerController.Instance.FillUpAmmo();
        flashEffect.Play();
        base.Activate();
    }
}
