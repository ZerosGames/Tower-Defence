using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    [SerializeField]
    private float Speed;

    [SerializeField]
    private float lookSensitivity;

    private Player player;

	// Use this for initialization
	void Start () {

        player = GetComponent<Player>();
	}
	
	// Update is called once per frame
	void Update () {

        float xRawMov = Input.GetAxisRaw("Horizontal");
        float zRawMov = Input.GetAxisRaw("Vertical");

        Vector3 movHorizontal = transform.right * xRawMov;
        Vector3 movVertical = transform.forward * zRawMov;

        Vector3 velocity = (movHorizontal + movVertical).normalized * Speed;

        player.Move(velocity);

        float yRotRaw = Input.GetAxisRaw("Mouse X");

        Vector3 rotation = new Vector3(0, yRotRaw, 0) * lookSensitivity;

        player.Rotate(rotation);

        float xRotRaw = Input.GetAxisRaw("Mouse Y");

        Vector3 camRotation = new Vector3(xRotRaw, 0, 0) * lookSensitivity;

        player.RotateCamera(camRotation);
    }
}
