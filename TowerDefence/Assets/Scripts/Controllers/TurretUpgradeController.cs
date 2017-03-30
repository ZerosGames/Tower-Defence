using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretUpgradeController : MonoBehaviour {

    public static TurretUpgradeController TUC;
    private TurretController SelectedTurret;

    void Awake()
    {
        if(TUC != null)
        {
            Debug.LogError("Two TUC's in scene");
            return;
        }

        TUC = this;
    }
	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {		

	}

    public bool isTurretSelected()
    {
        return (SelectedTurret == null) ? false : true; 
    }

    public TurretController GetSelectedController()
    {
        return SelectedTurret;
    }

    public void SetSelectedController(TurretController _Controller)
    {
        SelectedTurret = _Controller;
    }

    public void purchaseUpgrade()
    {

    }
}
