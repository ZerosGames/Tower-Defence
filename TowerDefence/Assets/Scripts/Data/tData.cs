using UnityEngine;
using System.Collections;

[System.Serializable]
public class tData
{

    public float tRange;
    public float tMaxRange;
    public float tRangePreUpgrade;

    public float tTurnSpeed;

    public float tFireRate;
    public float tMaxFireRate;
    public float tFireRatePreUpgrade;

    public int tDamage;
    public int tMaxDamage;
    public int tDamagePreUpgrade;

    public int tPenatration;

    public int layerMask;

    public float fireRateCoolDown;

    public int costToBuild;
    public int costToUpgradeDamage;
    public int costToUpgradeFirerate;
    public int costToUpgradeRange;

    public int costPerUpgrade;

    public int SellTurretCost;
}


public enum TurretUpgradeTypes
{
    Damage,
    FireRate,
    Range
}


