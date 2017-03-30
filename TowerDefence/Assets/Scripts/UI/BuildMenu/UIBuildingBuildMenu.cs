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
        buildManager = BuildManager.BuildManagerInstance;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ShowUI(bool _show)
    {
        IsShowing = _show;
        BuildManager.BuildManagerInstance.SetBuildMode(_show);
        Anim.SetBool("inBuildingMode", _show);
    }

    public void turretSelected(int _type)
    {
        buildManager.SetTurretToBuild(References.Refs.GetTurret(_type));
    }

    void SetCostDisplays()
    {
        for (int i = 0; i < costDisplays.Length; i++)
        {
            costDisplays[i].text = References.Refs.GetTurretData(i).costToBuild.ToString();
        }
    }

    public bool isShowing()
    {
        return IsShowing;
    }
}
