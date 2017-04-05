using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoBase : MonoBehaviour {

    public EntityType EntityType;

    public EntityType GetEntityType(GameObject go)  {
        var monoBase = go.GetComponent<MonoBase>();

        if (monoBase)
        {
            return monoBase.EntityType;
        }
        else
        {
            return EntityType.Undefined;
        }
    }
}

public enum EntityType
{
    Undefined,
    Player,
    LaserShot,
    SmallAsteroid, 
    BigAsteroid,
    EnergyPickup
}
