using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour 
{
    public static GameController Instance { get; set; }

    // Does the game go on forever despite being hit?
    [SerializeField]
    bool CAN_GAME_OVER;
    // How long after the player dies before the game over screen
    [SerializeField]
    float TIME_TILL_GAMEOVER;

	// Use this for initialization
	void Awake() 
	{
        Instance = this;
	}

    void Start()
    {
        ChangeState(GameStates.States.MENU);
    }
	
	// Update is called once per frame
	void Update() 
	{
        switch (GameStates.Current)
        {
            case GameStates.States.MENU:
                //ChangeState(GameStates.States.PLAYING);
                break;

            case GameStates.States.PLAYING:
                break;

            case GameStates.States.GAME_OVER:
                if (Input.GetKeyDown(KeyCode.R))
                {
                    ChangeState(GameStates.States.MENU);
                }
                break;

            default:
                break;
        }
	}

    void ChangeMenuState(bool entering)
    {
        if (entering)
        {
            GameStates.Current = GameStates.States.MENU;
            HUD.Instance.SetUpMainMenu(true);
        }
        else
        {
            HUD.Instance.SetUpMainMenu(false);
        }
    }

    public void ChangePlayingState(bool entering)
    {
        if (entering)
        {
            GameStates.Current = GameStates.States.PLAYING;

            Debug.Log("Resetting!");
            PlayerController.Instance.Reset();
            TurretBotController.Instance.Reset();
            BulletController.Instance.Reset();
            MeteorController.Instance.Reset();
        }
        //Time.timeScale = 1.0f;

        //System.GC.Collect();
    }

    public IEnumerator ChangeGameOverState(bool entering)
    {
        if (entering)
        {
            if (CAN_GAME_OVER)
            {
                yield return new WaitForSeconds(TIME_TILL_GAMEOVER);

                Debug.Log("Game over'd");
                GameStates.Current = GameStates.States.GAME_OVER;
                HUD.Instance.SetUpGameOver(true);

                //BulletController.Instance.Stop();
                //MeteorController.Instance.Stop();

                //Time.timeScale = 0.0f;
            }
        }
        else
        {
            HUD.Instance.SetUpGameOver(false);
        }
    }

    // Handles leaving the current state and changing into the new one
    public void ChangeState(GameStates.States newState)
    {
        // Don't do anything if the newState is a duplicate
        if (GameStates.Current == newState)
        {
            return;
        }

        // Clean up current state
        switch (GameStates.Current)
        {
            case GameStates.States.MENU:
                ChangeMenuState(false);
                break;

            case GameStates.States.GAME_OVER:
                StartCoroutine(ChangeGameOverState(false));
                break;
        }

        // Enter new state
        switch (newState)
        {
            case GameStates.States.MENU:
                ChangeMenuState(true);
                break;

            case GameStates.States.PLAYING:
                ChangePlayingState(true);
                break;

            case GameStates.States.GAME_OVER:
                StartCoroutine(ChangeGameOverState(true));
                break;
        }
    }
}
