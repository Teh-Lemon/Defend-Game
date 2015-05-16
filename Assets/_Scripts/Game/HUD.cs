using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using MemoryManagment;

public class HUD : MonoBehaviour 
{
    public static HUD Instance { get; set; }

    // Holds the sprites used for each character when displaying the score
    Sprite[] digitsFont;
    // Index of the space character in the digits font sheet
    const int FONT_SPACE_INDEX = 10;
    List<GameObject> activeDigits;
    // In Options menu?

    // Used to display the score number at the end
    GameObjectPool digitsPool;

    #region Inspector Variables

    [Header("Main Menu")]
    // Title screen title image
    [SerializeField]
    GameObject TitleGO;
    // Paused image
    [SerializeField]
    GameObject PausedGO;
    // Buttons
    [SerializeField]
    Button OptionsButton;
    [SerializeField]
    Button StartGameButton;
    [SerializeField]
    Button QuitButton;

    [Header("Options")]
    [SerializeField]
    GameObject VolumeGO;
    [SerializeField]
    Slider VolumeSlider;
    [SerializeField]
    Button OptionsBackButton;

    [Header("Playing")]
    // Scale of the ammo bar meter when it's at 100%
    [SerializeField]
    float AMMOBAR_MAX_SCALE;
    // Ammo bar meter to scale as the ammo count changes
    [SerializeField]
    GameObject AmmoBarGO;

    [Header("Game Over")]
    // Buttons
    [SerializeField]
    Button RetryButton;
    [SerializeField]
    Button MainMenuButton;
    // Score message, both parts. "You have survived for" 
    [SerializeField]
    GameObject ScoreMsg1GO;
    // "seconds"
    [SerializeField]
    GameObject ScoreMsg2GO;

    [Header("Other")]
    // Prefab to spawn the digits in the score from
    [SerializeField]
    GameObject DIGIT_PREFAB;
    // Displays the debug score
    [SerializeField]
    Text ScoreText;
    [SerializeField]
    AudioSource AudioSelect;

    #endregion


    void Awake()
    {
        Instance = this;
        digitsFont = Resources.LoadAll<Sprite>("DigitsSheet");
        activeDigits = new List<GameObject>();
        digitsPool = new GameObjectPool(3, DIGIT_PREFAB, ScoreMsg1GO);

        //Adds a listener to the main slider and invokes a method when the value changes.
        VolumeSlider.onValueChanged.AddListener(delegate { VolumeSliderChanged(); });
    }

    // Update the size of the ammo bar
    public void UpdateAmmo(float current, float max)
    {
        // Find how long the ammo bar should be
        float newScale = AMMOBAR_MAX_SCALE * (current / max);
        // Apply the new scale to the gameobject
        Vector3 oldScale = AmmoBarGO.transform.localScale;
        AmmoBarGO.transform.localScale = 
            new Vector3(newScale, oldScale.y, oldScale.z);
    }

    // Update the score display
    public void UpdateScore(int newScore)
    {       
        ScoreText.text = newScore.ToString();
    }

    // Sets up the score message on the game over screen
    void DisplayEndScore(int score)
    {
        // Length of the whole score message
        float totalWidth = 0.0f;

        // Size of the first part of the message
        float ScoreMsg1Size = ScoreMsg1GO
            .GetComponent<SpriteRenderer>().bounds.size.x;
        // Width of the space
        float SpaceWidth = digitsFont[FONT_SPACE_INDEX].bounds.size.x
            / ScoreMsg1GO.transform.localScale.x;

        // Where the first space after the message starts
        float scoreStartX = (ScoreMsg1Size / ScoreMsg1GO.transform.localScale.x)
            + SpaceWidth;

        // Digits
        // The player's final score, each digit separated into an individual
        List<int> scoreSeparated = Helper.SeparateDigits(score);
        // Create the sprites to display the final score
        for (int i = 0; i < scoreSeparated.Count; i++)
        {
            // Grab a new digit sprite to use
            activeDigits.Add(digitsPool.New());
            // Set its sprite to the right number
            activeDigits[i].GetComponent<SpriteRenderer>().sprite = 
                digitsFont[scoreSeparated[i]];

            // Width of the digit
            totalWidth += activeDigits[i].GetComponent<SpriteRenderer>().bounds.size.x;

            // Set it's position
            // If the first digit, set it after the space
            if (i == 0)
            {
                activeDigits[i].transform.localPosition =
                    new Vector3(scoreStartX, 0.0f, 0.0f);
            }
            // Else set it after the previous digit
            else
            {
                // Length of the previous digit
                float lastWidth = activeDigits[i - 1]
                    .GetComponent<SpriteRenderer>().bounds.size.x;
                // Move to the end of the previous digit
                activeDigits[i].transform.localPosition =
                    new Vector3(activeDigits[i - 1].transform.localPosition.x 
                        + lastWidth, 0.0f, 0.0f);
            }
            
            activeDigits[i].SetActive(true);
        }

        // Add the 2nd half of the message
        // Start at the previous digit, move to its end
        float msg2StartX = activeDigits[activeDigits.Count - 1]
            .transform.localPosition.x;
        // Move to the end of the digit
        msg2StartX += activeDigits[activeDigits.Count - 1]
            .GetComponent<SpriteRenderer>().bounds.size.x;
        // Add the space
        msg2StartX += SpaceWidth;      
        // Finally, move the 2nd half of the message here
        ScoreMsg2GO.transform.localPosition = new Vector3(msg2StartX, 0.0f, 0.0f);        

        // Centre the whole line
        totalWidth += ScoreMsg1Size + (2 * SpaceWidth) + 
            ScoreMsg2GO.GetComponent<SpriteRenderer>().bounds.size.x;
        ScoreMsg1GO.transform.position = 
            new Vector3(0 - (totalWidth / 2), ScoreMsg1GO.transform.position.y
                , ScoreMsg1GO.transform.position.z);
    }

    // Event handler for options button and options back button
    public void ShowOptionsMenu(bool entering)
    {
        if (entering)
        {
            // Hide main menu
            StartGameButton.gameObject.SetActive(false);
            OptionsButton.gameObject.SetActive(false);            

            // Show options menu
            VolumeGO.gameObject.SetActive(true);
            VolumeSlider.gameObject.SetActive(true);
            OptionsBackButton.gameObject.SetActive(true);

            VolumeSlider.value = AudioListener.volume;
        }
        else
        {
            // Hide options menu
            VolumeGO.gameObject.SetActive(false);
            VolumeSlider.gameObject.SetActive(false);
            OptionsBackButton.gameObject.SetActive(false);

            // Show main menu
            StartGameButton.gameObject.SetActive(true);
            OptionsButton.gameObject.SetActive(true);
        }
    }

    // Update the volume as the volume slider is changed
    void VolumeSliderChanged()
    {
        AudioListener.volume = VolumeSlider.value;
    }

    #region states
    // Show/Hide the game over buttons
    public void SetUpGameOver(bool entering, int score)
    {
        if (entering)
        {         
            RetryButton.gameObject.SetActive(true);
            MainMenuButton.gameObject.SetActive(true);

            // Score message
            ScoreMsg1GO.SetActive(true);
            //ScoreMsg2GO.SetActive(true);

            DisplayEndScore(score);
        }
        else
        {
            //Debug.Log("UI Leaving Game Over");
            RetryButton.gameObject.SetActive(false);
            MainMenuButton.gameObject.SetActive(false);

            // Score message
            ScoreMsg1GO.SetActive(false);
            //ScoreMsg2GO.SetActive(false);

            for (int i = 0; i < activeDigits.Count; i++)
            {
                digitsPool.Store(activeDigits[i]);
            }
            activeDigits.Clear();
        }
    }

    // Show/Hide the main menu buttons
    public void SetUpMainMenu(bool entering)
    {
        if (entering)
        {
            TitleGO.SetActive(true);
            StartGameButton.gameObject.SetActive(true);
            OptionsButton.gameObject.SetActive(true);

#if UNITY_STANDALONE_WIN
            QuitButton.gameObject.SetActive(true);
#endif
        }
        else
        {
            TitleGO.SetActive(false);
            StartGameButton.gameObject.SetActive(false);
            OptionsButton.gameObject.SetActive(false);
#if UNITY_STANDALONE_WIN
            QuitButton.gameObject.SetActive(false);
#endif
        }
    }

    // Show/Hide pause indicator
    public void SetUpPaused(bool entering)
    {
        if (entering)
        {
            PausedGO.SetActive(true);
        }
        else
        {
            PausedGO.SetActive(false);
        }
    }
    #endregion


    #region Button event handlers
    // Start Game button event handler
    public void ClickStartGameButton()
    {
            GameController.Instance.ChangeState(GameStates.States.GAME_START);
            AudioSelect.Play();
    }

    // Main menu button event handler
    public void ClickMainMenuButton()
    {
            GameController.Instance.ChangeState(GameStates.States.MENU);
            AudioSelect.Play();
    }

    // Restart button event handler
    public void ClickRetryButton()
    {
            GameController.Instance.ChangeState(GameStates.States.GAME_START);
            AudioSelect.Play();
    }

    // Options button on main menu
    public void ClickOptionsButton()
    {
        AudioSelect.Play();
        ShowOptionsMenu(true);
    }

    // Back to main menu button in options menu
    public void ClickOptionsBackButton()
    {
        AudioSelect.Play();
        ShowOptionsMenu(false);
    }

    // Quit button on the main menu
    public void ClickQuitButton()
    {
        Application.Quit();
    }
    #endregion
}
