using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class Player : SpaceObject
{
    #region variables

    [SerializeField]
    ParticleSystem accelerationAnimation;

    [SerializeField]
    GameObject bulletPrefab;

    [SerializeField]
    GameObject gun;

    [SerializeField]
    float acceleration;

    [SerializeField]
    public Action OnGameOver;
    public Action ONCuck;

    [SerializeField]
    float gunCoolDownTime;
    bool gunCanFire = true;

    [SerializeField]
    int lifes;

    public int Lifes
    {
        get { return lifes; }
        set
        {
            lifes = value;         
        }
    }

    [SerializeField]
    bool ShieldIsUp;

    [SerializeField]
    float bulletEnergyCost;

    [SerializeField]
    float largeCollisionEnergyCost;

    [SerializeField]
    float smallCollisionEnergyCost;

    Vector2 startPosition;

    [SerializeField]
    float energy;

    public float Energy
    {
        get { return energy; }
        set
        {
            energy = value;
            Shield.SetShieldStrength(value);
        }
    }

    public PlayerShield Shield;
    public PlayerState State;

    public float InvisibilityTime;

    #endregion

    #region SET UP PLAYER


    protected override void Awake()
    {
        base.Awake();
        print(rb2d);
        startPosition = transform.position;
        EntityType = EntityType.Player;
        ShootInSpaceSceneManager.Me.Player = this;
        

    }

    void Start()
    {
             
        ShootInSpaceSceneManager.Me.Player = this;

    }

    void OnEnabled()
    {
        SetupNewShip();
    }
    public void SetupNewShip()
    {
        gameObject.SetActive(true);
        Debug.Log("Setting up ship");
        transform.position = new Vector2(0, 0);
        State = PlayerState.Playing;
        Shield.ReActivateShield();
        Energy = 100;
        GetComponent<Collider2D>().enabled = true;
        GetComponentInChildren<SpriteRenderer>().enabled = true;        
        rb2d.simulated = true;
        EnableInvisibility(2f);
        InvokeRepeating("RegeneratePlayerShield", 1f, 1f);
        Debug.Log(ONCuck );


    }
    #endregion

    #region GAME LOGIC
    void Update()
    {
        // ROTATE SHIP TOWARDS MOUSE
        #region

        var cam = Camera.main;

        // Distance from camera to object.  We need this to get the proper calculation.
        float camDistance = cam.transform.position.y - transform.position.y;

        // Get the mouse position in world space. Using camDis for the Z axis.
        Vector3 mouse = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, camDistance));

        float AngleRad = Mathf.Atan2(mouse.y - transform.position.y, mouse.x - transform.position.x);
        float angle = (180 / Mathf.PI) * AngleRad;

        rb2d.rotation = angle - 90;
        #endregion

    }

    void FixedUpdate()
    {

        if (Input.GetKey(KeyCode.Mouse1))
        {

            SfxManager.PlayLoopingSfxOnGo(SfxNames.Acceleration.ToString(), gameObject, true);

            accelerationAnimation.Play();

            Accelerate(acceleration);
        }
        else
        {
            SfxManager.StopLoopingSfxOnGO(SfxNames.Acceleration.ToString(), gameObject);
            accelerationAnimation.Stop();
        }

        if (Input.GetKey(KeyCode.Mouse0))
        {
            if (gunCanFire)
            {

                FireBullet();
                gunCanFire = false;
                Invoke("GunCoolDownFinished", gunCoolDownTime);

            }

        }
    }

    public void GunCoolDownFinished()
    {
        gunCanFire = true;
    }

    void FireBullet()
    {
 
        // Shoot only if there is enough energy and player is alive
        if (Energy - bulletEnergyCost > 0
            && (State == PlayerState.Playing || State == PlayerState.Playing_Invinsible))
        {
            Energy -= bulletEnergyCost;
            PlaySfx(SfxNames.Laser, 1f);
            var bulletInstance = Instantiate(bulletPrefab, gun.transform.position, Quaternion.Euler(transform.rotation.eulerAngles));
            Physics2D.IgnoreCollision(bulletInstance.GetComponent<Collider2D>(), GetComponent<Collider2D>());
            bulletInstance.gameObject.SetActive(true);
        }
    }

    protected override void TakeDamage(float damage)
    {
        if (State == PlayerState.Playing)
        {

            if (Shield.State != PlayerShieldState.Down)
            { // Shield is up

                if (Energy - damage < 0)
                {// Prevent sudden death if shield is active

                    Energy = 0;
                }
                else
                {
                    Energy -= damage;

                }
            }
            else
            {// Shiels is down

                Energy -= damage;
            }

          
            if (Energy < 0)
            {  
                Die();
            }
            else
            {
                StartCoroutine(Shield.ShieldBurst_cr());
                SfxManager.PlaySfx(SfxNames.PlayerCollision.ToString());
                State = PlayerState.Playing_Invinsible;
                EnableInvisibility(2f);
            }
        }
    }

    void DisableInvisibility()
    {
        if (State == PlayerState.Playing_Invinsible)
        {
            State = PlayerState.Playing;
        }
    }

    void EnableInvisibility(float time)
    {
        State = PlayerState.Playing_Invinsible;
        Invoke("DisableInvisibility", time);
    }

    protected override void Die()
    {
        print("Dead");
        base.Die();
        Lifes -= 1;
        print("Lifes = " + Lifes);
        SfxManager.StopLoopingSfxOnGO(SfxNames.Acceleration.ToString(), gameObject);

        if (Lifes <= 0)
        {   
            print("Game Over in one seconds");
            Invoke("GameOver", 3f);
            
        }
        else
        {
            rb2d.simulated = false;
            State = PlayerState.Respawning;
            print("New ship in two seconds");
            Invoke("SetupNewShip", 2f);

        }
    }

    void GameOver()
    {
        CancelInvoke();
        StopAllCoroutines();
        State = PlayerState.GameOver;
        OnGameOver();
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        switch (collider.gameObject.GetComponent<MonoBase>().EntityType)
        {

            case EntityType.EnergyPickup:
                var pickUp = collider.gameObject.GetComponent<Pickup>();
                pickUp.PickedUp();
                Energy += 20;
                break;
            default:
                break;
        }
    }

    void OnCollisionEnter2D(Collision2D collider)
    {

        switch (GetEntityType(collider.gameObject))
        {

            case EntityType.Player:
                break;
            case EntityType.LaserShot:
                break;
            case EntityType.SmallAsteroid:

                TakeDamage(smallCollisionEnergyCost);

                break;
            case EntityType.BigAsteroid:
                TakeDamage(largeCollisionEnergyCost);
                break;
            case EntityType.Undefined:
            default:
                break;
        }
    }

    public void AddEnergy(float p)
    {
        Energy += p;
    }

    void RegeneratePlayerShield()
    {
        if (Energy < 50)
        {
            AddEnergy(0.5f);

        }
    }

    #endregion


    public enum PlayerState
    {
        Playing,
        Playing_Invinsible,
        Respawning,
        GameOver
    }

}

public enum PlayerShieldState
{
    Healthy,
    Unhealthy,
    Low,
    Critical,
    Down
}
