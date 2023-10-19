using System;
using UnityEngine;

public class Piece : MonoBehaviour
{
    [SerializeField]
    private Segment[] _segments;

    [SerializeField]
    private Transform _unplacedPosition;

    private bool _isDraggable = false;
    private bool _isBeingDragged = false;

    private Vector3 _mouseOffset;
    private Vector3 _mousePosition;

    public static event Action PieceHasBeenPlaced;

    private bool _isPlaced = false;

    private void Start()
    {
        ResetPosition();
    }

    private void OnMouseDown()
    {
        if (_isDraggable)
        {
            PickUpPiece();
        }
    }

    private void OnMouseDrag()
    {
        if (_isBeingDragged)
        {
            DragPiece();
        }
    }

    private void OnMouseUp()
    {
        if (_isBeingDragged)
        {
            DropPiece();
        }
    }

    public bool GetIsDraggable()
    {
        return _isDraggable;
    }

    public void SetIsDraggable(bool newValue)
    {
        _isDraggable = newValue;
    }

    private void PickUpPiece()
    {
        _isPlaced = false;
        _isBeingDragged = true;

        for (int i = 0; i < _segments.Length; i++)
        {
            _segments[i].RemovePlacementOnBoard();
            _segments[i].EnablePlacementIndicator();
        }

        UpdateMousePosition();
        _mouseOffset = gameObject.transform.position - _mousePosition;
    }

    private void DragPiece()
    {
        UpdateMousePosition();

        if (_isDraggable)
        {
            gameObject.transform.position = new Vector3(_mousePosition.x + _mouseOffset.x, gameObject.transform.position.y, _mousePosition.z + _mouseOffset.z);
        }
    }

    private void DropPiece()
    {
        bool canPlacePiece = true;

        // determine if piece can be placed
        for (int i = 0; i < _segments.Length; i++)
        {
            if (_segments[i].IsPlaceable() == false)
            {
                canPlacePiece = false;
            }
        }

        if (canPlacePiece == true)
        {
            PlacePiece();
        }
        else
        {
            ResetPosition();
        }

        for (int i = 0; i < _segments.Length; i++)
        {
            _segments[i].DisablePlacementIndicator();
        }

        _isBeingDragged = false;
    }

    private void UpdateMousePosition()
    {
        Ray rayThroughMouse = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(rayThroughMouse, out hit, 25, LayerMask.GetMask("Mouse Plane")))
        {
            _mousePosition = hit.point + new Vector3(0, .1f, 0);
        }
    }

    public void ResetPosition()
    {
        gameObject.transform.position = _unplacedPosition.position;
        gameObject.transform.rotation = _unplacedPosition.rotation;
    }

    private void PlacePiece()
    {
        for (int i = 0; i < _segments.Length; i++)
        {
            _segments[i].SetPlacementOnBoard();
        }

        float averageXPos = (_segments[0].GetOccupiedTile().gameObject.transform.position.x + _segments[_segments.Length - 1].GetOccupiedTile().gameObject.transform.position.x) / 2;
        float averageZPos = (_segments[0].GetOccupiedTile().gameObject.transform.position.z + _segments[_segments.Length - 1].GetOccupiedTile().gameObject.transform.position.z) / 2;

        gameObject.transform.position = new Vector3(averageXPos, gameObject.transform.position.y, averageZPos);

        _isPlaced = true;

        PieceHasBeenPlaced?.Invoke();
    }

    public bool IsPiecePlaced()
    {
        return _isPlaced;
    }
}
