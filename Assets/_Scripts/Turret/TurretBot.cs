using UnityEngine;
using System.Collections;

public class TurretBot : MonoBehaviour
{
    GameObject target;
    bool updatingTarget;
    bool firing;

    #region Inspector Variables
    [SerializeField]
    GameObject turretGO;
    [SerializeField]
    Turret turretScript;

    // How fast they can fire each bullet
    [SerializeField]
    float FireRate;
    // Cooldown between each burst of bullets
    [SerializeField]
    float BurstRate;
    // Number of bullets fired per burst
    [SerializeField]
    int BulletsPerBurst;

    // Delay before locking onto new targets
    [SerializeField]
    float REACTION_TIME;
    #endregion

    void Awake()
    {
        updatingTarget = false;
        firing = false;
    }

    void Start()
    {
        turretScript.Shield.ToggleShield(false);
        turretScript.AmmoCapacity = -1;
        turretScript.FireCooldown = FireRate;
        turretScript.Reset();
    }

    // Update is called once per frame
    void Update()
    {
        switch (GameStates.Current)
        {
            case GameStates.States.PLAYING:
                // Firing
                if (!updatingTarget)
                {
                    StartCoroutine(UpdateTarget());
                }
                if (target != null)
                {
                    if (!firing)
                    {
                        //StartCoroutine(BurstFire());
                    }
                }

                // Death check
                if (!turretScript.IsAlive)
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
        for (int i = 0; i < BulletsPerBurst; i++)
        {
            if (target == null)
            {
                firing = false;
                yield break;
            }

            turretScript.ShootBullet(target.transform.position);

            yield return new WaitForSeconds(FireRate);
        }
        yield return new WaitForSeconds(BurstRate);
        firing = false;
    }


    public void Disable()
    {
        gameObject.SetActive(false);
    }

    public void Spawn()
    {
        Awake();        
        gameObject.SetActive(true);
        turretScript.Reset();
    }


}
