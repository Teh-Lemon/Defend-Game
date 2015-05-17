using UnityEngine;
using System.Collections;

public class PowerUpController : MonoBehaviour
{
    public static PowerUpController Instance { get; set; }

    // Is there a power up on screen
    bool PowerUpActive;

    // Activation sound effect
    AudioSource activateAudioSource;

    #region Inspector Variables
    // List of all the power up game objects
    [SerializeField]
    GameObject[] PowerUpGOs;

    [Header("Spawning")]
    // Where the power up will spawn
    [SerializeField]
    float MinSpawnHeight;
    [SerializeField]
    float MaxSpawnHeight;
    // How far from the centre of the screen the power up spawns
    [SerializeField]
    float SpawnX;
    // How often the spawn diceroll is made
    [SerializeField]
    float DiceRollInterval;
    // Chance of spawning power up per diceroll
    [SerializeField]
    float DiceRollChance;

    [Header("Properties")]
    // How fast the power up moves across the screen
    [SerializeField]
    float MoveSpeed;
    // How fast the power up moves up and down
    [SerializeField]
    float FloatSpeed;
    // How far the power up moves up and down
    [SerializeField]
    float FloatDeviation;
    #endregion

    void Awake()
    {
        Instance = this;
        PowerUpActive = false;
    }

    // Use this for initialization
    void Start()
    {
        // Set up power ups
        for (int i = 0; i < PowerUpGOs.Length; i++)
        {
            PowerUpGOs[i].GetComponent<PowerUp>().SetUp(MoveSpeed, FloatSpeed, FloatDeviation);
        }

        activateAudioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        // If a power up is active on the field
        if (PowerUpActive)
        {
            PowerUpActive = false;

            // Check when it stops being active
            for (int i = 0; i < PowerUpGOs.Length; i++)
            {
                if (PowerUpGOs[i].activeInHierarchy)
                {
                    PowerUpActive = true;
                    break;
                }
            }
        }
    }

    // Spawn power ups while the game is playing
    IEnumerator SpawnNewPowerUp()
    {
        // Keep running while game is playing
        while (GameController.Instance.InGame)
        {
            yield return new WaitForSeconds(DiceRollInterval);

            // Don't spawn anything if there's already a powerup on the field
            // Don't spawn if diceroll fails
            if (Random.value > DiceRollChance || PowerUpActive)
            {
                continue;
            }

            // Randomise which power up
            int newPUp = Random.Range(0, PowerUpGOs.Length);
            // DEBUG ONLY
            if (Debug.isDebugBuild)
            {
                //newPUp = 3;
            }

            // Set spawn position
            float spawnY = Random.Range(MinSpawnHeight, MaxSpawnHeight);
            float spawnX = SpawnX * Helper.RandomSign();

            // Spawn power up
            PowerUpActive = true;
            PowerUpGOs[newPUp].transform.position = new Vector3(spawnX, spawnY, 1.0f);            
            PowerUpGOs[newPUp].SetActive(true);            
        }
    }

    
    public void Reset()
    {
        Clear();
        StartCoroutine(SpawnNewPowerUp());
    }

    public void Clear()
    {
        // Extra precaution for stopping spawning coroutines
        gameObject.SetActive(false);
        gameObject.SetActive(true);

        // Remove all active power ups from play
        for (int i = 0; i < PowerUpGOs.Length; i++)
        {
            PowerUpGOs[i].SetActive(false);
        }
    }

    /*
    public void PlayActivateSound()
    {
        activateAudioSource.Play();
    }*/
}
