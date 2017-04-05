using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseManager : MonoBehaviour
{
    private GameObject ThirdPersonhitObject;
    [SerializeField]
    private UITurretUpgrade UpgradeMenu;

    enum MouseInputStates
    {
        ThirdPersonMode,
        BuildingMode
    }

    MouseInputStates MInputStates;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(BuildManager.BuildManagerInstance.GetBuildingMode())
        {
            MInputStates = MouseInputStates.BuildingMode;
        }
        else
        {
            MInputStates = MouseInputStates.ThirdPersonMode;
        }

        switch (MInputStates)
        {
            case MouseInputStates.ThirdPersonMode:
                HandleThirdPersonMInput();
                break;
            case MouseInputStates.BuildingMode:
                break;
            default:
                break;
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

    void HandleThirdPersonMInput()
    {
        if (InputManager.GetMouseButtonLeftPressed() && !EventSystem.current.IsPointerOverGameObject())
        {
            if (FireRayCast(1 << 10, out ThirdPersonhitObject))
            {
                TurretController TC = ThirdPersonhitObject.GetComponent<TurretController>();

                if (TC != null)
                {
                    UpgradeMenu.ShowUI(true);
                    UpgradeMenu.SetSelectedTC(TC);
                    
                }
            }
            else
            {
                UpgradeMenu.ShowUI(false);
                UpgradeMenu.SetSelectedTC(null);
            }
        }
        else if (InputManager.GetMouseButtonRightPressed() && !EventSystem.current.IsPointerOverGameObject())
        {
            UpgradeMenu.ShowUI(false);
            UpgradeMenu.SetSelectedTC(null);

        }
    }
}
