using UnityEngine;
using System.Collections;

public class TurretBot : MonoBehaviour 
{
    public Turret turret;

	// Use this for initialization
	void Start () 
	{
        turret.Shield.ToggleShield(false);
        turret.AmmoCapacity = -1;
        turret.FireCooldown = 0;
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

    // Find target

    // Fire burst fire
}
