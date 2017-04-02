using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : SpaceObject {




    

	// Use this for initialization
	protected override void Start () {
        base.Start();

    }

    protected override void OnCollisionEnter2D(Collision2D collider)
    {
        base.OnCollisionEnter2D(collider);

        if (collider.gameObject.name.Contains("Laser Shot"))
        {
            TakeDamage(1);
            damageAnimation.Play();
            Destroy(collider.gameObject);

        }
    }

    protected override void Die()
    {
        ShootInSpaceSceneManager.Me.enemies.Remove(gameObject);
       
        base.Die();
    }

}
