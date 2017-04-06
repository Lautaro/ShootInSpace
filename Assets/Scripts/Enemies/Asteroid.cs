using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : SpaceObject {




    

	// Use this for initialization
	protected override void Awake () {
        base.Awake();

    }

     void OnCollisionEnter2D(Collision2D collider)
    {
        if (collider.gameObject.name.Contains("Laser Shot"))
        {
            SfxManager.PlaySfx(SfxNames.SmallExplosion.ToString());
            TakeDamage(1);
            damageAnimation.Play();
            Destroy(collider.gameObject);
        }

        if (collider.gameObject.name.Contains("Asteroid"))
        {
            SfxManager.PlaySfx(SfxNames.SmallRockHit.ToString());            
        }
    }

    protected override void Die()
    {
        ShootInSpaceSceneManager.Me.enemies.Remove(gameObject);
        Destroy(gameObject, 3f);
        base.Die();
    }

}
