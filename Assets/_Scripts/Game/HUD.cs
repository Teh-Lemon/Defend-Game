using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HUD : MonoBehaviour 
{
    public static HUD Instance { get; set; }

    Sprite[] digitsFont;

    #region Inspector Variables
    // Displays the debug score
    [SerializeField] 
    Text ScoreText;

    [Header("Main Menu")]
    // Title screen title image
    [SerializeField]
    GameObject TitleGO;
    [SerializeField]
    Button MainMenuButton;

    [Header("Playing")]
    [SerializeField]
    float AMMOBAR_MAX_SCALE;
    [SerializeField]
    GameObject AmmoBarGO;

    [Header("Game Over")]
    // Menu Buttons
    [SerializeField]
    Button RetryButton;
    [SerializeField]
    Button StartGameButton;
    // Score message, both parts. "You have survived for" "seconds"
    [SerializeField]
    GameObject ScoreMsg1GO;
    [SerializeField]
    GameObject ScoreMsg2GO;

//    [Header("Other")] 

    #endregion


    void Awake()
    {
        Instance = this;

        //digitsFont = Resources.LoadAll<Sprite>("digitSheet");
    }

    // Update the size of the ammo bar
    public void UpdateAmmo(float current, float max)
    {
        //AmmoText.text = current.ToString();

        // Find how long the ammo bar should be
        float newScale = AMMOBAR_MAX_SCALE * (current / max);
        // Reverse it since the ammo bar represents the empty part
        //newScale = AMMOBAR_MAX_SCALE - newScale;
        // Apply the new scale to the gameobject
        Vector3 oldScale = AmmoBarGO.transform.localScale;
        AmmoBarGO.transform.localScale = new Vector3(newScale, oldScale.y, oldScale.z);

        //Debug.Log(newScale);
    }

    // Update the score display
    public void UpdateScore(int newScore)
    {       
        ScoreText.text = newScore.ToString();
    }

    // Show/Hide the game over buttons
    public void SetUpGameOver(bool entering)
    {
        if (entering)
        {         
            RetryButton.gameObject.SetActive(true);
            MainMenuButton.gameObject.SetActive(true);

            // Score message
            ScoreMsg1GO.SetActive(true);
            ScoreMsg2GO.SetActive(true);
        }
        else
        {
            //Debug.Log("UI Leaving Game Over");
            RetryButton.gameObject.SetActive(false);
            MainMenuButton.gameObject.SetActive(false);

            // Score message
            ScoreMsg1GO.SetActive(false);
            ScoreMsg2GO.SetActive(false);
        }
    }

    // Show/Hide the main menu buttons
    public void SetUpMainMenu(bool entering)
    {
        if (entering)
        {
            TitleGO.SetActive(true);
            StartGameButton.gameObject.SetActive(true);
        }
        else
        {
            TitleGO.SetActive(false);
            StartGameButton.gameObject.SetActive(false);
        }
    }


    #region Button event handlers
    // Start Game button event handler
    public void ClickStartGameButton()
    {
            GameController.Instance.ChangeState(GameStates.States.PLAYING);        
    }

    // Main menu button event handler
    public void ClickMainMenuButton()
    {
            GameController.Instance.ChangeState(GameStates.States.MENU);        
    }

    // Restart button event handler
    public void ClickRetryButton()
    {
            GameController.Instance.ChangeState(GameStates.States.PLAYING);
    }
    #endregion
}
