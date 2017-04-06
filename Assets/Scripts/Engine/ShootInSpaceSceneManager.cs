using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShootInSpaceSceneManager : MonoBehaviour
{
    public static ShootInSpaceSceneManager Me;
    public Player Player;

    [SerializeField]
    GameObject[] edges;

    [SerializeField]
    public List<GameObject> enemies = new List<GameObject>();

    [SerializeField]
    private int maxBigAsteroids;

    [SerializeField]
    GameObject bigAsteroidPrefab;

    public GameState state;

    public StartMenu StartMenu;
    public GameObject Hud;
    

    void Awake()
    {
        state = GameState.StartMenu;
        Me = this;
    }

    // Use this for initialization
    void Start()
    {
        //StartGame();
        GoToStartMenu();

        //Bind StartMenus StartGame action to StartGame()
        StartMenu.StartGame = StartGame;

    }

    private void GoToStartMenu()
    {
        state = GameState.StartMenu;
        Hud.SetActive( false);        
        StartMenu.enabled = true;
     }

    void OnGameOver()
    {
        Debug.Log("All is lost!");
        Player.gameObject.SetActive(false);
        foreach (GameObject thing in enemies)
        {
            Destroy(thing);           
        }
        enemies.Clear();
        CancelInvoke();
        Invoke("GoToStartMenu", 1f);
    }

    private void StartGame()
    {
        print("starting game");
        state = GameState.InGame;
        Player.OnGameOver = OnGameOver;
        Player.gameObject.SetActive(true);
        Player.Lifes = 1;
        Player.SetupNewShip();
        SfxManager.PlaySfx(SfxNames.PlayerStart.ToString());
        InvokeRepeating("CheckIfSpawnEnemies", 1f, 1f);

        Hud.SetActive(true);
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
        var edge = edges[Random.Range(0, edges.Count())];
        var spawner = edge.transform.Find("SpawnZone");
   
        // instantiate
        var bigasteroid = SpawnObject(bigAsteroidPrefab, spawner.transform.position);
        var rb2d = bigasteroid.GetComponent<Rigidbody2D>();

        // set up
        enemies.Add(bigasteroid);
        bigasteroid.gameObject.SetActive(true);

        // add force towards center of screen
        var targetPosition = new Vector3(Random.Range(-3, 3), Random.Range(-3, 3), 00);
        var forceAmount = Random.Range(1000f, 6000f);
        rb2d.AddForce((targetPosition - bigasteroid.transform.position).normalized * forceAmount);

        Debug.DrawLine(targetPosition, bigasteroid.transform.position, Color.red, 3f);

    }

    public static GameObject SpawnObject(GameObject prefab, Vector2 position)
    {

        var v3 = new Vector3(position.x,position.y, 0);
        return Instantiate(prefab, v3, Quaternion.Euler(Me.transform.rotation.eulerAngles));
    }
}

public enum GameState
{
    StartMenu, 
    InGame,
    InGame_Pause
}