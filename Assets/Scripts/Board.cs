using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    [SerializeField] [Range(1, 99)]
    private int _boardWidth, _boardHeight;

    [SerializeField]
    private Transform _boardOffset;

    [SerializeField]
    private GameObject _tilePrefab;

    private Dictionary<Vector2Int, GameObject> _tiles;

    private void Start()
    {
        _boardOffset.localPosition = new Vector3(-(float)_boardWidth / 2 + 0.5f, 0, -(float)_boardHeight / 2 + .5f);

        SpawnTiles();
    }

    private void OnEnable()
    {
        Tile.TileHasBeenSelected += ReceiveSelectedTile;
    }

    private void OnDisable()
    {
        Tile.TileHasBeenSelected -= ReceiveSelectedTile;
    }

    private void SpawnTiles()
    {
        _tiles = new Dictionary<Vector2Int, GameObject>();

        for (int x = 0; x < _boardWidth; x++)
        {
            for (int z = 0; z < _boardHeight; z++)
            {
                GameObject newTile = Instantiate(_tilePrefab, new Vector3(_boardOffset.position.x + x, 0, _boardOffset.position.z + z), Quaternion.identity, _boardOffset);

                newTile.transform.eulerAngles = new Vector3(90, 0, 0);
                newTile.name = Tile.ConvertTilePositionToName(new Vector2Int(x, z));

                _tiles[new Vector2Int(x, z)] = newTile;
            }
        }
    }

    private void ReceiveSelectedTile(Vector2Int position)
    {
        SelectTile(position).SwitchMaterials(Random.Range(2,4));
    }

    private Tile SelectTile(Vector2Int position)
    {
        if (_tiles.TryGetValue(position, out GameObject tileObject) == false)
        {
            Debug.LogError("Couldn't find selected tile");
        }
        else
        {
            if (tileObject.TryGetComponent(out Tile tile) == false)
            {
                Debug.LogError("Couldn't find selected tile component");
            }
            else
            {
                return tile;
            }
        }

        return null;
    }
}
