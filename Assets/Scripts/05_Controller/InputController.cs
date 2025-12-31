using System;
using UnityEngine;

public class InputController : MonoBehaviour
{
    private const float _repeatThreshold = 0.5f;
    private const float _repeatRate = 0.25f;
    private const float _tapRate = 0.1f;

    private float _horNext, _verNext;
    private bool _horHold, _verHold;
    private float interval = 0.25f;
    private float doubleClickedTime = -1.0f;

    private string[] _buttons = new string[] { "Fire1", "Fire2", "Fire3" };

    public static event EventHandler<InfoEventArgs<int>> selEvent;
    public static event EventHandler<InfoEventArgs<Point>> moveEvent;
    public static event EventHandler<InfoEventArgs<Point>> tileClickEvent;
    public static event EventHandler<InfoEventArgs<float>> cameraZoomEvent;
    public static event EventHandler<InfoEventArgs<Vector3>> cameraMoveEvent;
    public static event EventHandler<InfoEventArgs<Vector2>> cameraRotateEvent;

    private float vertical;
    private float horizontal;

    private Vector3 dragOrigin;

    private void Update()
    {
        HandleKeyboardInput();
        HandleMouseLeft();
        HandleMouseRight();
        HandleMouseScroll();
    }

    private void HandleKeyboardInput()
    {
        vertical = Input.GetAxisRaw("Vertical");
        horizontal = Input.GetAxisRaw("Horizontal");

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
            {
                Select(i);
                Debug.Log("Key Selection Event");
            }
        }
    }

    private void HandleMouseLeft()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if ((Time.time - doubleClickedTime) < interval)
            {
                doubleClickedTime = -1.0f;
                CheckTileClick();
            }
            else 
            {
                doubleClickedTime = Time.time;
            }
            dragOrigin = Input.mousePosition;
        }

        if (Input.GetMouseButton(0))
        {
            Vector3 currentDrag = Input.mousePosition;
            Vector3 dragDelta = dragOrigin - currentDrag;

            cameraMoveEvent?.Invoke(this, new InfoEventArgs<Vector3>(dragDelta));
            dragOrigin = currentDrag;
        }
    }

    private void HandleMouseRight()
    {
        if (Input.GetMouseButton(1))
        {
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");

            Vector2 rotateDelta = new Vector2(mouseX, mouseY);
            cameraRotateEvent?.Invoke(this, new InfoEventArgs<Vector2>(rotateDelta));
        }
    }

    private void HandleMouseScroll()
    {
        float scrollDelta = Input.GetAxis("Mouse ScrollWheel");
        if (!Mathf.Approximately(scrollDelta, 0))
        {
            cameraZoomEvent?.Invoke(this, new InfoEventArgs<float>(scrollDelta));
        }
    }

    private void CheckTileClick()
    {   
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            PlaceObject selectedUnit = hit.collider.GetComponent<PlaceObject>();
            if (selectedUnit != null) 
            {
                Debug.Log($"유닛 선택: {selectedUnit.tile.pos}");
            }
            Tile hitTile = hit.collider.GetComponent<Tile>();
            if (hitTile != null)
            {
                Debug.Log($"타일 선택: {hitTile.pos}");
                tileClickEvent?.Invoke(this, new InfoEventArgs<Point>(hitTile.pos));
            }
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