using UnityEngine;
//using System.Collections.Generic;
using MemoryManagment; // Object pools
using System.Collections; // IEnumerator
using System.Collections.Generic; // IEnumerator

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
    [Tooltip("Spawn a big meteor every x wave, -1 to disable")]
    [SerializeField]
    int BIG_METEOR_WAVE;
    // How many meteors are spawned per regular wave
    [SerializeField]
    int MIN_NUM_METEORS;
    [SerializeField]
    int MAX_NUM_METEORS;
    // How fast the waves get difficult, in percent
    [Tooltip("How fast the waves get difficult, in percent")]
    [SerializeField]
    float DIFFICULTY_SCALING;

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
    List<GameObject> activeMeteors;

    // Wave number
    int waveNumber = 0;
    // Used to adjust the difficulty curve
    LinerBiGraph difficultyCurve;

    void Awake()
    {
        Instance = this;
        meteorPool = new GameObjectPool(15, METEOR_PREFAB, gameObject);
        activeMeteors = new List<GameObject>();

        // Set up the difficulty curve
        difficultyCurve = new LinerBiGraph(new Vector2(0.0f, MIN_NUM_METEORS)
            , new Vector2(1.0f, MAX_NUM_METEORS));
    }

    public void Reset()
    {
        ClearMeteors();
        StopCoroutine("SpawnWaves");
        waveNumber = 0;
        difficultyCurve.ResetMidPoint();
        StartCoroutine("SpawnWaves");
    }

    // Store away all the meteors in-play
    public void ClearMeteors()
    {   
        for (int i = 0; i < activeMeteors.Count; i++)
        {
            StoreMeteor(activeMeteors[i]);
        }
        activeMeteors.Clear();
    }

    // Spawn a new regular meteor at a randomise point and size
    GameObject SpawnMeteor(GameStates.MeteorTypes meteorType)
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
            if (meteorType == GameStates.MeteorTypes.NORMAL)
            {
                // Randomise a meteor size
                spawnSize = Random.Range(MIN_METEOR_SIZE, MAX_METEOR_SIZE);
                // Randomise a spawn point
                spawnPointX = Random.Range(MIN_SPAWN_X, MAX_SPAWN_X);
            }
            // big meteor
            else if (meteorType == GameStates.MeteorTypes.BIG)
            {
                spawnSize = BIG_METEOR_SIZE;
                // centre of screen
                spawnPointX = 0;
            }

            // Bottom of the meteor + the radius in world co-ordinates
            float spawnPointY = SPAWN_Y + (spawnSize * SCREEN_SCALE_RATIO);
            Vector2 spawnPoint = new Vector2(spawnPointX, spawnPointY);

            

            // Spawn the meteor
            meteorScript.Spawn(spawnPoint, spawnSize, GameStates.MeteorTypes.NORMAL);
            activeMeteors.Add(meteorGO);
            return meteorGO;
        }
        else
        {
            return null;
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
            // Regular wave
            if (BIG_METEOR_WAVE == -1 || waveNumber % BIG_METEOR_WAVE != 0)
            {
                // Spawn a random number of meteors each wave
                //int numMeteors = Random.Range(MIN_NUM_METEORS, MAX_NUM_METEORS);
                int numMeteors =
                        Mathf.RoundToInt(difficultyCurve.Evaluate(Random.value));

                    Debug.Log("Wave " + waveNumber + ": " + numMeteors + " meteors");

                for (int i = 0; i < numMeteors; i++)
                {
                    SpawnMeteor(GameStates.MeteorTypes.NORMAL);

                    // Don't spawn them all at once
                    yield return new WaitForSeconds(METEOR_SPAWN_INTERVAL);                                        
                }
            }
            // Big meteor wave
            else
            {
                GameObject bigM = SpawnMeteor(GameStates.MeteorTypes.BIG);

                // Don't spawn anymore waves until the big meteor is gone
                if (bigM != null)
                {
                    while (bigM.activeInHierarchy)
                    {
                        Debug.Log("big meteor still active");
                        yield return new WaitForSeconds(1);
                    }
                    Debug.Log("BigM gone");
                }
                else
                {
                    Debug.Log("No bigm");
                }                
            }

            // Increase the wave difficulty
            difficultyCurve.ScaleMidPoint(-DIFFICULTY_SCALING, DIFFICULTY_SCALING);
            

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

    // Return the meteor closest to the provided position
    // onlyOnScreen, only choose meteors that are onScreen
    public GameObject GetClosestMeteor(Vector2 sourcePos, bool onlyOnScreen)
    {
        // No meteors in play
        if (activeMeteors.Count == 0)
        {
            return null;
        }

        // index in activeMeteors
        int closestMeteor = 0;
        // current closest meteor distance to source
        float closestDist = Vector2.Distance(sourcePos, activeMeteors[0].transform.position);
        float newDist = 0.0f;

        // Start at 2nd index
        for (int i = 1; i < activeMeteors.Count; i++)
        {
            // If onlyOnScreen is specified, check whether meteor is on screen
            if (!onlyOnScreen || activeMeteors[i].GetComponent<Renderer>().isVisible)
            {
                newDist = Vector2.Distance(sourcePos, activeMeteors[i].transform.position);

                if (newDist < closestDist)
                {
                    closestDist = newDist;
                    closestMeteor = i;
                }
            }
        }

        // Catch for when there's only 1 meteor in play
        if (!onlyOnScreen || activeMeteors[closestMeteor].GetComponent<Renderer>().isVisible)
        {            
            return activeMeteors[closestMeteor];
        }
        else
        {
            return null;
        }
    }
}
