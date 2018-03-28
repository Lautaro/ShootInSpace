using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShootInSpaceSceneManager : MonoBehaviour
{
    public static ShootInSpaceSceneManager Me;

    [SerializeField]
    GameObject[] edges;

    [SerializeField]
    public List<GameObject> enemies = new List<GameObject>();

    [SerializeField]
    private int maxBigAsteroids;

    [SerializeField]
    GameObject bigAsteroidPrefab;

    void Awake()
    {
        Me = this;
    }

    // Use this for initialization
    void Start()
    {
        StartGame();

    }

    private void StartGame()
    {        
        InvokeRepeating("CheckIfSpawnEnemies", 1f, 1f);
    }

    void CheckIfSpawnEnemies()
    {
        //Throw dice 
        if (Random.Range(0, 2) == 0)
        {
            var bigAsteroids = from a in enemies
                               where a.GetComponent<BigAsteroid>() != null
                               select a;

            if (bigAsteroids.Count() < maxBigAsteroids)
            {
                SpawnBigAsteroid();

            }
        }
    }

    private void SpawnBigAsteroid()
    {

        // get random start position 
        var edge = edges[Random.Range(0, edges.Count() )];
        var startPosition = edge.transform.position;
        startPosition.z = -1;

        // instantiate
        var bigasteroid = Instantiate(bigAsteroidPrefab, startPosition, Quaternion.Euler(transform.rotation.eulerAngles));
        var rb2d = bigasteroid.GetComponent<Rigidbody2D>();

        // set up
        enemies.Add(bigasteroid);
        bigasteroid.gameObject.SetActive(true);

        // add force towards center of screen
        var targetPosition = new Vector3(Random.Range(-3, 3), Random.Range(-3, 3),00);
        var forceAmount = Random.Range(500f, 3000f);
        rb2d.AddForce((targetPosition - bigasteroid.transform.position).normalized * forceAmount);

        Debug.DrawLine(targetPosition, bigasteroid.transform.position,Color.red,3f);

    }
}
