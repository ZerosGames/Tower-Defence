using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    private Vector3 velocity;
    private Vector3 rotation;
    private Vector3 camRotation;
    [SerializeField]
    private Camera cam;

    private Rigidbody rb;
	// Use this for initialization
	void Start () {

        rb = GetComponent<Rigidbody>();
	}

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!GameManager.gameManager.GetPause())
        {
            UpdateMovement();
            UpdateRotation();
        }
    }

    public void Move(Vector3 _velocity)
    {
        velocity = _velocity;
    }

    public void Rotate(Vector3 _rotation)
    {
        rotation = _rotation;
    }

    public void RotateCamera(Vector3 _camRotation)
    {
        camRotation = _camRotation;
    }

    void UpdateRotation()
    {
        rb.MoveRotation(rb.rotation * Quaternion.Euler(rotation));

        if (cam != null)
        {
            cam.transform.Rotate(-camRotation);
        }
    }

    void UpdateMovement()
    {
        rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
    }
}
