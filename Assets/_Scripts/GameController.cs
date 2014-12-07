using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour 
{
    #region Variables
    public GameStates.States CurrentState = GameStates.States.PLAYING;

    // UI
    public HUD hud;

    // Current player object
    PlayerController playerController;
    #endregion

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
        switch (CurrentState)
        {
            case GameStates.States.PLAYING:
                //hud.UpdateAmmo(playerController.AmmoCount, playerController.AMMO_CAPACITY);
                break;

            default:
                break;
        }
	}
}
