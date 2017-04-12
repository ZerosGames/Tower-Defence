using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITurretBuildMenu : MonoBehaviour {

    private Animator Anim;

    BuildManager buildManager;

    [SerializeField]
    private Text[] costDisplays;

    private bool IsShowing = false;

    [SerializeField]
    private UITurretStats SelectedTurretStats;

    // Use this for initialization
    void Start ()
    {
        Anim = GetComponent<Animator>();
        buildManager = BuildManager.Instance;

        SetCostDisplays();
    }

    public void ShowUI(bool _show)
    {
        IsShowing = _show;
        BuildManager.Instance.SetBuildMode(_show);
        Anim.SetBool("inBuildingMode", _show);
    }

    public void turretSelected(int _type)
    {
        
        buildManager.SetTurretToBuild(References.Refs.GetTurret(_type));
    }

    void SetCostDisplays()
    {
        for (int i = 0; i < costDisplays.Length; i++)
        {
            costDisplays[i].text = References.Refs.GetTurretData(i).costToBuild.ToString();
        }
    }

    public bool isShowing()
    {
        return IsShowing;
    }

    public void OnShowTurretToolTip(int _turretElementIndex)
    {
        if (_turretElementIndex < References.Refs.turretTypes.Length)
        {
            GameObject TurretToBuild = References.Refs.turretTypes[_turretElementIndex];

            if (TurretToBuild)
            {
                TurretBase TurretToBuildController = TurretToBuild.GetComponent<TurretBase>();

                SelectedTurretStats.Setdamage(TurretToBuildController.GetTurretData().tDamage);
                SelectedTurretStats.SetFireRate(TurretToBuildController.GetTurretData().tFireRate);
                SelectedTurretStats.SetArmourPenetration(TurretToBuildController.GetTurretData().tPenatration);
                SelectedTurretStats.SetShieldPenetration(TurretToBuildController.GetTurretData().tPenatration);
                SelectedTurretStats.SetTurretRange(TurretToBuildController.GetTurretData().tRange);

                SelectedTurretStats.ShowUI(true);
            }
        }
    } 

    public void OnStopShowingTurretToolTip(int _turretElementIndex)
    {
        SelectedTurretStats.ShowUI(false);
    }
}
