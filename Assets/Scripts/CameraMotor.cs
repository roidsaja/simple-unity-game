using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMotor : MonoBehaviour {

    public Transform lookAt;
    private Vector3 offset;
    private Vector3 desiredPosition;

    private Vector2 touchPosition;
    private float swipeResistance = 200.0f;

    private float smoothSpeed = 7.5f;
    private float distance = 5.0f; //distance between camera and user
    private float yOffset = 3.5f;

    private void Start()
    {
        offset = new Vector3(0, yOffset, -1f * distance); //place camera in the back of the ball
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
            SlideCamera(true);
        else if (Input.GetKeyDown(KeyCode.RightArrow))
            SlideCamera(false);

        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))  //left click or first touch of a touchscreen
        {
            touchPosition = Input.mousePosition; //record position of where we are clicking
        }

        if (Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1)) //whenever we release touchscreen or mouse
        {
            float swipeForce = touchPosition.x - Input.mousePosition.x;
            if (Mathf.Abs(swipeForce)> swipeResistance) //return us the absolute of swipe force
            {
                //if greater than resistance, then there is a swipe
                if (swipeForce < 0) //means we are swiping towards left
                    SlideCamera(true);
                else
                    SlideCamera(false);
            }
        }
    }
    
    private void FixedUpdate()
    {
        desiredPosition = lookAt.position + offset; //follows player around for every frame
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.LookAt(lookAt.position + Vector3.up);
    }

    public void SlideCamera(bool left)
    {
        //called when you want to swipe camera to left or right
        if (left)
            offset = Quaternion.Euler(0, 90, 0) * offset;
        else
            offset = Quaternion.Euler(0, -90, 0) * offset;
        
    }
}
