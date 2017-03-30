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

    void OnEnable()
    {
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

    public void ShowUI(bool _show)
    {
        gameObject.SetActive(_show);
        isShowing = _show;   
    }
}
