using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EdgeSpawnPoint : MonoBehaviour {

    SpriteRenderer spawnZone;
    void Start()
    {
        spawnZone = GetComponentInChildren<SpriteRenderer>();

    }

    public Vector3 GetRandomSpawnPosition()
    {
        var bound = spawnZone.bounds;
        var x = Random.Range(bound.extents.x - bound.size.x, bound.extents.x);
        var y = Random.Range(bound.extents.y - bound.size.y, bound.extents.y);

        return new Vector3(x, y, 0);

    }
}
