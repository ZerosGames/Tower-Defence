using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class PlacementController : MonoBehaviour {

    BuildManager buildManager;
    public LayerMask rayMask;
    private GameObject turretToPlace;

    public MapGenerator mapGrid;

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
        Node hitNode;
  
        if (Input.GetMouseButton(1))
        {
            CancelPlacement();
        }

        if (getRayPointFromCameraAndObject(out hitNode, rayMask) && !EventSystem.current.IsPointerOverGameObject())
        {
            _turretToPlace.transform.position = hitNode.WorldPos;
            _turretToPlace.transform.position += Vector3.up;

            if (InputManager.GetMouseButtonLeftPressed() && !placementBlocked)
            {
                if (InputManager.GetShiftButtonPressed())
                {
                    if (PlayerData.playerData.purchaseTurret(turretToPlace.GetComponent<TurretController>().GetTurretData()))
                    {
                        ShiftPlaceTurret(hitNode);
                        hitNode.Placeable = false;
                    }
                }
                else
                {
                    if (PlayerData.playerData.purchaseTurret(turretToPlace.GetComponent<TurretController>().GetTurretData()))
                    {
                        placeTurret(hitNode);
                        hitNode.Placeable = false;
                    }
                }
            }
        }
        else
        {
            _turretToPlace.transform.position = Vector3.zero;
        }
    }

    public void placeTurret(Node _node)
    {
        TurretController TC = turretToPlace.GetComponent<TurretController>();
        TC.SetPlaced(true);
        TC.SetPlacementNode(_node);
        References.turretPlaced.Add(turretToPlace);
        turretToPlace = null;
        buildManager.SetTurretToBuild(null);
    }

    public void ShiftPlaceTurret(Node _node)
    {
        TurretController TC = turretToPlace.GetComponent<TurretController>();
        TC.SetPlaced(true);
        TC.SetPlacementNode(_node);
        References.turretPlaced.Add(turretToPlace);
        turretToPlace = null;
        turretToPlace = Instantiate(buildManager.GetTurretToBuild(), new Vector3(0, -10, 0), Quaternion.identity) as GameObject;
    }

    public void CancelPlacement()
    {
        Destroy(turretToPlace);
        turretToPlace = null;
        buildManager.SetTurretToBuild(null);
    }

    bool getRayPointFromCameraAndObject(out Node _HitNode, LayerMask _mask)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo = new RaycastHit();
        if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity, _mask))
        {
            if (hitInfo.collider.gameObject.layer == 9)
            {
                Node hitNode = NodeGrid.NodeFromWorldPos(hitInfo.point, mapGrid.grid, mapGrid.MapWidth, mapGrid.MapWidth, mapGrid.GridSizeX, mapGrid.GridSizeY);

                if(hitNode.Placeable)
                {
                    placementBlocked = false;
                    _HitNode = hitNode;
                    return true;
                }

                _HitNode = hitNode;
                placementBlocked = false;
                return false;
            }
        }
        _HitNode = null;
        return false;
    }
}
