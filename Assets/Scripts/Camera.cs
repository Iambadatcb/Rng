using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class Camera : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0, 0, 10);
    public float rotationSpeed = 5;
    public float scrollSensitivity = 2;
    public float sensX;
    public float sensY;
    private float currentX;
    private float currentY;
    private float currentZoom;
    private bool Locked = false;
    
    void Start()
    {
        currentZoom = offset.z;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
                //mouse input
                currentX += Input.GetAxis("Mouse X") * rotationSpeed *sensX*Time.deltaTime;
                currentY += Input.GetAxis("Mouse Y") * rotationSpeed*sensY*Time.deltaTime;
       if (Locked == false)
        {

            if (Input.GetKeyDown(KeyCode.LeftShift))
            {


                //scroll
                var scroll = Input.GetAxis("Mouse ScrollWheel");
                currentZoom += scroll * scrollSensitivity;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                Locked = true;
            }
        }
        else if (Locked == true)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {

                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                Locked = false;
            }
        }
    }

    private void LateUpdate()
    {
        if(!target) return;
        
        var rotation = Quaternion.Euler(currentY, currentX, 0);
        var direction = new Vector3(0, 0, currentZoom);
        var desiredPosition = target.position + rotation * direction;
        transform.position = desiredPosition + offset;
        transform.LookAt(target.position + Vector3.up * 1.5f);
    }
}
