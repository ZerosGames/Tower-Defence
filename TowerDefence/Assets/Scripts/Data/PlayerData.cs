using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour {

    public int currentcurrency;
    public int startingCurrency = 300;

    public int currentZomnCurrency;
    public int startingZomnCurrency = 0;

	public void InitData ()
    {
        currentcurrency = startingCurrency;
        currentZomnCurrency = startingZomnCurrency;
    }

	void Update ()
    {
		
	}

    public bool purchaseTurretUpgrade(TurretUpgradeTypes types, tData _turretData)
    {
        switch (types)
        {
            case TurretUpgradeTypes.Damage:
                if(currentZomnCurrency - _turretData.costToUpgradeDamage < 0)
                {
                    return false;
                }

                currentZomnCurrency -= _turretData.costToUpgradeDamage;
                break;
            case TurretUpgradeTypes.FireRate:
                if (currentZomnCurrency - _turretData.costToUpgradeFirerate < 0)
                {
                    return false;
                }

                currentZomnCurrency -= _turretData.costToUpgradeFirerate;
                break;
            case TurretUpgradeTypes.Range:
                if (currentZomnCurrency - _turretData.costToUpgradeRange < 0)
                {
                    return false;
                }

                currentZomnCurrency -= _turretData.costToUpgradeRange;
                break;
            default:
                break;
        }

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

    public bool purchaseBuilding(bData _buildingData)
    {
        if (currentcurrency - _buildingData.costToBuild < 0)
        {
            return false;
        }

        currentcurrency -= _buildingData.costToBuild;
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
