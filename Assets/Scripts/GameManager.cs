using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private Board _playerBoard, _opponentBoard;

    [SerializeField]
    private Piece[] _pieces;

    private List<Tile> _tilesAICanAttack;

    [SerializeField]
    private OpponentAIBase _opponentAI;

    [SerializeField]
    private GameObject _hitMarker, _missMarker;

    private int _playerHitCount = 0, _opponentHitCount = 0;

    [SerializeField]
    private UIManager _uiManager;

    [SerializeField]
    private CameraManager _cameraManager;

    private void Awake()
    {
        _tilesAICanAttack = new List<Tile>();
    }

    private void OnEnable()
    {
        Piece.PieceHasBeenPlaced += CheckIfAllPiecesArePlaced;
        Tile.TileHasBeenSelected += AttackOpponentTile;
    }

    private void OnDisable()
    {
        Piece.PieceHasBeenPlaced -= CheckIfAllPiecesArePlaced;
        Tile.TileHasBeenSelected -= AttackOpponentTile;
    }

    // called from button
    public void StartPiecePlacementPhase()
    {
        _uiManager.StartPiecePlacementPhaseUI();

        PlaceOpponentPieces();

        SetPieceInteractability(true);
    }

    private void PlaceOpponentPieces()
    {
        InvisiblePiecePlacer.PlacePiece(_opponentBoard, 5);
        InvisiblePiecePlacer.PlacePiece(_opponentBoard, 4);
        InvisiblePiecePlacer.PlacePiece(_opponentBoard, 3);
        InvisiblePiecePlacer.PlacePiece(_opponentBoard, 3);
        InvisiblePiecePlacer.PlacePiece(_opponentBoard, 2);
    }

    private void SetPieceInteractability(bool newValue)
    {
        for (int i = 0; i < _pieces.Length; i++)
        {
            _pieces[i].isDraggable = newValue;
        }
    }

    private void CheckIfAllPiecesArePlaced()
    {
        for (int i = 0; i < _pieces.Length; i++)
        {
            if (_pieces[i].IsPiecePlaced() == false)
            {
                return;
            }
        }

        // if all pieces are placed, start next phase
        StartTurnTakingPhase();
    }

    private void StartTurnTakingPhase()
    {
        _uiManager.StartTurnTakingPhaseUI();

        SetPieceInteractability(false);

        _cameraManager.SwitchCameras(1);

        for (int i = 0; i < _opponentBoard.GetBoardWidth(); i++)
        {
            for (int j = 0; j < _opponentBoard.GetBoardHeight(); j++)
            {
                _tilesAICanAttack.Add(_playerBoard.GetTile(new Vector2Int(i, j)));
            }
        }

        StartPlayerTurn();
    }

    private void StartPlayerTurn()
    {
        _opponentBoard.SetIsBoardInteractable(true);
    }

    private void AttackOpponentTile(Tile attackedTile)
    {
        // if the tile has been attacked before, do nothing
        if (attackedTile.hasBeenAttacked == true)
        {
            return;
        }

        attackedTile.hasBeenAttacked = true;

        // if the tile is unoccupied, apply a miss marker
        if (attackedTile.isOccupied == false)
        {
            ApplyMissMarkerToTile(attackedTile);

            StartOpponentTurn();
        }
        // if the tile is occupied, apply a hit marker
        else
        {
            ApplyHitMarkerToOpponentTile(attackedTile);

            _playerHitCount++;

            if (_playerHitCount >= 17)
            {
                TriggerVictory();
            }
            else
            {
                StartOpponentTurn();
            }
        }
    }

    private void StartOpponentTurn()
    {
        _opponentBoard.SetIsBoardInteractable(false);

        Tile attackedTile = _opponentAI.ChooseTileToAttack(_tilesAICanAttack);

        if (attackedTile.isOccupied == false)
        {
            ApplyMissMarkerToTile(attackedTile);

            StartPlayerTurn();
        }
        else
        {
            ApplyHitMarkerToPlayerTile(attackedTile);

            _opponentHitCount++;

            if (_opponentHitCount == 17)
            {
                TriggerDefeat();
            }
            else
            {
                StartPlayerTurn();
            }
        }
    }

    private void ApplyHitMarkerToOpponentTile(Tile tile)
    {
        GameObject newMarker = Instantiate(_hitMarker, tile.gameObject.transform);

        newMarker.transform.localPosition = new Vector3(0, 0, -.25f);
        newMarker.transform.localEulerAngles = new Vector3(-90, 0, 0);
    }

    private void ApplyHitMarkerToPlayerTile(Tile tile)
    {
        GameObject newMarker = Instantiate(_hitMarker, tile.gameObject.transform);

        newMarker.transform.localPosition = new Vector3(0, 0, -.6f);
        newMarker.transform.localEulerAngles = new Vector3(-90, 0, 0);
    }

    private void ApplyMissMarkerToTile(Tile tile)
    {
        GameObject newMarker = Instantiate(_missMarker, tile.gameObject.transform);

        newMarker.transform.localPosition = new Vector3(0, 0, -.25f);
        newMarker.transform.localEulerAngles = new Vector3(-90, 0, 0);
    }

    private void TriggerVictory()
    {
        _opponentBoard.SetIsBoardInteractable(false);

        _uiManager.TriggerVictoryUI();
    }

    private void TriggerDefeat()
    {
        _opponentBoard.SetIsBoardInteractable(false);

        _uiManager.TriggerDefeatUI();
    }
}
