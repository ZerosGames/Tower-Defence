using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class PlacementController : MonoBehaviour {

    BuildManager buildManager;
    public LayerMask rayMask;
    private GameObject turretToPlace;

    public NodeGrid mapGrid;

    private bool placementBlocked = false;

    void Start()
    {
        buildManager = BuildManager.BuildManagerInstance;
    }

    void Update()
    {
        if (buildManager.GetBuildingMode() && buildManager.GetTurretToBuild() != null)
        {
            if(turretToPlace == null)
            {
                turretToPlace = Instantiate(buildManager.GetTurretToBuild(), new Vector3(0, -10, 0), Quaternion.identity) as GameObject;
            }

            HandlePlacingTurret(turretToPlace);
        }
        else if(turretToPlace != null)
        {
            CancelPlacement();
        }
    }

    public void HandlePlacingTurret(GameObject _turretToPlace)
    {
        Vector3 hitPoint;
  
        if (Input.GetMouseButton(1))
        {
            CancelPlacement();
        }

        if (getRayPointFromCameraAndObject(out hitPoint, rayMask) && !EventSystem.current.IsPointerOverGameObject())
        {
            _turretToPlace.transform.position = hitPoint;

            if (InputManager.GetMouseButtonLeftPressed() && !placementBlocked)
            {
                if (InputManager.GetShiftButtonPressed())
                {
                    if (PlayerData.playerData.purchaseTurret(turretToPlace.GetComponent<TurretController>().GetTurretData()))
                    {
                        ShiftPlaceTurret();
                        mapGrid.NodeFromWorldPos(hitPoint).Placeable = false;
                    }
                }
                else
                {
                    if (PlayerData.playerData.purchaseTurret(turretToPlace.GetComponent<TurretController>().GetTurretData()))
                    {
                        placeTurret();
                        mapGrid.NodeFromWorldPos(hitPoint).Placeable = false;
                    }
                }
            }
        }
        else
        {
            _turretToPlace.transform.position = hitPoint;
        }
    }

    void placeTurret()
    {
        TurretController TC = turretToPlace.GetComponent<TurretController>();
        TC.SetPlaced(true);
        References.turretPlaced.Add(turretToPlace);
        turretToPlace = null;
        buildManager.SetTurretToBuild(null);
    }

    void ShiftPlaceTurret()
    {
        TurretController TC = turretToPlace.GetComponent<TurretController>();
        TC.SetPlaced(true);
        References.turretPlaced.Add(turretToPlace);
        turretToPlace = null;
        turretToPlace = Instantiate(buildManager.GetTurretToBuild(), new Vector3(0, -10, 0), Quaternion.identity) as GameObject;
    }

    void CancelPlacement()
    {
        Destroy(turretToPlace);
        turretToPlace = null;
        buildManager.SetTurretToBuild(null);
    }

    bool getRayPointFromCameraAndObject(out Vector3 _point, LayerMask _mask)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo = new RaycastHit();
        if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity, _mask))
        {
            if (hitInfo.collider.gameObject.layer == 9)
            {
                Node hitNode = mapGrid.NodeFromWorldPos(hitInfo.point);

                if(hitNode.Placeable)
                {
                    placementBlocked = false;
                    _point = hitNode.WorldPos;
                    return true;
                }

                _point = new Vector3 (hitNode.WorldPos.x, 1, hitNode.WorldPos.z);
                placementBlocked = false;
                return false;
            }
        }
        _point = new Vector3(0, -10, 0);
        return false;
    }
}
