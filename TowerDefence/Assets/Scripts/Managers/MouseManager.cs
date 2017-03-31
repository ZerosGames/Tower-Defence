using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseManager : MonoBehaviour
{
    private GameObject hitObject;
    [SerializeField]
    private UITurretUpgrade UpgradeMenu;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (InputManager.GetMouseButtonLeftPressed() && !BuildManager.BuildManagerInstance.GetBuildingMode() && !EventSystem.current.IsPointerOverGameObject())
        {
            if(FireRayCast(1<<10, out hitObject))
            {
                TurretController TC = hitObject.GetComponent<TurretController>();

                if(TC != null)
                {
                    TurretUpgradeController.TUC.SetSelectedController(TC);
                    UpgradeMenu.ShowUI(true);
                }
            }
            else
            {
                if(TurretUpgradeController.TUC.GetSelectedController())
                {
                    TurretUpgradeController.TUC.SetSelectedController(null);
                    UpgradeMenu.ShowUI(false);
                }
            }
        }
        else if(InputManager.GetMouseButtonRightPressed() && !BuildManager.BuildManagerInstance.GetBuildingMode() && !EventSystem.current.IsPointerOverGameObject())
        {
            TurretUpgradeController.TUC.SetSelectedController(null);
            UpgradeMenu.ShowUI(false);
        }
    }

    bool FireRayCast(LayerMask _mask, out GameObject _hitObject)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo = new RaycastHit();

        if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity, _mask))
        {
            _hitObject = hitInfo.transform.gameObject;
            return true;
        }

        _hitObject = null;
        return false;
    }
}
