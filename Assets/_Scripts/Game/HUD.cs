using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HUD : MonoBehaviour 
{
    public static HUD Instance { get; set; }

    [SerializeField] 
    Text AmmoText;
    [SerializeField] 
    Button RetryButton;
    [SerializeField] 
    Button StartGameButton;
    [SerializeField]
    Button MainMenuButton;
    

    void Awake()
    {
        Instance = this;
    }

    public void UpdateAmmo(int current, int max)
    {
        AmmoText.text = current.ToString();
    }

    // Show/Hide the game over buttons
    public void SetUpGameOver(bool entering)
    {
        if (entering)
        {
            //Debug.Log("UI Game Over");
            RetryButton.gameObject.SetActive(true);
            MainMenuButton.gameObject.SetActive(true);
        }
        else
        {
            //Debug.Log("UI Leaving Game Over");
            RetryButton.gameObject.SetActive(false);
            MainMenuButton.gameObject.SetActive(false);
        }
    }

    // Show/Hide the main menu buttons
    public void SetUpMainMenu(bool entering)
    {
        if (entering)
        {
            StartGameButton.gameObject.SetActive(true);
        }
        else
        {
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
