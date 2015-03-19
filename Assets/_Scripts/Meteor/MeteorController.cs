using UnityEngine;
//using System.Collections.Generic;
using MemoryManagment; // Object pools
using System.Collections; // IEnumerator

public class MeteorController : MonoBehaviour
{
    public static MeteorController Instance { get; set; }

#region Inspector Variables
    // Prefab to spawn
    [SerializeField]
    GameObject METEOR_PREFAB;

    [Header("Spawning")]
    // Range of where the meteor can spawn
    [SerializeField]
    float MIN_SPAWN_X;
    [SerializeField]
    float MAX_SPAWN_X;
    // How high the meteors spawn, measured from the bottom of the meteor
    [Tooltip("Measured from the bottom of the meteor")]
    [SerializeField]
    float SPAWN_Y;

    [Header("Waves")]
    // The pause inbetween each wave
    [SerializeField]
    float WAVE_REST_PERIOD;
    // How quickly the meteors are spawned during a wave
    [SerializeField]
    float METEOR_SPAWN_INTERVAL;
    // Spawn a big meteor every x wave
    [Tooltip("Spawn a big meteor every x wave")]
    [SerializeField]
    int BIG_METEOR_WAVE;
    // How many meteors are spawned per regular wave
    [SerializeField]
    int MIN_NUM_METEORS;
    [SerializeField]
    int MAX_NUM_METEORS;

    [Header("Size")]
    [Tooltip("Ratio between screen space co-ordinates and meteor scale")]
    [SerializeField]
    float SCREEN_SCALE_RATIO;
    // Range of how big the normal meteors can spawn
    [SerializeField]
    float MIN_METEOR_SIZE;
    [SerializeField]
    float MAX_METEOR_SIZE;
    // How big the big meteor is
    [SerializeField]
    float BIG_METEOR_SIZE;
#endregion

    // Meteor gameobjects to use
    GameObjectPool meteorPool;

    // Wave number
    int waveNumber = 0;

    //List<Meteor> Meteors;

    void Awake()
    {
        Instance = this;
        //Meteors = new List<Meteor>();
        meteorPool = new GameObjectPool(10, METEOR_PREFAB, gameObject);
    }

    public void Reset()
    {
        waveNumber = 0;
        StartCoroutine(SpawnWaves());
    }

    // Spawn a new regular meteor at a randomise point and size
    void SpawnMeteor(bool bigMeteor)
    {
        // Grab an inactive meteor from the pool
        GameObject meteorGO = meteorPool.New();
        if (meteorGO != null)
        {
            // Used to set up and spawn the meteor
            var meteorScript = meteorGO.GetComponent<Meteor>();
                        
            float spawnSize = 0;
            float spawnPointX = 0;

            // Regular meteor
            if (!bigMeteor)
            {
                // Randomise a meteor size
                spawnSize = Random.Range(MIN_METEOR_SIZE, MAX_METEOR_SIZE);
                // Randomise a spawn point
                spawnPointX = Random.Range(MIN_SPAWN_X, MAX_SPAWN_X);
            }
            // big meteor
            else
            {
                spawnSize = BIG_METEOR_SIZE;
                // centre of screen
                spawnPointX = 0;
            }
            
            // Bottom of the meteor + the radius in world co-ordinates
            float spawnPointY = SPAWN_Y + (spawnSize * SCREEN_SCALE_RATIO);
            Vector2 spawnPoint = new Vector2(spawnPointX, spawnPointY);
            
            // Spawn the meteor
            meteorScript.Spawn(spawnPoint, spawnSize);
        }
    }

    // Continuely spawn meteors at random positions and sizes
    IEnumerator SpawnWaves()
    {
        // Keep going until game over
        while (GameStates.Current == GameStates.States.PLAYING)
        {
            // Increase the wave number
            waveNumber++;

            // Check whether it's a big meteor wave
            if (waveNumber % BIG_METEOR_WAVE != 0)
            {
                // Spawn a random number of meteors each wave
                // TODO: Include a bias so higher waves spawn more meteors
                int numMeteors = Random.Range(MIN_NUM_METEORS, MAX_NUM_METEORS);
                for (int i = 0; i < numMeteors; i++)
                {
                    SpawnMeteor(false);

                    // Don't spawn them all at once
                    yield return new WaitForSeconds(METEOR_SPAWN_INTERVAL);
                }
            }
            else
            {
                SpawnMeteor(true);
                
                // TODO: Wait for big meteor to leave before continuing
            }

            // Wait before starting the next wave
            yield return new WaitForSeconds(WAVE_REST_PERIOD);
        }
    }

    // Disable a meteor and add it back into the pool to be re-used
    public void StoreMeteor(GameObject meteor)
    {
        if (meteorPool != null)
        {
            meteorPool.Store(meteor);
        }
    }
}
