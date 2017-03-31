using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITurretUpgrade : MonoBehaviour
{
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
        if (TurretUpgradeController.TUC.GetSelectedController().GetTurretData().tDamage >= TurretUpgradeController.TUC.GetSelectedController().GetTurretData().tMaxDamage)
        {
            DamageUgradeButton.interactable = false;
        }

        if (TurretUpgradeController.TUC.GetSelectedController().GetTurretData().tFireRate >= TurretUpgradeController.TUC.GetSelectedController().GetTurretData().tMaxFireRate)
        {
            FireRateUpgradeButton.interactable = false;
        }

        if (TurretUpgradeController.TUC.GetSelectedController().GetTurretData().tRange >= TurretUpgradeController.TUC.GetSelectedController().GetTurretData().tMaxRange)
        {
            RangeUpgradeButton.interactable = false;
        }

        DamageUpgradeCostText.text = TurretUpgradeController.TUC.GetSelectedController().GetTurretData().costToUpgradeDamage.ToString();
        RangeUpgradeCostText.text = TurretUpgradeController.TUC.GetSelectedController().GetTurretData().costToUpgradeRange.ToString();
        FireRateUpgradeCostText.text = TurretUpgradeController.TUC.GetSelectedController().GetTurretData().costToUpgradeFirerate.ToString();

        SellTurret.text = TurretUpgradeController.TUC.GetSelectedController().GetTurretData().SellTurretCost.ToString();

        UpdateUI();
    }

    void UpdateUI()
    {
        DamageText.text = TurretUpgradeController.TUC.GetSelectedController().GetTurretData().tDamage.ToString();
        FireRateText.text = TurretUpgradeController.TUC.GetSelectedController().GetTurretData().tFireRate.ToString();
        RangeText.text = TurretUpgradeController.TUC.GetSelectedController().GetTurretData().tRange.ToString();
    }
       
    public void UpgradeTurretDamage()
    {
        if (PlayerData.playerData.purchaseTurretUpgrade(TurretController.TurretUpgradeTypes.Damage, TurretUpgradeController.TUC.GetSelectedController().GetTurretData()))
        {
            TurretUpgradeController.TUC.GetSelectedController().UpgradeTurret(TurretController.TurretUpgradeTypes.Damage);
            UpdateUI();
        }
    }

    public void UpgradeTurretFireRate()
    {
        if (PlayerData.playerData.purchaseTurretUpgrade(TurretController.TurretUpgradeTypes.FireRate, TurretUpgradeController.TUC.GetSelectedController().GetTurretData()))
        {
            TurretUpgradeController.TUC.GetSelectedController().UpgradeTurret(TurretController.TurretUpgradeTypes.FireRate);
            UpdateUI();
        }
    }

    public void UpgradeTurretRange()
    {
        if (PlayerData.playerData.purchaseTurretUpgrade(TurretController.TurretUpgradeTypes.Range, TurretUpgradeController.TUC.GetSelectedController().GetTurretData()))
        {
            TurretUpgradeController.TUC.GetSelectedController().UpgradeTurret(TurretController.TurretUpgradeTypes.Range);
            UpdateUI();
        }
    }

    public void OnSellTurret()
    {
        TurretUpgradeController.TUC.GetSelectedController().SellTurret();
        ShowUI(false);
    }

    void ResetUI()
    {
        DamageText.text = "0";
        FireRateText.text = "0";
        RangeText.text = "0";

        DamageUgradeButton.interactable = true;
        FireRateUpgradeButton.interactable = true;
        RangeUpgradeButton.interactable = true;
    }

    public void ShowUI(bool _show)
    {
        if(_show == true)
        {
            Vector2 pos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(myCanvas.transform as RectTransform, Input.mousePosition, myCanvas.worldCamera, out pos);
            transform.position = myCanvas.transform.TransformPoint(pos);
        }

        ResetUI();
        gameObject.SetActive(_show);
        isShowing = _show;

    }
}
