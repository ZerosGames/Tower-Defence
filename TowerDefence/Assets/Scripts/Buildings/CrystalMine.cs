using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalMine : MonoBehaviour
{

    public int zonmCrystalPerSecond = 1;

    [SerializeField]
    private bData buildingData = new bData();
    

	public void StartMining ()
    {
        InvokeRepeating("MineCrystal", 0, 1);
	}
	
    void MineCrystal()
    {
        References.Refs.playerData.currentZomnCurrency += zonmCrystalPerSecond;
    }

    public bData GetBuildingData()
    {
        return buildingData;
    }
}
