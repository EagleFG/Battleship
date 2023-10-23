using System;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [HideInInspector]
    public Board boardTileBelongsTo;

    [SerializeField]
    private Material[] _materials;

    [SerializeField]
    private MeshRenderer _renderer;

    enum TileColorStatus { Normal, Hovered, Confirmed, Rejected };
    private TileColorStatus _status = TileColorStatus.Normal;

    public static event Action<Tile> TileHasBeenSelected;

    public bool isOccupied = false;
    public string occupyingPieceName = "";
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
        if (boardTileBelongsTo.isBoardInteractable && _status == TileColorStatus.Normal)
        {
            SetTileColorStatus((int)TileColorStatus.Hovered);
        }
    }

    private void OnMouseExit()
    {
        if (boardTileBelongsTo.isBoardInteractable && _status == TileColorStatus.Hovered)
        {
            SetTileColorStatus((int)TileColorStatus.Normal);
        }
    }

    // Broadcast this tile's location as an event when selected with the mouse
    private void OnMouseUpAsButton()
    {
        if (boardTileBelongsTo.isBoardInteractable)
        {
            TileHasBeenSelected?.Invoke(this);
        }
    }

    public static string ConvertTilePositionToName(Vector2Int position)
    {
        string rowName = Convert.ToChar(position.y + 65).ToString();
        string columnName = (position.x + 1).ToString();

        return rowName + columnName;
    }

    public static Vector2Int ConvertTileNameToPosition(string name)
    {
        int positionX = int.Parse(name.Substring(1)) - 1;
        int positionY = Convert.ToInt32(name[0]) - 65;

        return new Vector2Int(positionX, positionY);
    }
}
