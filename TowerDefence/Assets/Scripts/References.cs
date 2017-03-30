using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enum = System.Enum;

public class References : MonoBehaviour {

    public static References Refs;

    //Turrets
    public static List<GameObject> turretPlaced = new List<GameObject>();
    public GameObject[] turretTypes;

    //Enemys
    public static List<GameObject> enemysAlive = new List<GameObject>();
    public GameObject[] enemyTypes;

    //InGameRefs
    public EnemySpawner spawnerRef;

    //UI
    public UIManager UIManger;
    //Maps
    //Levels
        
    void Start()
    {
        if(Refs != null)
        {
            Debug.LogError("More than one refs class in scene");
            return;
        }

        Refs = this;
    }


    //Turret Gets\\

    public GameObject GetTurret(int _type)
    {
        return turretTypes[_type];
    }

    public tData GetTurretData(int _type)
    {
        return turretTypes[_type].GetComponent<TurretController>().GetTurretData();
    }

    //Enemy Gets\\
    public GameObject GetEnemy(int _type)
    {
        return enemyTypes[_type];
    }

    public eData GetEnemyData(int _type)
    {
        return enemyTypes[_type].GetComponent<EnemyController>().enemyData;
    }
}
