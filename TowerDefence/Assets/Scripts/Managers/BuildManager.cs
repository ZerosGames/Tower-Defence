using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BuildManager : MonoBehaviour {

    public static BuildManager BuildManagerInstance;

    private GameObject turretToBuild;
    private bool BuildMode = false;

    void Awake()
    {
        if (BuildManagerInstance != null)
        {
            Debug.LogError("More Than One Buildmanager in Scene");
            return;
        }

        BuildManagerInstance = this;    
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

    public void SetBuildMode(bool _buildMode)
    {
        BuildMode = _buildMode;
    }

    public void SetTurretToBuild(GameObject _turretToBuild)
    {
        turretToBuild = _turretToBuild;
    }

    public bool GetBuildingMode()
    {
        return BuildMode;
    }  
}
