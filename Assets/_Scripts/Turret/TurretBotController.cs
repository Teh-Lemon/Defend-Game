using UnityEngine;
using System.Collections.Generic;

public class TurretBotController : MonoBehaviour 
{
    [SerializeField]
    List<TurretBot> TurretBots;

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
	
	// Update is called once per frame
	void Update () 
	{
	
	}

    public void Reset()
    {
        for (int i = 0; i < TurretBots.Count; i++)
        {
            TurretBots[i].Reset();
        }
    }
}
