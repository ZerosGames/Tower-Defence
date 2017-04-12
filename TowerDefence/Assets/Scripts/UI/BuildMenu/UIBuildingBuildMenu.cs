using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBuildingBuildMenu : MonoBehaviour {

    private Animator Anim;

    BuildManager buildManager;

    [SerializeField]
    private Text[] costDisplays;

    private bool IsShowing = false;

    // Use this for initialization
    void Start()
    {

        Anim = GetComponent<Animator>();
        buildManager = BuildManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ShowUI(bool _show)
    {
        IsShowing = _show;
        BuildManager.Instance.SetBuildMode(_show);
        Anim.SetBool("inBuildingMode", _show);
    }

    public void buildingSelected(int _type)
    {
        buildManager.SetBuildingToBuild(References.Refs.GetBuilding(_type));
    }

    public bool isShowing()
    {
        return IsShowing;
    }
}
