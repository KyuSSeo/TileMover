using System;
using UnityEngine;

public class InputController : MonoBehaviour
{
    private const float _repeatThreshold = 0.5f;
    private const float _repeatRate = 0.25f;
    private const float _tapRate = 0.1f;

    private float _horNext, _verNext;
    private bool _horHold, _verHold;
    private string[] _buttons = new string[] { "Fire1", "Fire2", "Fire3" };

    public float _rotationSpeed = 3.5f; // 우클릭 회전 속도
    public float _zoomSpeed = 10.0f;    // 휠 줌 속도

    public static event EventHandler<InfoEventArgs<int>> selEvent;
    public static event EventHandler<InfoEventArgs<Point>> moveEvent;
    public static event EventHandler<InfoEventArgs<string>> mouseEvent;


    private float vertical;
    private float horizontal;
    private Vector3 dragOrigin;

    private void Update()
    {
        vertical = Input.GetAxisRaw("Vertical");
        horizontal = Input.GetAxisRaw("Horizontal");

        HandleLeftClick();
        HandleRightClick();
        HandleZoom();

        int x = 0, y = 0;

        if (!Mathf.Approximately(horizontal, 0))
        {
            if (Time.time > _horNext)
            {
                x = (horizontal < 0f) ? -1 : 1;
                _horNext = Time.time + (_horHold ? _repeatRate : _repeatThreshold);
                _horHold = true;
            }
        }
        else
        {
            _horHold = false;
            _horNext = 0;
        }

        if (!Mathf.Approximately(vertical, 0))
        {
            if (Time.time > _verNext)
            {
                y = (vertical < 0f) ? -1 : 1;
                _verNext = Time.time + (_verHold ? _repeatRate : _repeatThreshold);
                _verHold = true;
            }
        }
        else
        {
            _verHold = false;
            _verNext = 0;
        }


        if (x != 0 || y != 0)
            Move(new Point(x, y));


        for (int i = 0; i < 3; ++i)
        {
            if (Input.GetButtonUp(_buttons[i]))
                Select(i);
        }
    }
    private void HandleLeftClick()
    {
        // 클릭
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 1000f))
            {
                Debug.Log($"정보 확인: {hit.collider.name}");
            }
        }

        // 드래그
        if (Input.GetMouseButton(0))
        {
            Plane plane = new Plane(Vector3.up, Vector3.zero);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            float entry;

            if (plane.Raycast(ray, out entry))
            {
                Vector3 currentPos = ray.GetPoint(entry);
                Vector3 difference = dragOrigin - currentPos;
                transform.position += difference;
            }
        }
    }
    void HandleRightClick()
    {
        // 클릭
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 1000f))
            {
                Debug.Log($"정보 확인: {hit.collider.name}");
            }
        }

        // 드래그
        if (Input.GetMouseButton(1))
        {
            // B. 카메라 회전 (기능 4)
            float h = Input.GetAxis("Mouse X");
            float v = Input.GetAxis("Mouse Y");

            // 좌우 회전 (Y축 기준, 월드 좌표)
            transform.Rotate(Vector3.up * h * _rotationSpeed, Space.World);

            // 상하 회전 (X축 기준, 로컬 좌표 - 고개 끄덕임)
            transform.Rotate(Vector3.left * v * _rotationSpeed, Space.Self);
        }
    }

    private void HandleZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (scroll != 0)
        {
            transform.Translate(transform.forward * scroll * _zoomSpeed, Space.World);
        }
    }


    private void Select(int i)
    {
        if (selEvent != null)
            selEvent(this, new InfoEventArgs<int>(i));
    }

    private void Move(Point p)
    {
        if (moveEvent != null)
            moveEvent(this, new InfoEventArgs<Point>(p));
    }
}