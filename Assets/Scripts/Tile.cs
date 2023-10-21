using System;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField]
    private Material[] _materials;

    [SerializeField]
    private MeshRenderer _renderer;

    enum TileColorStatus { Normal, Hovered, Confirmed, Rejected };
    private TileColorStatus _status = TileColorStatus.Normal;

    public static event Action<Tile> TileHasBeenSelected;

    public bool isInteractable = false;
    public bool isOccupied = false;
    public bool hasBeenAttacked = false;

    public void SetTileColorStatus(int newStatus)
    {
        _status = (TileColorStatus)newStatus;
        SetMaterial(newStatus);
    }

    // To ensure proper material switching, material array should contain materials in this order:
    // Default = 0, Hovered = 1, Confirmed = 2, Rejected = 3
    private void SetMaterial(int materialIndex)
    {
        _renderer.material = _materials[materialIndex];
    }

    private void OnMouseEnter()
    {
        if (isInteractable && _status == TileColorStatus.Normal)
        {
            SetTileColorStatus((int)TileColorStatus.Hovered);
        }
    }

    private void OnMouseExit()
    {
        if (isInteractable && _status == TileColorStatus.Hovered)
        {
            SetTileColorStatus((int)TileColorStatus.Normal);
        }
    }

    // Broadcast this tile's location as an event when selected with the mouse
    private void OnMouseUpAsButton()
    {
        if (isInteractable)
        {
            TileHasBeenSelected?.Invoke(this);
        }
    }

    public static string ConvertTilePositionToName(Vector2Int position)
    {
        string columnName = Convert.ToChar(position.x + 65).ToString();
        string rowName = (position.y + 1).ToString();

        return columnName + rowName;
    }

    public static Vector2Int ConvertTileNameToPosition(string name)
    {
        int positionX = Convert.ToInt32(name[0]) - 65;
        int positionY = int.Parse(name.Substring(1)) - 1;

        return new Vector2Int(positionX, positionY);
    }
}
