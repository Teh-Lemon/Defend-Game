using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HUD : MonoBehaviour 
{
    public static HUD Instance { get; set; }

    #region Inspector Variables
    [SerializeField] 
    Text AmmoText;
    // Menu Buttons
    [SerializeField] 
    Button RetryButton;
    [SerializeField] 
    Button StartGameButton;
    [SerializeField]
    Button MainMenuButton;
    [SerializeField]
    GameObject Title;

    // Player HUD
    [SerializeField]
    float AMMOBAR_MAX_SCALE;
    [SerializeField]
    GameObject AMMOBAR_OBJECT;
    // Scoring

    // Title screen
    #endregion


    void Awake()
    {
        Instance = this;
    }

    public void UpdateAmmo(float current, float max)
    {
        //AmmoText.text = current.ToString();

        // Find how long the ammo bar should be
        float newScale = AMMOBAR_MAX_SCALE * (current / max);
        // Reverse it since the ammo bar represents the empty part
        newScale = AMMOBAR_MAX_SCALE - newScale;
        // Apply the new scale to the gameobject
        Vector3 oldScale = AMMOBAR_OBJECT.transform.localScale;
        AMMOBAR_OBJECT.transform.localScale = new Vector3(newScale, oldScale.y, oldScale.z);

        //Debug.Log(newScale);
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
            Title.SetActive(true);
            StartGameButton.gameObject.SetActive(true);
        }
        else
        {
            Title.SetActive(false);
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
