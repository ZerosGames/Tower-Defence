using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class EnemySpawner : MonoBehaviour {

    public static int enemyAlive = 0;
    public Transform[] spawnLocations;
    public float timeBetweenWaves = 5f;

    private float countDown = 2f;
    private List<Transform> activePortals;
    private int waveIndex = 0;
    private float waveDifficulty;
    private Wave nextWave;
    private bool startNextWave = false;
    private bool isSpawningWaves = false;

    void Start()
    {
        activePortals = new List<Transform>();
        activePortals.Add(spawnLocations[0]);
    }

    void Update()
    {
        if (enemyAlive > 0 && !startNextWave)
            return;

        if (!GameManager.gameManager.GetPause())
        {
            if (isSpawningWaves)
            {
                if (countDown <= 0 || startNextWave)
                {
                    StartCoroutine(SpawnWave(GenerateNextWave()));
                    countDown = timeBetweenWaves;
                    startNextWave = false;
                    return;
                }

                countDown -= Time.deltaTime;
            }
        }
    }

    IEnumerator SpawnWave(Wave _wave)
    {
        waveIndex++;

        for (int i = 0; i < _wave.enemyCount; i++)
        {
            for (int p = 0; p < _wave.activePortals; p++)
            {
                SpawnEnemy(p);
            }
            yield return new WaitForSeconds(1f);
        }
    }

    void SpawnEnemy(int _portalIndex)
    {
        enemyAlive++;
        GameObject Temp = Instantiate(References.Refs.GetEnemy(0), spawnLocations[_portalIndex].position, Quaternion.identity).gameObject;
        References.enemysAlive.Add(Temp);
    }

    public string GetWaveTimerText()
    {
        return Mathf.Floor(countDown).ToString();
    }

    public string GetWaveText()
    {
       return "Wave " + waveIndex.ToString();
    }

    public bool GetIsSpawning()
    {
        return isSpawningWaves;
    }

    public void SetIsSpawning(bool _Spawning)
    {
        isSpawningWaves = _Spawning;
    }

    public void StartNextWave()
    {
        startNextWave = true;
    }

    Wave GenerateNextWave()
    {
        Wave nextWave = new Wave();

        nextWave.activePortals = 1;
        nextWave.enemyCount = 10;
        nextWave.enemysType = 0;

        return nextWave;
    }
}
