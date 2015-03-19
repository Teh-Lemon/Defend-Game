using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour 
{
    public static GameController Instance { get; set; }

    // How long after the player dies before the game over screen
    [SerializeField]
    float TIME_TILL_GAMEOVER;

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
            case GameStates.States.MENU:
                StartGame();
                break;

            case GameStates.States.PLAYING:
                break;

            case GameStates.States.GAME_OVER:
                if (Input.GetKeyDown(KeyCode.R))
                {
                    StartMenu();
                }
                break;

            default:
                break;
        }
	}

    void StartMenu()
    {
        GameStates.Current = GameStates.States.MENU;
    }

    void StartGame()
    {
        GameStates.Current = GameStates.States.PLAYING;

        Debug.Log("Resetting!");
        PlayerController.Instance.Reset();
        TurretBotController.Instance.Reset();
        BulletController.Instance.Reset();
        MeteorController.Instance.Reset();
        
        Time.timeScale = 1.0f;

        //System.GC.Collect();
    }

    public IEnumerator StartGameOver()
    {
        Debug.Log("Game over in " + TIME_TILL_GAMEOVER);
        yield return new WaitForSeconds(TIME_TILL_GAMEOVER);

        Debug.Log("Game over'd");
        GameStates.Current = GameStates.States.GAME_OVER;
        Time.timeScale = 0.0f;
    }
}
