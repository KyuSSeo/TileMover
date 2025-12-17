using UnityEngine;
using System.Collections;

public class CamRig : MonoBehaviour
{
    private Transform _transform;
    public Transform target;
    public float moveSpeed = 0.5f;
    public float rotateSpeed = 3.0f;
    public float zoomSpeed = 10.0f; 

    public float minZoom = 5f;
    public float maxZoom = 20f;
    public Vector3 targetOffset = Vector3.zero;

    private float currentXRotation = 0f;

    void Awake()
    {
        _transform = transform;
    }

    void Update()
    {
        OnMove();
    }
    
    private void OnMove()
    {
        if (target)
            _transform.position = Vector3.Lerp(_transform.position, target.position, moveSpeed * Time.deltaTime);
    }


    private void OnEnable()
    {
        InputController.cameraMoveEvent += OnCameraMove;
        InputController.cameraRotateEvent += OnCameraRotate;
        InputController.cameraZoomEvent += OnCameraZoom;
    }

    private void OnDisable()
    {
        // InputController 이벤트 구독 해제
        InputController.cameraMoveEvent -= OnCameraMove;
        InputController.cameraRotateEvent -= OnCameraRotate;
        InputController.cameraZoomEvent -= OnCameraZoom;
    }

    private void OnCameraMove(object sender, InfoEventArgs<Vector3> e)
    {
        Vector3 dragDelta = e.info;
        Vector3 moveDelta = new Vector3(dragDelta.x, 0, dragDelta.y);
        _transform.Translate(moveDelta * moveSpeed * Time.deltaTime, Space.World);
    }

    private void OnCameraRotate(object sender, InfoEventArgs<Vector2> e)
    {
        Vector2 rotateDelta = e.info;

        float nextXRotation = currentXRotation + (rotateDelta.y * rotateSpeed);
        
        nextXRotation = Mathf.Clamp(nextXRotation, -30f, 30f); 

        float rotationChange = nextXRotation - currentXRotation;

        _transform.Rotate(Vector3.left, rotationChange, Space.Self);
        _transform.Rotate(Vector3.up, -rotateDelta.x * rotateSpeed, Space.World);

        currentXRotation = nextXRotation;
    }

    private void OnCameraZoom(object sender, InfoEventArgs<float> e)
    {
        float zoomDelta = e.info;
        float newZoom = Mathf.Clamp(_transform.position.z + zoomDelta * zoomSpeed, minZoom, maxZoom);
        _transform.Translate(Vector3.forward * zoomDelta * zoomSpeed * Time.deltaTime, Space.Self);
    }
}
