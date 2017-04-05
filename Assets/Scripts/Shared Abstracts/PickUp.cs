using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBase
{
    [SerializeField]
    SfxNames spawnSfx;
    [SerializeField]
    SfxNames pickupSfx;

    // Use this for initialization
    void Start()
    {
        if (spawnSfx != null)
        {
            SfxManager.PlaySfx(spawnSfx.ToString());
        }

    }

    public void PickedUp()
    {
        SfxManager.PlaySfx(pickupSfx.ToString());
        GetComponentInChildren<SpriteRenderer>().enabled = false;
        GetComponent<Collider2D>().enabled = false;
        GetComponentInChildren<ParticleSystem>().Stop();
        Destroy(gameObject, 3f);

    }
   
}