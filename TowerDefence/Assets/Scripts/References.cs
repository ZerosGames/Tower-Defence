using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class References : MonoBehaviour{

    public static References Refs;

    //Turrets
    public static List<GameObject> turretPlaced = new List<GameObject>();
    public GameObject[] turretTypes;

    //Enemys
    public static List<GameObject> enemysAlive = new List<GameObject>();
    public GameObject[] enemyTypes;

    //buildings
    public static List<GameObject> buildingsPlaced = new List<GameObject>();
    public GameObject[] buildingTypes;

    //InGameRefs
    public EnemySpawner spawnerRef;
    public BuildManager buildManager;
    public PlacementController placeController;
    public PlayerData playerData;
    public InputManager inputManager;
    public MouseManager mouseController;
    public MapGenerator mapGenerator;
    public CameraController camController;
    public World world;

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

    public void InitReferences()
    {
        spawnerRef = FindObjectOfType<EnemySpawner>();
        buildManager = FindObjectOfType<BuildManager>();
        placeController = FindObjectOfType<PlacementController>();
        playerData = FindObjectOfType<PlayerData>();
        inputManager = FindObjectOfType<InputManager>();
        mouseController = FindObjectOfType<MouseManager>();
        mapGenerator = FindObjectOfType<MapGenerator>();
        camController = FindObjectOfType<CameraController>();
        world = FindObjectOfType<World>();
    }

    //Turret Gets\\

    public GameObject GetTurret(int _type)
    {
        return turretTypes[_type];
    }

    public tData GetTurretData(int _type)
    {
        return turretTypes[_type].GetComponent<TurretBase>().GetTurretData();
    }

    //Enemy Gets\\
    public GameObject GetEnemy(int _type)
    {
        return enemyTypes[_type];
    }

    public eData GetEnemyData(int _type)
    {
        return enemyTypes[_type].GetComponent<EnemyBase>().enemyData;
    }

    //Building Gets\\

    public GameObject GetBuilding(int _type)
    {
        return buildingTypes[_type];
    }

    public bData GetBuilingData(int _type)
    {
        return buildingTypes[_type].GetComponent<CrystalMine>().GetBuildingData();
    }
}
