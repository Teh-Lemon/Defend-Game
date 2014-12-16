using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour 
{
    #region Variables
    public static GameController Instance { get; set; }
    #endregion

	// Use this for initialization
	void Awake() 
	{
        Instance = this;
	}
	
	// Update is called once per frame
	void Update() 
	{
        switch (GameStates.Current)
        {
            case GameStates.States.PLAYING:
                break;

            default:
                break;
        }
	}
}
