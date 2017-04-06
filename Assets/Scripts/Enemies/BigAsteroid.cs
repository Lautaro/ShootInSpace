using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigAsteroid : Asteroid
{

    Vector2 velocity;


    [SerializeField]
    GameObject pickUp;

    [SerializeField]
    GameObject smallAsteroidPrefab;

    [SerializeField]
    bool isSpawning = true;

protected override   void Awake()
    {
        base.Awake();
        EntityType = EntityType.BigAsteroid;
    }

    /// On creation collider is trigger. To enable it to pass the edges. 
    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.name.Contains("Edge"))
        {
            isSpawning = false;
            GetComponent<PolygonCollider2D>().isTrigger = false;
        }

    }

    private void SpawnSmallAsteroid()
    {

        // instantiate
        var smallAsteroid =  ShootInSpaceSceneManager.SpawnObject(smallAsteroidPrefab, transform.position);
        var rb2d = smallAsteroid.GetComponent<Rigidbody2D>();

        // set up
         ShootInSpaceSceneManager.Me.enemies.Add(smallAsteroid);
        smallAsteroid.gameObject.SetActive(true);

        // add force towards center of screen
        var targetPosition = new Vector3(Random.Range(-3, 3), Random.Range(-3, 3), 0);
        var forceAmount = Random.Range(100f, 500f);
        rb2d.AddForce((targetPosition - smallAsteroid.transform.position).normalized * forceAmount);

        Debug.DrawLine(targetPosition, smallAsteroid.transform.position, Color.red, 3f);

    }

    protected override void Die()
    {
        

        var amountOfAsteroids = Random.Range(1, 4);

        for (int i = 0; i < amountOfAsteroids; i++)
        {
            SpawnSmallAsteroid();
        }

        base.Die();

        // Spawn PickUp
        ShootInSpaceSceneManager.SpawnObject(pickUp, transform.position);

    }
}
