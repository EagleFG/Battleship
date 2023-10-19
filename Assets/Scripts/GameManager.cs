using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private Board _playerBoard, _opponentBoard;

    [SerializeField]
    private Piece[] _pieces;

    [SerializeField]
    private CameraManager _cameraManager;

    private void OnEnable()
    {
        Piece.PieceHasBeenPlaced += CheckIfAllPiecesArePlaced;
    }

    private void OnDisable()
    {
        Piece.PieceHasBeenPlaced -= CheckIfAllPiecesArePlaced;
    }

    // temporarily called from button
    public void StartPiecePlacementPhase()
    {
        _playerBoard.SetIsBoardInteractable(true);

        SetPieceInteractability(true);
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

        StartTurnTakingPhase();
    }

    private void StartTurnTakingPhase()
    {
        SetPieceInteractability(false);
        _playerBoard.SetIsBoardInteractable(false);

        _opponentBoard.SetIsBoardInteractable(true);
        _cameraManager.SwitchCameras(1);
    }

    private void SetPieceInteractability(bool newValue)
    {
        for (int i = 0; i < _pieces.Length; i++)
        {
            _pieces[i].SetIsDraggable(newValue);
        }
    }
}
