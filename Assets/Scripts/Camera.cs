using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;
    public Vector3 firstPersonOffset = new Vector3(0, 1.5f, 0); // Camera height in first-person
    public Vector3 thirdPersonOffset = new Vector3(0, 1.5f, -5f); // Camera behind player in third-person
    public float rotationSpeed = 150f;
    public float scrollSensitivity = 2f;
    public float sensitivityX = 1f;
    public float sensitivityY = 1f;
    private float currentX;
    private float currentY;
    private float currentZoom;
    private bool isLocked = false;

    private const float minZoom = 0.5f; // First person close zoom
    private const float maxZoom = 10f;  // Max third person zoom
    private const float firstPersonThreshold = 1.0f; // Distance threshold to switch to first person

    void Start()
    {
        currentZoom = maxZoom;
        currentX = 0f;
        currentY = 10f;  // Slightly looking down by default
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // Handle zoom input
        var scroll = Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Abs(scroll) > 0.01f)
        {
            currentZoom = Mathf.Clamp(currentZoom - scroll * scrollSensitivity, minZoom, maxZoom);
        }

        if (IsInFirstPerson())
        {
            // First person mode allows free rotation
            currentX += Input.GetAxis("Mouse X") * rotationSpeed * sensitivityX * Time.deltaTime;
            currentY -= Input.GetAxis("Mouse Y") * rotationSpeed * sensitivityY * Time.deltaTime;
            currentY = Mathf.Clamp(currentY, -30f, 85f); // Clamp vertical rotation
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            isLocked = true;
        }
        else // Third person mode
        {
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                isLocked = !isLocked;
                Cursor.lockState = isLocked ? CursorLockMode.Locked : CursorLockMode.None;
                Cursor.visible = !isLocked;
            }

            if (isLocked)
            {
                // Free camera rotation with mouse input
                currentX += Input.GetAxis("Mouse X") * rotationSpeed * sensitivityX * Time.deltaTime;
                currentY -= Input.GetAxis("Mouse Y") * rotationSpeed * sensitivityY * Time.deltaTime;
            }
            else
            {
                // Camera rotation only allowed when left mouse button held
                if (Input.GetMouseButton(1))
                {
                    currentX += Input.GetAxis("Mouse X") * rotationSpeed * sensitivityX * Time.deltaTime;
                    currentY -= Input.GetAxis("Mouse Y") * rotationSpeed * sensitivityY * Time.deltaTime;
                }
            }
            currentY = Mathf.Clamp(currentY, -30f, 85f);
        }
    }

    private void LateUpdate()
    {
        if (!target) return;

        if (IsInFirstPerson())
        {
            // First person: camera at player's head position looking forward exactly
            Vector3 fpPosition = target.position + firstPersonOffset;
            transform.position = fpPosition;
            transform.rotation = Quaternion.Euler(currentY, currentX, 0);
        }
        else
        {
            // Third person: calculate position based on rotation and zoom with offset height
            Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
            Vector3 direction = new Vector3(0, 0, -currentZoom);   // Negative zoom to go behind target
            Vector3 desiredPosition = target.position + rotation * direction + thirdPersonOffset;
            transform.position = desiredPosition;
            transform.LookAt(target.position + Vector3.up * 1.5f);
        }
    }

    private bool IsInFirstPerson()
    {
        return currentZoom <= firstPersonThreshold;
    }
}
