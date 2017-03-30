using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour {

    public int currentcurrency;
    public int startingCurrency = 300;

    public int currentZomnCurrency;
    public int startingZomnCurrency = 0;

    public static PlayerData playerData;

    void Awake()
    {
        if (playerData != null)
        {
            Debug.LogError("There are two PlayerData in Scene");
            return;
        }

        playerData = this;

    }

	void Start ()
    {
        currentcurrency = startingCurrency;
        currentZomnCurrency = startingZomnCurrency;
}

	void Update ()
    {
		
	}

    public bool purchaseTurretUpgrade(TurretController.TurretUpgradeTypes types, tData _turretData)
    {
        switch (types)
        {
            case TurretController.TurretUpgradeTypes.Damage:
                if(currentZomnCurrency - _turretData.costToUpgrade < 0)
                {
                    return false;
                }

                currentZomnCurrency -= _turretData.costToUpgrade;
                break;
            case TurretController.TurretUpgradeTypes.FireRate:
                if (currentZomnCurrency - _turretData.costToUpgrade < 0)
                {
                    return false;
                }

                currentZomnCurrency -= _turretData.costToUpgrade;
                break;
            case TurretController.TurretUpgradeTypes.Range:
                if (currentZomnCurrency - _turretData.costToUpgrade < 0)
                {
                    return false;
                }

                currentZomnCurrency -= _turretData.costToUpgrade;
                break;
            default:
                break;
        }

        //currentcurrency -= _turretData.costToBuild;
        return true;
    }

    public bool purchaseTurret(tData _turretData)
    {
        if(currentcurrency - _turretData.costToBuild < 0)
        {
            return false;
        }

        currentcurrency -= _turretData.costToBuild;
        return true;
    }

    public string GetCurrencyText()
    {
        return currentcurrency.ToString();
    }

    public string GetZomnCurrencyText()
    {
        return currentZomnCurrency.ToString();
    }
}
