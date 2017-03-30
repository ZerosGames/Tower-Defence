using UnityEngine;
using UnityEngine.EventSystems;


public class UIHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private UITurretBuildMenu turretBuildMenu;

    [SerializeField]
    private int turretElementIndex;

    public void OnPointerEnter(PointerEventData eventData)
    {
        turretBuildMenu.OnShowTurretToolTip(turretElementIndex);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        turretBuildMenu.OnStopShowingTurretToolTip(turretElementIndex);
    }
}
