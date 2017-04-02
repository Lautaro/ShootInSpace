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
    float gunCoolDownTime;
    bool gunCanFire = true;

    [SerializeField]
    AudioClip laserSfx;

    [SerializeField]
    AudioClip playerStartSfx;

    [SerializeField]
    AudioClip playerExplosionSfx;

    [SerializeField]
    AudioClip playerCollisionSfx;

    [SerializeField]
    AudioSource accelerationAudioSource;
      

    #endregion


    // Use this for initialization
    protected override void Start()
    {
        base.Start();
        PlaySfx(playerStartSfx, 1f);
    }

    void Update()
    {
        var cam = Camera.main;

        // Distance from camera to object.  We need this to get the proper calculation.
        float camDistance = cam.transform.position.y - transform.position.y;

        // Get the mouse position in world space. Using camDis for the Z axis.
        Vector3 mouse = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, camDistance));

        float AngleRad = Mathf.Atan2(mouse.y - transform.position.y, mouse.x - transform.position.x);
        float angle = (180 / Mathf.PI) * AngleRad;

        rb2d.rotation = angle - 90;

    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if (Input.GetKey(KeyCode.Mouse1))
        {
            if (!accelerationAudioSource.isPlaying)
            {
                accelerationAudioSource.Play();
            }

            accelerationAnimation.Play();

            Accelerate(acceleration);
        }
        else
        {
            accelerationAudioSource.Stop();
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
        PlaySfx(laserSfx, 1f);
        var bulletInstance = Instantiate(bulletPrefab, gun.transform.position, Quaternion.Euler(transform.rotation.eulerAngles));
        Physics2D.IgnoreCollision(bulletInstance.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        bulletInstance.gameObject.SetActive(true);

    }

     protected override void OnCollisionEnter2D(Collision2D collider)
    {
        base.OnCollisionEnter2D(collider);
        var asteroid = collider.gameObject.GetComponent<BigAsteroid>();
        if (asteroid)
        {
            damageAnimation.Play();
            TakeDamage(4);
        }
    }

}
