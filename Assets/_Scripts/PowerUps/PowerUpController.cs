using UnityEngine;
using System.Collections;

public class PowerUpController : MonoBehaviour
{
    public static PowerUpController Instance { get; set; }

    // Power up sprites
    // In order of sprite sheet
    enum ID
    {
        Shield,
        Turret,
        BigBullet,
        AmmoRefill
    }

    bool PowerUpActive;

    //Sprite[] spriteSheet;

    #region Inspector Variables
    [SerializeField]
    GameObject[] PowerUpGOs;

    [Header("Spawning")]
    // Where the power up will spawn
    [SerializeField]
    float MinSpawnHeight, MaxSpawnHeight;
    [SerializeField]
    float SpawnX;
    // How often the spawn diceroll is made
    [SerializeField]
    float DiceRollInterval;
    // Chance of spawning power up per diceroll
    [SerializeField]
    float DiceRollChance;

    [Header("Properties")]
    [SerializeField]
    float MoveSpeed;
    [SerializeField]
    float FloatSpeed;
    [SerializeField]
    float FloatDeviation;
    #endregion

    void Awake()
    {
        Instance = this;
        PowerUpActive = false;

        /*
        // Load power up sprites
        spriteSheet = Resources.LoadAll<Sprite>("PowerUpSheet");
        
        powerUpGOs = new GameObject[spriteSheet.Length];

        for (int i = 0; i < powerUpGOs.Length; i++)
        {
            powerUpGOs[i] = new GameObject(i.ToString());
            SpriteRenderer newSpr = powerUpGOs[i].AddComponent<SpriteRenderer>();
            newSpr.sprite = spriteSheet[i];
            CircleCollider2D newCol = powerUpGOs[i].AddComponent<CircleCollider2D>();
        }
         * */
    }

    // Use this for initialization
    void Start()
    {
        // Set up power ups
        for (int i = 0; i < PowerUpGOs.Length; i++)
        {
            PowerUpGOs[i].GetComponent<PowerUp>().SetUp(MoveSpeed, FloatSpeed, FloatDeviation);
        }
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

    IEnumerator SpawnNewPowerUp()
    {
        // Keep running while game is playing
        while (GameController.Instance.InGame)
        {
            yield return new WaitForSeconds(DiceRollInterval);

            Debug.Log("checking");

            // Don't spawn anything if there's already a powerup on the field
            // Don't spawn if diceroll fails
            if (Random.value > DiceRollChance || PowerUpActive)
            {
                continue;
            }

            Debug.Log("spawning");

            // Randomise which power up
            int newPUP = Random.Range(0, PowerUpGOs.Length);
            // DEBUG ONLY
            newPUP = 2;

            // Set spawn position
            float spawnY = Random.Range(MinSpawnHeight, MaxSpawnHeight);
            float spawnX = SpawnX * Helper.RandomSign();

            // Spawn power up
            PowerUpActive = true;
            PowerUpGOs[newPUP].transform.position = new Vector3(spawnX, spawnY, 1.0f);
            
            PowerUpGOs[newPUP].SetActive(true);
            
        }
    }

    public void Reset()
    {
        Clear();
        StartCoroutine(SpawnNewPowerUp());
    }

    public void Clear()
    {
        for (int i = 0; i < PowerUpGOs.Length; i++)
        {
            PowerUpGOs[i].SetActive(false);
        }
    }
}
