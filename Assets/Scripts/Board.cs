using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    [SerializeField] [Range(1, 99)]
    private int _boardWidth, _boardHeight;

    [SerializeField]
    private Transform _boardOffsetPosition, _boardStartingPosition;

    [SerializeField]
    private GameObject _tilePrefab;

    private Dictionary<Vector2Int, Tile> _boardTiles;

    private bool _isBoardInteractable = false;

    private bool _isInDebugView = false;

    private void Start()
    {
        _boardOffsetPosition.localPosition = new Vector3(-(float)_boardWidth / 2 + 0.5f, 0, -(float)_boardHeight / 2 + .5f);

        SpawnTiles();

        if (_boardStartingPosition != null)
        {
            gameObject.transform.position = _boardStartingPosition.position;
            gameObject.transform.eulerAngles = _boardStartingPosition.eulerAngles;
        }
    }

    private void SpawnTiles()
    {
        _boardTiles = new Dictionary<Vector2Int, Tile>();

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

                _boardTiles[new Vector2Int(x, z)] = newTile;
            }
        }
    }

    public Tile GetTile(Vector2Int position)
    {
        if (_boardTiles.TryGetValue(position, out Tile tile) == false)
        {
            Debug.LogError("Couldn't find selected tile");
        }

        return tile;
    }

    public int GetBoardWidth()
    {
        return _boardWidth;
    }

    public int GetBoardHeight()
    {
        return _boardHeight;
    }

    public bool GetIsBoardInteractable()
    {
        return _isBoardInteractable;
    }

    public void SetIsBoardInteractable(bool newValue)
    {
        foreach (Tile tile in _boardTiles.Values)
        {
            tile.isInteractable = newValue;
        }

        _isBoardInteractable = newValue;
    }

    // called by button
    public void SwitchDebugViews()
    {
        if (_isInDebugView == false)
        {
            DebugViewOn();
            _isInDebugView = true;
        }
        else
        {
            DebugViewOff();
            _isInDebugView = false;
        }
    }

    private void DebugViewOn()
    {
        foreach (Tile tile in _boardTiles.Values)
        {
            if (tile.isOccupied == false)
            {
                tile.SetTileColorStatus(2);
            }
            else
            {
                tile.SetTileColorStatus(3);
            }
        }
    }

    private void DebugViewOff()
    {
        foreach (Tile tile in _boardTiles.Values)
        {
            tile.SetTileColorStatus(0);
        }
    }

    // visualization of where the board will be after generation
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.matrix = _boardStartingPosition.localToWorldMatrix;
        Gizmos.DrawWireCube(Vector3.zero, new Vector3(_boardWidth, 0, _boardHeight));
    }
}
