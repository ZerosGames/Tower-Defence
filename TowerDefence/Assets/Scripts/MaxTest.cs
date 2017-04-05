using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaxTest : MonoBehaviour {

    public GameObject Turret;

	// Use this for initialization
	public void SpawnTurrets () {

        foreach (Node n in VectorFieldGrid.nodeGrid.Grid)
        {
            if (n.Placeable)
            {
                //Instantiate(Turret, n.WorldPos, Quaternion.identity);
            }
        }
	}
}
