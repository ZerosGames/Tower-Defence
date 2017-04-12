using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class PlacementController : MonoBehaviour {

    BuildManager buildManager;
    public LayerMask rayMask;
    private GameObject turretToPlace;

    private GameObject buildingToPlace;

    [SerializeField]
    private GameObject turretsRoot;

    [SerializeField]
    private GameObject buildingsRoot;

    public MapGenerator mapGrid;

    private bool placementBlocked = false;

    [SerializeField]
    private UIManager uiManager;

    void Start()
    {
        buildManager = BuildManager.Instance;
    }

    void Update()
    {
        if (buildManager.GetBuildingMode() && buildManager.GetTurretToBuild() != null && buildManager.GetBuildingMode() && buildManager.GetBuildingToBuild() == null)
        {
            if(turretToPlace == null)
            {
                turretToPlace = Instantiate(buildManager.GetTurretToBuild(), new Vector3(0, -10, 0), Quaternion.identity) as GameObject;
            }

            HandlePlacingTurret(turretToPlace);
        }
        else if(turretToPlace != null)
        {
            CancelTurretPlacement();
        }

        if (buildManager.GetBuildingMode() && buildManager.GetTurretToBuild() == null && buildManager.GetBuildingMode() && buildManager.GetBuildingToBuild() != null)
        {
            if (buildingToPlace == null)
            {
                buildingToPlace = Instantiate(buildManager.GetBuildingToBuild(), new Vector3(0, -10, 0), Quaternion.identity) as GameObject;
            }

            HandlePlacingBuilding(buildingToPlace);
        }
        else if (buildingToPlace != null)
        {
            CancelBuildingPlacement();
        }
    }

    public void HandlePlacingBuilding(GameObject _buildingToPlace)
    {
        RaycastHit hitInfo;

        if (Input.GetMouseButton(1))
        {
            CancelBuildingPlacement();
        }

        if (getRayPointFromCameraAndObject(out hitInfo, rayMask) && !EventSystem.current.IsPointerOverGameObject())
        {
            Node hitNode = NodeGrid.NodeFromWorldPos(hitInfo.point, mapGrid.grid, mapGrid.MapWidth, mapGrid.MapWidth, mapGrid.GridSizeX, mapGrid.GridSizeY);

            if (hitNode.CrystalNode == true && hitNode != null)
            {
                Vector3 PlacementVector = new Vector3(hitNode.WorldPos.x, hitInfo.point.y, hitNode.WorldPos.z);

                _buildingToPlace.transform.position = PlacementVector;

                if (InputManager.GetMouseButtonLeftPressed() && !placementBlocked)
                {
                    if (InputManager.GetShiftButtonPressed())
                    {
                        if (PlayerData.playerData.purchaseBuilding(buildingToPlace.GetComponent<CrystalMine>().GetBuildingData()))
                        {
                            shiftPlaceBuiling(hitNode);
                            hitNode.Placeable = false;
                        }
                    }
                    else
                    {
                        if (PlayerData.playerData.purchaseBuilding(buildingToPlace.GetComponent<CrystalMine>().GetBuildingData()))
                        {
                            placeBuiling(hitNode);
                            hitNode.Placeable = false;
                        }
                    }
                }
            }
            else
            {
                _buildingToPlace.transform.position = new Vector3(0, -10, 0);
            }
        }
        else
        {
            _buildingToPlace.transform.position = new Vector3(0, -10, 0);
        }
    }

    public void HandlePlacingTurret(GameObject _turretToPlace)
    {
        RaycastHit hitInfo;
  
        if (Input.GetMouseButton(1))
        {
            CancelTurretPlacement();
        }

        if (getRayPointFromCameraAndObject(out hitInfo, rayMask) && !EventSystem.current.IsPointerOverGameObject())
        {
            Node hitNode = NodeGrid.NodeFromWorldPos(hitInfo.point, mapGrid.grid, mapGrid.MapWidth, mapGrid.MapWidth, mapGrid.GridSizeX, mapGrid.GridSizeY);

            if (hitNode.Placeable == true && hitNode != null)
            {

                Vector3 PlacementVector = new Vector3(hitNode.WorldPos.x, hitInfo.point.y, hitNode.WorldPos.z);

                _turretToPlace.transform.position = PlacementVector;

                if (InputManager.GetMouseButtonLeftPressed() && !placementBlocked)
                {
                    if (InputManager.GetShiftButtonPressed())
                    {
                        if (PlayerData.playerData.purchaseTurret(turretToPlace.GetComponent<TurretBase>().GetTurretData()))
                        {
                            ShiftPlaceTurret(hitNode);
                            hitNode.Placeable = false;
                        }
                    }
                    else
                    {
                        if (PlayerData.playerData.purchaseTurret(turretToPlace.GetComponent<TurretBase>().GetTurretData()))
                        {
                            placeTurret(hitNode);
                            hitNode.Placeable = false;
                        }
                    }
                }
            }
            else
            {
                _turretToPlace.transform.position = new Vector3(0, -10, 0);
            }
        }
        else
        {
            _turretToPlace.transform.position = new Vector3(0, -10, 0);
        }
    }

    public void placeTurret(Node _node)
    {
        TurretBase TC = turretToPlace.GetComponent<TurretBase>();
        TC.SetPlaced(true);
        TC.SetPlacementNode(_node);
        References.turretPlaced.Add(turretToPlace);
        turretToPlace.transform.parent = turretsRoot.transform;
        turretToPlace = null;
        buildManager.SetTurretToBuild(null);
    }

    public void placeBuiling(Node _node)
    {
        CrystalMine TC = buildingToPlace.GetComponent<CrystalMine>();
        References.buildingsPlaced.Add(buildingToPlace);
        buildingToPlace.transform.parent = buildingsRoot.transform;
        buildingToPlace = null;
        buildManager.SetBuildingToBuild(null);
        TC.StartMining();
        uiManager.SetZomnIncomeText(TC.GetBuildingData().zonmCrystalPerSecond);
    }

    public void shiftPlaceBuiling(Node _node)
    {
        CrystalMine TC = buildingToPlace.GetComponent<CrystalMine>();
        References.buildingsPlaced.Add(buildingToPlace);
        buildingToPlace.transform.parent = buildingsRoot.transform;
        buildingToPlace = null;
        TC.StartMining();
        uiManager.SetZomnIncomeText(TC.GetBuildingData().zonmCrystalPerSecond);
        buildingToPlace = Instantiate(buildManager.GetBuildingToBuild(), new Vector3(0, -10, 0), Quaternion.identity) as GameObject;
    }

    public void ShiftPlaceTurret(Node _node)
    {
        TurretBase TC = turretToPlace.GetComponent<TurretBase>();
        TC.SetPlaced(true);
        TC.SetPlacementNode(_node);
        References.turretPlaced.Add(turretToPlace);
        turretToPlace.transform.parent = turretsRoot.transform;
        turretToPlace = null;
        turretToPlace = Instantiate(buildManager.GetTurretToBuild(), new Vector3(0, -10, 0), Quaternion.identity) as GameObject;
    }

    public void CancelTurretPlacement()
    {
        Destroy(turretToPlace);
        turretToPlace = null;
        buildManager.SetTurretToBuild(null);
    }

    public void CancelBuildingPlacement()
    {
        Destroy(buildingToPlace);
        buildingToPlace = null;
        buildManager.SetBuildingToBuild(null);
    }

    bool getRayPointFromCameraAndObject(out RaycastHit _HitNode, LayerMask _mask)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo = new RaycastHit();
        if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity, _mask))
        {
            if (hitInfo.collider.gameObject.layer == 9)
            {
                Node hitNode = NodeGrid.NodeFromWorldPos(hitInfo.point, mapGrid.grid, mapGrid.MapWidth, mapGrid.MapWidth, mapGrid.GridSizeX, mapGrid.GridSizeY);

                _HitNode = hitInfo;
                return true;
            }

            _HitNode = hitInfo;
            return false;
        }

        _HitNode = hitInfo;
        return false;
    }
}
