using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class SpaceObject : MonoBase
{

    [SerializeField]
    protected ParticleSystem deathAnimation;
    [SerializeField]
    protected ParticleSystem damageAnimation;
    protected Rigidbody2D rb2d;
    protected float deathDelay = 0f;

    
    [SerializeField]
    float hitPoints;

    public float HitPoints
    {
        get { return hitPoints; }

    }

    // Use this for initialization
    protected virtual void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();

    }
 
    protected void Accelerate(float force)
    {

        rb2d.AddForce(transform.up * force);
    }

    protected virtual void TakeDamage(float damage)
    {
        hitPoints -= damage;

        if (hitPoints < 0)
        {
            Die();
        }

    }




    public void PlaySfx(SfxNames sfx, float volume)
    {
         SfxManager.PlaySfx(sfx.ToString(),0f,volume);
         
    }

    protected virtual void Die()
    {
        PlaySfx(SfxNames.Death, 1f);
        deathDelay = 4f;
        GetComponentInChildren<SpriteRenderer>().enabled = false;
        deathAnimation.Play();
        rb2d.simulated = false;
        GetComponent<Collider2D>().enabled = false;
        
    }
}

