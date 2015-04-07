using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour 
{
    public static GameController Instance { get; set; }

    #region Inspector Variables
    // Does the game go on forever despite being hit?
    [SerializeField]
    bool CAN_GAME_OVER;
    // How long after the player dies before the game over screen
    [SerializeField]
    float TIME_TILL_GAMEOVER;

    // How much score is added per increase
    [SerializeField]
    int SCORE_INCREASE_RATE;
    // How fast the score will increment
    [SerializeField]
    float SCORE_INCREASE_INTERVAL;
    #endregion

    // Player's current score
    int score;

	// Use this for initialization
	void Awake() 
	{
        Instance = this;
        score = 0;
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
                if (PlayerController.Instance.HasDied)
                {
                    ChangeState(GameStates.States.GAME_OVER);
                }
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
            MeteorController.Instance.ClearMeteors();
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

            
            PlayerController.Instance.Reset();
            TurretBotController.Instance.Reset();
            BulletController.Instance.Reset();
            MeteorController.Instance.Reset();
            TurretBotController.Instance.Spawn();

            StartCoroutine(StartScoring());
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
                GameStates.Current = GameStates.States.GAME_OVER;

                yield return new WaitForSeconds(TIME_TILL_GAMEOVER);

                HUD.Instance.SetUpGameOver(true, score);

                //BulletController.Instance.Stop();
                //MeteorController.Instance.Stop();

                //Time.timeScale = 0.0f;
            }
        }
        else
        {
            HUD.Instance.SetUpGameOver(false, 1);            
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

    // Start the scoring system, it ends itself on game over
    IEnumerator StartScoring()
    {
        score = 0;
		
		// Prevent the score increasing immediately
		yield return new WaitForSeconds(SCORE_INCREASE_INTERVAL);
		
		// Keep adding score until the player dies
        while (GameStates.Current == GameStates.States.PLAYING)
        {
            score += SCORE_INCREASE_RATE;
            //HUD.Instance.UpdateScore(score);
                        
            yield return new WaitForSeconds(SCORE_INCREASE_INTERVAL);            
        }
    }
}
