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
    private Quaternion initialRotation;

    void Awake()
    {
        _transform = transform;
        initialRotation = _transform.rotation;
    }

    private void OnEnable()
    {
        InputController.cameraMoveEvent += OnCameraMove;
        InputController.cameraRotateEvent += OnCameraRotate;
        InputController.cameraZoomEvent += OnCameraZoom;
        InputController.moveEvent += OnKeybordMove;
    }

    private void OnDisable()
    {
        InputController.cameraMoveEvent -= OnCameraMove;
        InputController.cameraRotateEvent -= OnCameraRotate;
        InputController.cameraZoomEvent -= OnCameraZoom;
        InputController.moveEvent -= OnKeybordMove;
    }
    private void OnKeybordMove(object sender, InfoEventArgs<Point> e)
    {
        if (target)
        {
            StopAllCoroutines();
            StartCoroutine(MoveToTarget(target.position + targetOffset));
        }
    }

    private IEnumerator MoveToTarget(Vector3 targetPosition)
    {
        float duration = 0.5f;
        float elapsed = 0f;

        Vector3 startPosition = _transform.position;
        Quaternion startRotation = _transform.rotation;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            _transform.position = Vector3.Lerp(startPosition, targetPosition, t);
            _transform.rotation = Quaternion.Slerp(startRotation, initialRotation, t);
            yield return null;
        }
        _transform.position = targetPosition;
        currentXRotation = 0f;
    }

    private void OnCameraMove(object sender, InfoEventArgs<Vector3> e)
    {
        StopAllCoroutines();
        Vector3 dragDelta = e.info;
        Vector3 moveDelta = new Vector3(dragDelta.x, 0, dragDelta.y);
        _transform.Translate(moveDelta * moveSpeed * Time.deltaTime, Space.World);
    }

    private void OnCameraRotate(object sender, InfoEventArgs<Vector2> e)
    {
        StopAllCoroutines();
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
