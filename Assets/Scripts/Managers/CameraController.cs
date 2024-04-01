using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    private CameraControlActions cameraActions;
    private InputAction movement;
    private Transform cameraTransform;

    public AnimationCurve tiltCurve;

    //horizontal mmotion
    [SerializeField] private float maxSpeed = 5f;
    [SerializeField] private float speed;
    [SerializeField] private float acceleration = 10f;
    [SerializeField] private float damping = 15f;
    [SerializeField] private float angleChangeSpeed = 2.0f;

    //vertical motion zooming
    [SerializeField] private float stepSize = 2f;
    [SerializeField] private float zoomDampening = 7.5f;
    [SerializeField] private float minHeight = 5f;
    [SerializeField] private float maxHeight = 50f;
    [SerializeField] private float zoomSpeed = 2f;

    //rotation
    [SerializeField] private float maxRotationSpeed = 1f;

    //screen edge motion
    [SerializeField][Range(0f, 0.1f)] private float edgeTolerance = 0.05f;
    [SerializeField] private bool useScreenEdge = true;

    private bool angleChange;
    private Vector3 targetPosition;

    private float zoomHeight;
    private float tiltAngle;
    

    private Vector3 horizontalVelocity;
    private Vector3 lastPosition;

    private Vector3 startDrag;
    private Vector3 zoomTarget;

    private void Awake()
    {
        cameraActions = new CameraControlActions();
        cameraTransform = this.GetComponentInChildren<Camera>().transform;
        zoomTarget = new Vector3(cameraTransform.localPosition.x, maxHeight, cameraTransform.localPosition.z);// + cameraTransform.forward;
    }


    private void OnEnable()
    {
        zoomHeight = cameraTransform.localPosition.y;
        //cameraTransform.LookAt(this.transform);

        lastPosition = this.transform.position;
        movement = cameraActions.Camera.Movement;

        cameraActions.Camera.Rotate.performed += RotateCamera;
        cameraActions.Camera.Zoom.performed += OnMouseScroll;
        cameraActions.Camera.Angle.performed += (x) => { angleChange = true; };
        cameraActions.Camera.Angle.canceled += (x) => { angleChange = false; };
        cameraActions.Camera.Enable();
    }

    private void OnDisable()
    {
        cameraActions.Camera.Rotate.performed -= RotateCamera;
        cameraActions.Camera.Zoom.performed -= OnMouseScroll;
        cameraActions.Camera.Angle.performed -= (x) => { angleChange = true; };
        cameraActions.Camera.Angle.canceled -= (x) => { angleChange = false; };
        cameraActions.Disable(); 
    }

    private void Update()
    {
        GetKeyboardMovement();

        //if(useScreenEdge)
        //{
        //    CheckMouseAtScreenEdge();

        //}

        DragCamera();

        UpdateVelocity();
        UpdateCameraPosition();
        UpdateCamaraAngle();
        UpdateBasePosition(); 
    }

    private void UpdateVelocity()
    {
        horizontalVelocity = (this.transform.position - lastPosition) / Time.deltaTime;
        horizontalVelocity.y = 0;
        lastPosition = this.transform.position;
    }

    private void GetKeyboardMovement()
    {
        Vector3 inputValue = movement.ReadValue<Vector2>().x * GetCameraRight()
                                + movement.ReadValue<Vector2>().y * GetCameraForward();

        inputValue = inputValue.normalized;

        if (inputValue.sqrMagnitude > 0.1f)
            targetPosition += inputValue;
    }

    private Vector3 GetCameraRight()
    {
        Vector3 right = cameraTransform.right;
        right.y = 0;
        return right;
    }

    private Vector3 GetCameraForward()
    {
        Vector3 forward = cameraTransform.forward;
        forward.y = 0;
        return forward;
    }

    private void OnMouseScroll(InputAction.CallbackContext inputValue)
    {
        if(angleChange)
        {
            //ChangeCameraAngle(-inputValue.ReadValue<Vector2>().y);
        }
        else
        {
            ZoomCamera(-inputValue.ReadValue<Vector2>().y);
        }
        
    }

    private void UpdateBasePosition()
    {
        if(targetPosition.sqrMagnitude > 0.1f)
        {
            speed = Mathf.Lerp(speed, maxSpeed, Time.deltaTime * acceleration);
            transform.position += targetPosition * speed * Time.deltaTime;
        }
        else
        {
            horizontalVelocity = Vector3.Lerp(horizontalVelocity, Vector3.zero, Time.deltaTime * damping);
            transform.position += horizontalVelocity * Time.deltaTime;
        }

        targetPosition = Vector3.zero;
    }

    private void RotateCamera(InputAction.CallbackContext inputValue)
    {
        if(!Mouse.current.middleButton.isPressed)
        {
            return;
        }

        float value = inputValue.ReadValue<Vector2>().x;
        transform.rotation = Quaternion.Euler(0f, value * maxRotationSpeed + transform.rotation.eulerAngles.y, 0f);
    }

    private void ZoomCamera(float inputValue)
    {
        float value = inputValue / 100f;

        if(Mathf.Abs(value) > 0.1f)
        {
            zoomHeight = cameraTransform.localPosition.y + value * stepSize;
            if(zoomHeight < minHeight)
                zoomHeight = minHeight;
            else if(zoomHeight > maxHeight)
                zoomHeight = maxHeight;

            Vector3 moveDirection = ((cameraTransform.localEulerAngles.x >= 60) ? cameraTransform.forward : (Vector3.down));
            zoomTarget = cameraTransform.localPosition  - Mathf.Sign(value) * moveDirection;
            zoomTarget.y = Mathf.Clamp(zoomTarget.y * (Mathf.Sin(60 * Mathf.Deg2Rad)), minHeight, maxHeight);
            zoomTarget.z = -Mathf.Clamp(-zoomTarget.z * (Mathf.Cos(60 * Mathf.Deg2Rad)), minHeight, maxHeight);
            //zoomTarget = Vector3.ClampMagnitude(zoomTarget, maxHeight);
        }
    }

    private void ChangeCameraAngle(float inputValue)
    {
        float value = inputValue / 100f;
        if(Mathf.Abs(value) > 0.1f)
        {
            tiltAngle = cameraTransform.localEulerAngles.x + value * 2f;
            if(tiltAngle < 0)
                tiltAngle = 0;
            else if(tiltAngle > 60)
                tiltAngle = 60;
        }
    }

    private void UpdateCameraPosition()
    {
        cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, zoomTarget, Time.deltaTime * zoomDampening);
    }

    private void UpdateCamaraAngle()
    {
        float dis = Mathf.Clamp((zoomHeight - minHeight) / (maxHeight - minHeight), 0, 1);

        float testAngle = tiltCurve.Evaluate(dis);

        cameraTransform.localRotation = Quaternion.Slerp(cameraTransform.localRotation, Quaternion.Euler(testAngle, 0, 0), Time.deltaTime * 20);
    }

    private void CheckMouseAtScreenEdge()
    {
        Vector2 mousePosition = Mouse.current.position.ReadValue();
        Vector3 moveDirection = Vector3.zero;

        if (mousePosition.x < edgeTolerance * Screen.width)
            moveDirection += -GetCameraRight();
        else if (mousePosition.x > (1f - edgeTolerance) * Screen.width)
            moveDirection += GetCameraRight();

        if (mousePosition.y < edgeTolerance * Screen.height)
            moveDirection += -GetCameraForward();
        else if(mousePosition.y > (1f - edgeTolerance) * Screen.height)
            moveDirection += GetCameraForward();

        targetPosition += moveDirection;
    }

    private void DragCamera()
    {
        if (!Mouse.current.rightButton.isPressed)
            return;

        Plane plane = new Plane(cameraTransform.forward, Vector3.zero);
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());

        if(plane.Raycast(ray, out float distance))
        {
            if(Mouse.current.rightButton.wasPressedThisFrame)
                startDrag = ray.GetPoint(distance);
            else
                targetPosition += startDrag - ray.GetPoint(distance);

            //targetPosition.z = cameraTransform.forward * 
            //Debug.Log(targetPosition);
            if(cameraTransform.forward.z == 1)
            {
                targetPosition.z = targetPosition.y;
                targetPosition.y = 0;
                
            }
            else
            {
                targetPosition.y = 0;
            }
            
            
        }
    }

}
