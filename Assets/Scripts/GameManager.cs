using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public float max_x_position, max_y_position, min_x_position, min_y_position;
    public LayerMask human, animal, tree,border;
    public List<GameObject> Animals;
    public List<GameObject> Trees;
    public List<GameObject> Humans;
    static int NUMBER_OF_ANIMALS = 9;
    public int levelDifficulty;
    public float gametime;
    public Text countdownTxt;
    bool chopperSpawned, hunterSpawned,lastWave;
    public static int woodBalance = 16, meatBalance = 16;
    public RectTransform meatIndicator, woodIndicator;
    public GameObject gameOver;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

        void Start()
    {
        StartCoroutine( SpawnTrees());
        SpawnAnimals();
        gametime = 90f;
        //RandomEncounter();
        StartCoroutine(Countdown());
        Time.timeScale = 1;
    }

    public void Restart()
    {
        gameOver.GetComponent<Canvas>().enabled = false;
        SceneManager.LoadScene("SampleScene");
        Time.timeScale = 1;
    }

    // Update is called once per frame
    void Update()
    {
        meatIndicator.position = new Vector2(meatIndicator.parent.position.x + meatBalance * 5, meatIndicator.parent.position.y);
        woodIndicator.position = new Vector2(woodIndicator.parent.position.x + woodBalance * 5, woodIndicator.parent.position.y);

        if (!gameOver.GetComponent<Canvas>().enabled && 
            ((woodIndicator.parent.position.x - 85 + woodBalance * 3) <= 0 ||
            (woodIndicator.parent.position.x - 85 + woodBalance * 3) >= 100 ||
            (meatIndicator.parent.position.x - 85 + meatBalance * 3) <= 0 ||
            (meatIndicator.parent.position.x - 85 + meatBalance * 3) >= 100))
        {
            gameOver.GetComponent<Canvas>().enabled = true;
            woodBalance = 0;
            meatBalance = 0;
            Time.timeScale = 0;
        }

        countdownTxt.text = "" + (int)gametime;
        if (gametime <= 45 && !chopperSpawned)
        {
            spawnChopper();
            chopperSpawned = true;
        }
        else if(gametime<=30 && !hunterSpawned)
        {
            spawnHunter();
            hunterSpawned = true;
        }
        else if(gametime<=15 && !lastWave)
        {
            spawnChopper();
            spawnHunter();
            lastWave = true;
        }

        if (gametime <= 0)
        {
            gameOver.GetComponent<Canvas>().enabled = true;
            Time.timeScale = 0;
        }

    }
    public void RandomEncounter()
    {
        int count = Random.Range(0, levelDifficulty);

    }
    public int PointCalculator()
    {
        return (0);
    }

    public float BalanceBar()
    {
        return (0);
    }

    public IEnumerator Countdown()
    {
        while(gametime>0)
        {
            gametime -= Time.deltaTime;
            yield return null;
        }
    }

    Vector3 spawnPosition;

    public void spawnHunter()
    {
        for (int i = 0; i < 3; i++)
        {
            spawnPosition = new Vector3(Random.Range(min_x_position, max_x_position), Random.Range(min_y_position, max_y_position), 0);
            while (Physics2D.OverlapCircle(spawnPosition, 2f, border) == null)
            {
                spawnPosition = new Vector3(Random.Range(min_x_position + 0.2f, max_x_position - 0.2f), Random.Range(min_y_position + 0.1f, max_y_position - 0.1f), 0);
            }

            Humans.Add(PoolManager.Instance.Spawn("Hunter", spawnPosition, Quaternion.identity));
        }
    }
    public void spawnChopper()
    {
        for(int i=0; i <3; i++)
        {
            spawnPosition = new Vector3(Random.Range(min_x_position, max_x_position), Random.Range(min_y_position, max_y_position), 0);
            while (Physics2D.OverlapCircle(spawnPosition, 2f, border) == null)
            {
                spawnPosition = new Vector3(Random.Range(min_x_position + 0.2f, max_x_position - 0.2f), Random.Range(min_y_position + 0.1f, max_y_position - 0.1f), 0);
            }

            Humans.Add(PoolManager.Instance.Spawn("Chopper", spawnPosition, Quaternion.identity));
        }
    }
    public void SpawnAnimals()
    {
        for (int i = 0; i < 8; i++)
        {
            spawnPosition = new Vector3(Random.Range(min_x_position + 5, max_x_position - 4), Random.Range(min_y_position + 3, max_y_position - 2), 0);
            while (Physics2D.OverlapCircle(spawnPosition, 2f, animal) != null || Physics2D.OverlapCircle(spawnPosition, 2f, tree) != null)
            {
                spawnPosition = new Vector3(Random.Range(min_x_position + 0.2f, max_x_position - 0.2f), Random.Range(min_y_position + 0.1f, max_y_position - 0.1f), 0);
            }

            Animals.Add(PoolManager.Instance.Spawn("Pig", spawnPosition, Quaternion.identity));


        }
    }
    public IEnumerator SpawnTrees()
    {
        for (int i = 0; i < 15; i++)
        {
            spawnPosition = new Vector3(Random.Range(min_x_position + 0.2f, max_x_position - 0.2f), Random.Range(min_y_position + 0.1f, max_y_position - 0.1f), 0);
            
            while (Physics2D.OverlapCircle(spawnPosition, 2f, tree) != null)
            {
                spawnPosition = new Vector3(Random.Range(min_x_position + 0.2f, max_x_position - 0.2f), Random.Range(min_y_position + 0.1f, max_y_position - 0.1f), 0);
            }
            Trees.Add(PoolManager.Instance.Spawn("Tree", spawnPosition, Quaternion.identity));
            yield return null;
        }

    }
    public IEnumerator spawnAnimal()
    {
        yield return new WaitForSeconds(1);
        spawnPosition = new Vector3(Random.Range(min_x_position + 10, max_x_position - 10), Random.Range(min_y_position + 10, max_y_position - 10), 0);
        Animals.Add(PoolManager.Instance.Spawn("Pig", spawnPosition, Quaternion.identity));
    }
        

    public GameObject getRandomAnimal()
    {
        
        return Animals[Random.Range(0, Animals.Count)];
    }
}
