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
    [Header("Size")]
    [Tooltip("Ratio between screen space co-ordinates and meteor scale")]
    [SerializeField]
    float SCREEN_SCALE_RATIO;
    // Range of how big the normal meteors can spawn
    [SerializeField]
    float MIN_METEOR_SIZE;
    [SerializeField]
    float MAX_METEOR_SIZE;
    // How big the large meteor is
    [SerializeField]
    float LARGE_METEOR_SIZE;
#endregion

    // Meteor gameobjects to use
    GameObjectPool meteorPool;

    //List<Meteor> Meteors;

    void Awake()
    {
        Instance = this;
        //Meteors = new List<Meteor>();
        meteorPool = new GameObjectPool(10, METEOR_PREFAB, gameObject);
    }

    public void Reset()
    {
        StartCoroutine(SpawnWaves());
    }

    // Spawn a new regular meteor at a randomise point and size
    void SpawnMeteor()
    {
        // Grab an inactive meteor from the pool
        GameObject meteorGO = meteorPool.New();
        if (meteorGO != null)
        {
            // Used to set up and spawn the meteor
            var meteorScript = meteorGO.GetComponent<Meteor>();

            // Randomise a meteor size
            float spawnSize = Random.Range(MIN_METEOR_SIZE, MAX_METEOR_SIZE);

            // Randomise a spawn point
            float spawnPointX = Random.Range(MIN_SPAWN_X, MAX_SPAWN_X);
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
        while (GameStates.Current == GameStates.States.PLAYING)
        {
            SpawnMeteor();

            //// PLACEHOLDER VALUE ////
            yield return new WaitForSeconds(2);
        }
    }

    // Disable a meteor and add it back into the pool to be re-used
    void StoreMeteor(GameObject meteor)
    {
        if (meteorPool != null)
        {
            meteorPool.Store(meteor);
        }
    }
}
