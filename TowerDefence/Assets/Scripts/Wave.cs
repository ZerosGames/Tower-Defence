using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class Wave
{
    public Queue<GameObject> enemyToSpawn = new Queue<GameObject>();

    public int waveNumber;
    public int enemysType;
    public int enemyCount;
    public int timeBetweenWaces;
    public int activePortals;
    public int waveDifficulty;
}
