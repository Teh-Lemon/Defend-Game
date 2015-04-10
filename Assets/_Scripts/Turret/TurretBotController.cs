using UnityEngine;
using System.Collections.Generic;

public class TurretBotController : MonoBehaviour 
{
    [SerializeField]
    TurretBot[] turretBots;

    public static TurretBotController Instance {get;set;}

    void Awake()
    {
        Instance = this;
    }

	// Use this for initialization
	void Start () 
	{
        Reset();
	}

    // Find and spawn the first inactive turret bot
    public void Spawn()
    {        
        for (int i = 0; i < turretBots.Length; i++)
        {
            if (!turretBots[i].gameObject.activeInHierarchy)
            {
                turretBots[i].Spawn();
            }
        }
    }

    public void Reset()
    {
        for (int i = 0; i < turretBots.Length; i++)
        {
            turretBots[i].Disable();
        }
    }
}
