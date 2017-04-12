using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalMine : MonoBehaviour
{

    public int zonmCrystalPerSecond = 1;

    PlayerData playerData;

    [SerializeField]
    private bData buildingData = new bData();
    
    void Start()
    {
        playerData = PlayerData.playerData;
    }

	public void StartMining ()
    {
        InvokeRepeating("MineCrystal", 0, 1);
	}
	
    void MineCrystal()
    {
        playerData.currentZomnCurrency += zonmCrystalPerSecond;
    }

    public bData GetBuildingData()
    {
        return buildingData;
    }
}
