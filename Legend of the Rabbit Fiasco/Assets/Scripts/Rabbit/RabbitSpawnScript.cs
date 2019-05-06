using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpawnData
{
    public string difficulty;
    //! Rate in seconds each type of rabbit spawn represented in a list of 3
    public List<float> spawnRateList;
    //! Amount of rabbits spawned in each interval represented in a list of 3
    public List<float> spawnAmountList;
}

public class RabbitSpawnScript : MonoBehaviour
{

    public static RabbitSpawnScript instance { get; set; }

    private int rabbitCount = 3; //! rmb to change this!

    public List<GameObject> rabbitList;
    private List<float> spawnRateList;
    private List<float> spawnAmountList;
    //! The time in seconds they will start spawning represented in a list of 3
    public List<float> spawnStartTimeList;
    public List<int> spawnLimitList;
    [HideInInspector]
    public List<int> spawedNumberList;

    [HideInInspector]
    public float elaspedTime;

    public List<GameObject> platformList;
    public List<GameObject> platformListCopy;
    private List<float> elaspedTimeList;

    public List<SpawnData> spawnDataList;

    private void Awake()
    {
        if (instance != null && instance != this)
            Destroy(gameObject);
        else
            instance = this;
    }

    void Start ()
    {
        DifficultyCheck();
        elaspedTimeList = new List<float>();
        spawedNumberList = new List<int>();
        platformListCopy = platformList;
        for(int i = 0; i < rabbitCount; i ++)
        {
            elaspedTimeList.Add(0);
            spawedNumberList.Add(0);
        }
        elaspedTime = 0.0f;
	}
	
	void Update ()
    {
        SpawnRabbits();
        for (int i = 0; i < rabbitCount; i++)
        {
            elaspedTimeList[i] += Time.deltaTime;
        }

        elaspedTime += Time.deltaTime;
    }

    void SpawnRabbits ()
    {
        for (int i = 0; i < rabbitCount; i++)
        {
            if (elaspedTimeList[i] >= spawnRateList[i] && elaspedTime >= spawnStartTimeList[i] && spawedNumberList[i] < spawnLimitList[i])
            {
                for (int j = 0; j < spawnAmountList[i]; j++)
                {
                    int randomPlatform;
                    switch (i)
                    {
                        case 0:
                            randomPlatform = Random.Range(3, platformList.Count + 9); //! The +9 gives a 69 percent to spawn on ground
                            break;
                        case 1:
                            randomPlatform = Random.Range(0, platformListCopy.Count * 2);
                            break;
                        case 2:
                            randomPlatform = platformList.Count;
                            break;
                        default:
                            randomPlatform = 0;
                            break;
                    }

                    float randomX;
                    //! Platform spawning for small rabbit and big rabbit
                    if (randomPlatform < platformList.Count && i != 1)
                    {
                        randomX = Random.Range(-1.0f, 1.0f);
                        Vector3 platformPos = platformList[randomPlatform].transform.position;
                        Instantiate(rabbitList[i], new Vector3(platformPos.x - randomX, platformPos.y, 0.0f), Quaternion.identity);
                    }
                    //! Platform spawning for carrot rabbit
                    else if (randomPlatform < platformListCopy.Count && i == 1)
                    {
                        randomX = Random.Range(-1.0f, 1.0f);
                        Vector3 platformPos = platformListCopy[randomPlatform].transform.position;

                        GameObject tempRabbit = Instantiate(rabbitList[i], new Vector3(platformPos.x - randomX, platformPos.y, 0.0f), Quaternion.identity);
                        tempRabbit.GetComponent<RabbitAIScript>().spawnedPlatform = platformListCopy[randomPlatform];
                        platformListCopy.Remove(platformListCopy[randomPlatform]);
                    }
                    //! Ground Spawn
                    else
                    {
                        randomX = Random.Range(-8.5f, 8.5f);
                        Instantiate(rabbitList[i], new Vector3(randomX, -4.151f, 0.0f), Quaternion.identity);
                    }
                    spawedNumberList[i]++;
                }
                elaspedTimeList[i] = 0.0f;
            }

        }
    }

    void DifficultyCheck ()
    {
        switch (GameDataScript.instance.difficulty)
        {
            case GameDataScript.Difficulty.EASY:
                AssignDifficulty(1);
                break;
            case GameDataScript.Difficulty.REGULAR:
                AssignDifficulty(2);
                break;
            case GameDataScript.Difficulty.HARD:
                AssignDifficulty(3);
                break;
        }
    }

    void AssignDifficulty (int difficulty)
    {
        spawnRateList = spawnDataList[difficulty - 1].spawnRateList;
        spawnAmountList = spawnDataList[difficulty - 1].spawnAmountList;
    }
}
