using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITurretUpgrade : MonoBehaviour
{
    private TurretController SelectedTC;
    private tData SelectedTCData;

    public bool isShowing = false;
    public RectTransform CanvasRect;

    public Text DamageText;
    public Text FireRateText;
    public Text RangeText;

    public Button DamageUgradeButton;
    public Button FireRateUpgradeButton;
    public Button RangeUpgradeButton;

    public Text DamageUpgradeCostText;
    public Text RangeUpgradeCostText;
    public Text FireRateUpgradeCostText;

    public Text SellTurret;

    public Canvas myCanvas;

    void Update()
    {
        if (SelectedTC != null)
        {
            SelectedTCData = SelectedTC.GetTurretData();

            

            UpdateUI();
        }
    }

    void UpdateUI()
    {
        if (SelectedTCData.tDamage >= SelectedTCData.tMaxDamage)
        {
            DamageUgradeButton.interactable = false;
        }
        else
        {
            DamageUgradeButton.interactable = true;
        }

        if (SelectedTCData.tFireRate >= SelectedTCData.tMaxFireRate)
        {
            FireRateUpgradeButton.interactable = false;
        }
        else
        {
            FireRateUpgradeButton.interactable = true;
        }

        if (SelectedTCData.tRange >= SelectedTCData.tMaxRange)
        {
            RangeUpgradeButton.interactable = false;
        }
        else
        {
            RangeUpgradeButton.interactable = true;
        }

        DamageUpgradeCostText.text = SelectedTCData.costToUpgradeDamage.ToString();
        RangeUpgradeCostText.text = SelectedTCData.costToUpgradeRange.ToString();
        FireRateUpgradeCostText.text = SelectedTCData.costToUpgradeFirerate.ToString();

        SellTurret.text = SelectedTCData.SellTurretCost.ToString();

        DamageText.text = SelectedTCData.tDamage.ToString();
        FireRateText.text = SelectedTCData.tFireRate.ToString();
        RangeText.text = SelectedTCData.tRange.ToString();
    }
       
    public void UpgradeTurretDamage()
    {
        if (PlayerData.playerData.purchaseTurretUpgrade(TurretController.TurretUpgradeTypes.Damage, SelectedTCData))
        {
            SelectedTC.UpgradeTurret(TurretController.TurretUpgradeTypes.Damage);
            UpdateUI();
        }
    }

    public void UpgradeTurretFireRate()
    {
        if (PlayerData.playerData.purchaseTurretUpgrade(TurretController.TurretUpgradeTypes.FireRate, SelectedTCData))
        {
            SelectedTC.UpgradeTurret(TurretController.TurretUpgradeTypes.FireRate);
            UpdateUI();
        }
    }

    public void UpgradeTurretRange()
    {
        if (PlayerData.playerData.purchaseTurretUpgrade(TurretController.TurretUpgradeTypes.Range, SelectedTCData))
        {
            SelectedTC.UpgradeTurret(TurretController.TurretUpgradeTypes.Range);
            UpdateUI();
        }
    }

    public void OnSellTurret()
    {
        SelectedTC.SellTurret();
        ShowUI(false);
    }

    public void ShowUI(bool _show)
    {
        if(_show == true)
        {
            Vector2 pos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(myCanvas.transform as RectTransform, Input.mousePosition, myCanvas.worldCamera, out pos);
            transform.position = myCanvas.transform.TransformPoint(pos);
        }

        gameObject.SetActive(_show);
        isShowing = _show;
    }

    public void SetSelectedTC(TurretController _tc)
    {
        SelectedTC = _tc;
    }
}
