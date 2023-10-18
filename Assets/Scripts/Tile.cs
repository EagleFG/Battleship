using System;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField]
    private Material[] _materials;

    private MeshRenderer _renderer;

    public static event Action<Vector2Int> TileHasBeenSelected;

    private void Awake()
    {
        if (TryGetComponent(out _renderer) == false)
        {
            Debug.LogError("Couldn't find Tile" + gameObject.name + "'s MeshRenderer");
        }
    }

    // To ensure proper material switching, material array should contain materials in this order:
    // Default, Hovered, Confirmed, Rejected
    public void SwitchMaterials(int materialIndex)
    {
        _renderer.material = _materials[materialIndex];
    }

    private void OnMouseEnter()
    {
        SwitchMaterials(1);
    }

    private void OnMouseExit()
    {
        SwitchMaterials(0);
    }

    private void OnMouseUpAsButton()
    {
        TileHasBeenSelected?.Invoke(ConvertTileNameToPosition(gameObject.name));
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
