using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Motor : MonoBehaviour {

    public float moveSpeed = 7.5f;
    public float drag = 0.5f;
    public float terminalRotationSpeed = 25.0f;
    public VirtualJoyStick moveJoystick;
    public float boostSpeed = 7.0f;
    public float boostCooldown = 2.0f;


    private float lastBoost;
    private Rigidbody controller; //reference to rigidbody
    private Transform camTransform;
    private void Start()
    {
        lastBoost = Time.time - boostCooldown;
        controller = GetComponent<Rigidbody>();
        controller.maxAngularVelocity = terminalRotationSpeed;
        controller.drag = drag;

        camTransform = Camera.main.transform; //gets active of main camera tag and fetches the transform properties
    }

    private void Update()
    {
        //if keys pressed, store in vector3
        Vector3 direction = Vector3.zero;
        direction.x = Input.GetAxis("Horizontal");
        direction.z = Input.GetAxis("Vertical");

        if (direction.magnitude >1)
            direction.Normalize();

        //override keyboard input for joystick
        if (moveJoystick.InputDirection != Vector3.zero)
        {
            direction = moveJoystick.InputDirection;
        }

        //Rotate our direction vector with the camera
        Vector3 rotatedDir = camTransform.TransformDirection(direction);
        //remove the y component which affects the camera tilt
        rotatedDir = new Vector3(rotatedDir.x, 0 , rotatedDir.z);
        rotatedDir = rotatedDir.normalized * direction.magnitude;

        controller.AddForce(rotatedDir * moveSpeed);
    }

    public void Boost()
    {
        if (Time.time - lastBoost > boostCooldown)
        {
            lastBoost = Time.time;
            controller.AddForce(controller.velocity.normalized * boostSpeed, ForceMode.VelocityChange);
        }
    }
}
