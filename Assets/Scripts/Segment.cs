using UnityEngine;

public class Segment : MonoBehaviour
{
    private Tile _occupiedTile = null;

    [SerializeField]
    private Material[] _materials;

    [SerializeField]
    private MeshRenderer _renderer;

    enum IndicatorStatus { Confirmed, Rejected };
    private IndicatorStatus _status = IndicatorStatus.Rejected;

    private bool _isAssessingTileBeneath = false;

    private Tile _currentTileBeingAssessed;
    private Tile _previousTileAssessed;

    private void Update()
    {
        if (_isAssessingTileBeneath)
        {
            AssessTileBeneath();
        }
    }

    public bool GetIsAssessingTileBeneath()
    {
        return _isAssessingTileBeneath;
    }

    public void SetIsAssessingTileBeneath(bool newValue)
    {
        _isAssessingTileBeneath = newValue;
        _renderer.enabled = newValue;

        if (newValue == false)
        {
            ResetTileMaterial(_currentTileBeingAssessed);
        }
    }

    private void AssessTileBeneath()
    {
        RaycastHit hit;

        if (Physics.Raycast(gameObject.transform.position, Vector3.down, out hit, 1, LayerMask.GetMask("Tile")))
        {
            if (hit.collider.gameObject.TryGetComponent(out Tile tile))
            {
                _currentTileBeingAssessed = tile;

                // reset previous tile if a new tile is being assessed
                if (_currentTileBeingAssessed != _previousTileAssessed && _previousTileAssessed != null)
                {
                    ResetTileMaterial(_previousTileAssessed);
                }

                if (_currentTileBeingAssessed.isOccupied == false)
                {
                    ConfirmAssessedTile(_currentTileBeingAssessed);
                }
                else
                {
                    RejectAssessedTile(_currentTileBeingAssessed);
                }

                _previousTileAssessed = _currentTileBeingAssessed;
            }
        }
        // if no tile is beneath the indicator
        else
        {
            if (_previousTileAssessed != null)
            {
                SetIndicatorStatus(IndicatorStatus.Rejected);

                ResetTileMaterial(_previousTileAssessed);
                _previousTileAssessed = null;
            }
        }
    }

    // To ensure proper material switching, material array should contain Placement Indicator materials in this order:
    // Confirmed = 0, Rejected = 1
    private void SetIndicatorStatus(IndicatorStatus newStatus)
    {
        _status = newStatus;

        switch (_status)
        {
            case IndicatorStatus.Confirmed:
                SetMaterial(0);
                break;
            case IndicatorStatus.Rejected:
                SetMaterial(1);
                break;
        }
    }

    private void SetMaterial(int materialIndex)
    {
        _renderer.material = _materials[materialIndex];
    }

    private void ConfirmAssessedTile(Tile tile)
    {
        tile.SetTileColorStatus(2);

        SetIndicatorStatus(IndicatorStatus.Confirmed);
    }

    private void RejectAssessedTile(Tile tile)
    {
        tile.SetTileColorStatus(3);

        SetIndicatorStatus(IndicatorStatus.Rejected);
    }

    private void ResetTileMaterial(Tile tile)
    {
        tile?.SetTileColorStatus(0);
    }

    public bool IsPlaceable()
    {
        if (_status == IndicatorStatus.Confirmed)
        {
            return true;
        }

        return false;
    }

    public Tile GetOccupiedTile()
    {
        return _occupiedTile;
    }

    public void SetOccupiedTile(string pieceName)
    {
        _occupiedTile = _currentTileBeingAssessed;

        _occupiedTile.isOccupied = true;
        _occupiedTile.occupyingPieceName = pieceName;
    }

    public void RemoveOccupiedTile()
    {
        if (_occupiedTile != null)
        {
            _occupiedTile.isOccupied = false;
            _occupiedTile.occupyingPieceName = "";

            _occupiedTile = null;
        }
    }
}
