using UnityEngine;
using System.Collections;

public class TurretBot : MonoBehaviour
{
    GameObject target;
    bool updatingTarget;

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

    void Start()
    {
        turretScript.Shield.ToggleShield(false);
        turretScript.AmmoCapacity = -1;
        turretScript.FireCooldown = FireRate;
    }

    // Update is called once per frame
    void Update()
    {
        switch (GameStates.Current)
        {
            case GameStates.States.PLAYING:
                if (!updatingTarget)
                {
                    StartCoroutine(UpdateTarget());
                }
                if (target != null)
                {
                    BurstFire();   
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
    void BurstFire()
    {
        turretScript.ShootBullets(target.transform.position, BulletsPerBurst);
    }


    public void Disable()
    {
        gameObject.SetActive(false);
    }

    public void Spawn()
    {
        gameObject.SetActive(true);
    }


}
