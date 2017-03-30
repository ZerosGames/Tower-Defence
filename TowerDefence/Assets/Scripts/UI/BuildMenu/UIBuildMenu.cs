using UnityEngine;
using UnityEngine.UI;

public class UIBuildMenu : MonoBehaviour {

    [SerializeField]
    private UITurretBuildMenu turretbuildMenu;
    private bool showTurretUI = false;

    [SerializeField]
    private UIBuildingBuildMenu buildingsbuildMenu;
    private bool showBuildingsUI = false;

    public void OnClickedTurretBuildOption()
    {
        if(buildingsbuildMenu.isShowing())
        {
            showBuildingsUI = !showBuildingsUI;
            buildingsbuildMenu.ShowUI(showBuildingsUI);
        }

        showTurretUI = !showTurretUI;
        turretbuildMenu.ShowUI(showTurretUI);
    }

    public void OnClickedBuildingsBuildOption()
    {
        if (turretbuildMenu.isShowing())
        {
            showTurretUI = !showTurretUI;
            turretbuildMenu.ShowUI(showTurretUI);
        }

        showBuildingsUI = !showBuildingsUI;
        buildingsbuildMenu.ShowUI(showBuildingsUI);
    }
}
