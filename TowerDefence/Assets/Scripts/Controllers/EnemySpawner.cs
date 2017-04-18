using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class EnemySpawner : MonoBehaviour {

    [SerializeField]
    private MapGenerator mapGen;

    public static int enemyAlive = 0;
    public float timeBetweenWaves = 5f;

    private float countDown = 30f;
    private int waveIndex = 0;
    private int queuedWaveIndex = 0;
    private float waveDifficulty;
    private float timeBetweenEnemySpawns = 0.8f;

    float EnemysSpawn;

    [SerializeField]
    private Wave _nextWave;
    private Wave CurrentWave;
    private bool startNextWave = false;
    private bool isSpawningWaves = false;

    [SerializeField]
    GameObject enemyRoot;

    [SerializeField]
    public Queue<Wave> wavesToSpawn = new Queue<Wave>();

    public bool finisheWave = true;

    void Update()
    {
        HandleWaves();
    }

    void HandleWaves()
    {
        if (!GameManager.gameManager.GetPause() && isSpawningWaves)
        {
            if (wavesToSpawn.Count > 0)
            {
                if (finisheWave)
                {
                    waveIndex++;
                    CurrentWave = wavesToSpawn.Dequeue();
                    StartCoroutine(SpawnWave(CurrentWave));
                }

                return;
            }
            else if (enemyAlive <= 0)
            {
                if (countDown <= 0 || startNextWave && wavesToSpawn.Count <= 0)
                {
                    _nextWave = GenerateNextWave();
                    wavesToSpawn.Enqueue(_nextWave);

                    if (finisheWave)
                    {
                        waveIndex++;
                        CurrentWave = wavesToSpawn.Dequeue();
                        StartCoroutine(SpawnWave(CurrentWave));
                        countDown = timeBetweenWaves;
                    }

                    startNextWave = false;
                    return;
                }

                countDown -= Time.deltaTime;
            }
        }
    }

    IEnumerator SpawnWave(Wave _wave)
    {
        finisheWave = false;

        for (int i = 0; i < _wave.enemyCount; i++)
        {
            EnemysSpawn++;
            SpawnEnemy(References.Refs.GetEnemy(0));
            yield return new WaitForSeconds(timeBetweenEnemySpawns);
        }

        finisheWave = true;
    }

    void SpawnEnemy(GameObject _enemyToSpawn)
    {
        //enemyAlive++;
        //GameObject Temp = Instantiate(_enemyToSpawn, mapGen.Path[0].WorldPos, Quaternion.identity) as GameObject;
        //Temp.transform.parent = enemyRoot.transform;
        //Temp.GetComponent<EnemyBase>().SetPath(mapGen.Path);
        //References.enemysAlive.Add(Temp);

        enemyAlive++;       
        ObjectPoolManager.instance.ReuseObject(_enemyToSpawn, mapGen.Path[0].WorldPos, Quaternion.identity);
        References.enemysAlive.Add(_enemyToSpawn);
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

    public void NextWave()
    {
        Wave Temp = GenerateNextWave();
        wavesToSpawn.Enqueue(Temp);
    }

    public void StartNextWave()
    {
        startNextWave = true;
    }

    Wave GenerateNextWave()
    {
        queuedWaveIndex++;

        Wave nextWave = new Wave();

        nextWave.enemyCount += queuedWaveIndex;
        nextWave.enemysType = 0;

        return nextWave;
    }
}
