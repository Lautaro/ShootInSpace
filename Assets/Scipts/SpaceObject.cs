using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class SpaceObject : MonoBehaviour
{

    [SerializeField]
    protected AudioClip death;

    [SerializeField]
    protected AudioClip collision;

    [SerializeField]
    protected ParticleSystem deathAnimation;
    [SerializeField]
    protected ParticleSystem damageAnimation;
    protected float deathDelay = 0f;

    protected AudioSource audioPlayer;
    /*
         
Accelerate forward
Physics object	
Bounces off screen edges
Take damage
Explode when zero life
Takes damage from collision or laser shots
     */

    [SerializeField]
    float hitPoints;

    public float HitPoints
    {
        get { return hitPoints; }

    }

    protected Rigidbody2D rb2d;

    // Use this for initialization
    protected virtual void Start()
    {
        audioPlayer = GetComponent<AudioSource>();
        rb2d = GetComponent<Rigidbody2D>();

    }

   protected virtual void OnCollisionEnter2D(Collision2D collider)
    {
        PlaySfx(collision,1f);

    }

    protected void Accelerate(float force)
    {

        rb2d.AddForce(transform.up * force);
    }

    protected void TakeDamage(float damage)
    {
        hitPoints -= damage;

        if (hitPoints < 0)
        {
            Die();
        }

    }

    public void PlaySfx(AudioClip sfx, float volume)
    {

        audioPlayer.clip = sfx;
        audioPlayer.volume = volume;
        audioPlayer.Play();

    }

    protected virtual void Die()
    {
        PlaySfx(death, 1f);
        deathDelay = 4f;
        GetComponentInChildren<SpriteRenderer>().enabled = false;
        deathAnimation.Play();
        rb2d.simulated = false;
        GetComponent<Collider2D>().enabled = false;
        Destroy(gameObject, deathDelay);
    }

}

