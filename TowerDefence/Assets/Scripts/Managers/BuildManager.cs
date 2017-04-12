using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BuildManager : MonoBehaviour {

    public static BuildManager Instance;

    private GameObject turretToBuild;

    private GameObject buildingToBuild;

    private bool BuildMode = false;

    void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("More Than One Buildmanager in Scene");
            return;
        }

        Instance = this;    
    }

    void Update()
    {
        if (!GameManager.gameManager.GetPause())
        {
            if (Input.GetButtonDown("Build"))
            {
                SetBuildMode(!BuildMode);
            }
        }
        else
        {
            if(BuildMode)
            {
                BuildMode = false;
            }
        }
    }

    public GameObject GetTurretToBuild()
    {
        return turretToBuild;
    }

    public GameObject GetBuildingToBuild()
    {
        return buildingToBuild;
    }

    public void SetTurretToBuild(GameObject _turretToBuild)
    {
        turretToBuild = _turretToBuild;
    }

    public void SetBuildingToBuild(GameObject _buildingToBuild)
    {
        buildingToBuild = _buildingToBuild;
    }

    public void SetBuildMode(bool _buildMode)
    {
        BuildMode = _buildMode;
    }

    public bool GetBuildingMode()
    {
        return BuildMode;
    }  
}
