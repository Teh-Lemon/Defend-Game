using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Variables
    // Used to update ammo count
    public static PlayerController Instance { get; set; }

    [SerializeField] Turret turret;
    #endregion

    // Use this for initialization
    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        Reset();
    }

    // Update is called once per frame
    void Update()
    {
        switch (GameStates.Current)
        { 
            case GameStates.States.PLAYING:
                // Fire a bullet at the crosshair
                // If the player clicks with the mouse
                if (Input.GetButton("Fire1"))
                {
                    // Mouse position in the world space
                    Vector2 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                    turret.ShootBullet(worldPos);
                }
                // or touchs their phone screen
                else if (Input.touchSupported)
                {
                    if (Input.touchCount > 0)
                    {
                        Vector2 worldPos = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);

                        turret.ShootBullet(worldPos);
                    }
                }

                if (!turret.IsAlive)
                {
                    StartCoroutine(GameController.Instance.GameOver());
                }
                break;

        }
    }

    public void Reset()
    {        
        turret.Shield.ToggleShield(true);
        turret.Reset();
    }
}
