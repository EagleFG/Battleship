using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    [SerializeField] [Range(1, 99)]
    private int _boardWidth, _boardHeight;

    [SerializeField]
    private Transform _boardStartingPosition, _boardOffsetPosition;

    [SerializeField]
    private GameObject _tilePrefab;

    private Dictionary<Vector2Int, Tile> _tiles;

    private bool _isBoardInteractable = false;

    private void Start()
    {
        _boardOffsetPosition.localPosition = new Vector3(-(float)_boardWidth / 2 + 0.5f, 0, -(float)_boardHeight / 2 + .5f);

        SpawnTiles();

        gameObject.transform.position = _boardStartingPosition.position;
        gameObject.transform.eulerAngles = _boardStartingPosition.eulerAngles;
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
        _tiles = new Dictionary<Vector2Int, Tile>();

        for (int x = 0; x < _boardWidth; x++)
        {
            for (int z = 0; z < _boardHeight; z++)
            {
                GameObject newTileObject = Instantiate(_tilePrefab, new Vector3(_boardOffsetPosition.position.x + x, 0, _boardOffsetPosition.position.z + z), Quaternion.identity, _boardOffsetPosition);

                newTileObject.transform.eulerAngles = new Vector3(90, 0, 0);
                newTileObject.name = Tile.ConvertTilePositionToName(new Vector2Int(x, z));

                // retrieve tile component to store in dictionary
                if (newTileObject.TryGetComponent(out Tile newTile) == false)
                {
                    Debug.LogError("Couldn't find Tile " + newTileObject.name + "'s Tile component upon spawning");
                }

                _tiles[new Vector2Int(x, z)] = newTile;
            }
        }
    }

    private void ReceiveSelectedTile(Vector2Int position)
    {
        if (_isBoardInteractable == true)
        {
            GetTile(position).SetTileColorStatus(Random.Range(2,4));
        }
    }

    public Tile GetTile(Vector2Int position)
    {
        if (_tiles.TryGetValue(position, out Tile tile) == false)
        {
            Debug.LogError("Couldn't find selected tile");
        }

        return tile;
    }

    public bool GetIsBoardInteractable()
    {
        return _isBoardInteractable;
    }

    public void SetIsBoardInteractable(bool newValue)
    {
        foreach (Tile tile in _tiles.Values)
        {
            tile.SetIsInteractable(newValue);
        }

        _isBoardInteractable = newValue;
    }

    // visualization of where the board will be after generation
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.matrix = _boardStartingPosition.localToWorldMatrix;
        Gizmos.DrawWireCube(Vector3.zero, new Vector3(_boardWidth, 0, _boardHeight));
    }
}
