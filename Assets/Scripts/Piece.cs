using System;
using UnityEngine;

public class Piece : MonoBehaviour
{
    [SerializeField]
    private Segment[] _segments;

    [SerializeField]
    private Transform _unplacedPosition;

    [HideInInspector]
    public bool isDraggable = false;
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
        if (isDraggable)
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

    private void PickUpPiece()
    {
        _isPlaced = false;
        _isBeingDragged = true;

        for (int i = 0; i < _segments.Length; i++)
        {
            _segments[i].RemoveOccupiedTile();
            _segments[i].SetIsAssessingTileBeneath(true);
        }

        // rotate the piece to the horizontal position if it's not already in the vertical position
        if (gameObject.transform.eulerAngles.y != 270)
        {
            gameObject.transform.eulerAngles = Vector3.zero;
        }

        UpdateMousePosition();
        _mouseOffset = gameObject.transform.position - _mousePosition;
    }

    private void DragPiece()
    {
        UpdateMousePosition();

        if (Input.GetMouseButtonUp(1))
        {
            if (gameObject.transform.eulerAngles.y == 0)
            {
                gameObject.transform.eulerAngles = new Vector3(0, 270, 0);
            }
            else
            {
                gameObject.transform.eulerAngles = Vector3.zero;
            }
        }

        gameObject.transform.position = new Vector3(_mousePosition.x + _mouseOffset.x, gameObject.transform.position.y, _mousePosition.z + _mouseOffset.z);
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
            _segments[i].SetIsAssessingTileBeneath(false);
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
        if (_unplacedPosition != null)
        {
            gameObject.transform.position = _unplacedPosition.position;
            gameObject.transform.rotation = _unplacedPosition.rotation;
        }
    }

    private void PlacePiece()
    {
        for (int i = 0; i < _segments.Length; i++)
        {
            _segments[i].SetOccupiedTile();
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
