using UnityEngine;
using System.Collections;

public class TurretBot : MonoBehaviour
{
    // Firing
    // Found meteor target
    GameObject target;
    // Is the turret looking for a new target
    bool updatingTarget;
    // Is the turret in the middle of a burst fire
    bool firing;
    // Default position
    Vector3 defaultPos;

    // Spawn animation
    bool spawnAnimPlaying;
    //float turretHeight;

    #region Inspector Variables
    [SerializeField]
    GameObject turretGO;
    [SerializeField]
    Turret turretScript;

    // How fast they can fire each bullet
    [SerializeField]
    float FIRE_RATE;
    // Cooldown between each burst of bullets
    [SerializeField]
    float BURST_RATE;
    // Number of bullets fired per burst
    [SerializeField]
    int BULLETS_PER_BURST;

    // Delay before locking onto new targets
    [SerializeField]
    float REACTION_TIME;

    [SerializeField]
    float SPAWN_ANIM_LENGTH;
    #endregion

    void Start()
    {
        turretScript.Shield.ToggleShield(false);
        turretScript.AmmoCapacity = -1;
        turretScript.FireCooldown = FIRE_RATE;
        defaultPos = transform.position;
        //turretCollider = turretGO.GetComponent<Collider2D>();
        //turretHeight = turretScript.TURRET_BODY_SPRITE.bounds.size.y;
    }

    // Update is called once per frame
    void Update()
    {
        switch (GameStates.Current)
        {
            case GameStates.States.PLAYING:
                if (spawnAnimPlaying)
                {
                    break;
                }

                if (turretScript.IsAlive)
                {
                    // Firing
                    if (!updatingTarget)
                    {
                        StartCoroutine(UpdateTarget());
                    }
                    if (target != null)
                    {
                        if (!firing)
                        {
                            StartCoroutine(BurstFire());
                        }
                    }
                }
                // Death check
                else
                {
                    Disable();
                }
                break;
        }
    }

    // Find the next target, the closest on-screen meteor to the turret bot
    IEnumerator UpdateTarget()
    {
        updatingTarget = true;
        yield return new WaitForSeconds(REACTION_TIME);

        target = MeteorController.Instance.GetClosestMeteor(turretGO.transform.position, true);
        updatingTarget = false;
    }

    // Fire burst fire
    IEnumerator BurstFire()
    {
        firing = true;
        for (int i = 0; i < BULLETS_PER_BURST; i++)
        {
            if (target == null)
            {
                firing = false;
                yield break;
            }

            turretScript.ShootBullet(target.transform.position);

            yield return new WaitForSeconds(FIRE_RATE);
        }
        yield return new WaitForSeconds(BURST_RATE);
        firing = false;
    }

    // Move the turret below the stage then raise it up
    IEnumerator PlaySpawnAnim()
    {
        spawnAnimPlaying = true;
        float timePassed = 0.0f;
        
        transform.Translate(0, -(turretScript.GetHeight() / 2), 0);
        Vector3 startPos = transform.position;

        while (transform.position != defaultPos)
        {
            // Move the animation along every frame
            timePassed += Time.deltaTime;
            //Debug.Log("timepassed " + timePassed);
            if (timePassed > SPAWN_ANIM_LENGTH)
            {
                timePassed = SPAWN_ANIM_LENGTH;
            }

            float t = timePassed / SPAWN_ANIM_LENGTH;
            // Ease out
            //t = Mathf.Sin(t * Mathf.PI * 0.5f);
            // Smoothstep http://en.wikipedia.org/wiki/Smoothstep
            //t = t * t * (3f - 2f * t);            
            // Smootherstep
            t = t * t * t * (t * (6f * t - 15f) + 10f);
            transform.position = Vector3.Lerp(startPos, defaultPos, t);

            // Resume next frame
            yield return null;
        }

        spawnAnimPlaying = false;
    }


    public void Disable()
    {
        turretScript.IsAlive = false;
        //turretScript.GetSprite().enabled = false;
        turretScript.ToggleVisibility(false);
        //transform.Translate(0f, -100f, 0f);
        turretScript.GetCollider().enabled = false;
        //gameObject.SetActive(false);
    }

    public void Spawn()
    {
        //Awake();
        //gameObject.SetActive(true);
        turretScript.Reset();
        spawnAnimPlaying = false;
        updatingTarget = false;
        firing = false;
        //Debug.Log("spawning");        
        turretScript.GetCollider().enabled = true;
        transform.position = defaultPos;
        StartCoroutine(PlaySpawnAnim());
    }

    public bool IsAlive
    {
        get { return turretScript.IsAlive; }
    }
}