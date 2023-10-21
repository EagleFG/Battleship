using UnityEngine;

public class PlacementIndicator : MonoBehaviour
{
    [SerializeField]
    private Material[] _materials;

    [SerializeField]
    private MeshRenderer _renderer;

    enum IndicatorStatus { Confirmed, Rejected };
    private IndicatorStatus _status = IndicatorStatus.Rejected;

    private Tile _currentTileBeingAssessed;
    private Tile _previousTileAssessed;

    private void Update()
    {
        AssessTileBeneath();
    }

    private void OnEnable()
    {
        _renderer.enabled = true;
    }

    private void OnDisable()
    {
        _renderer.enabled = false;
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
                    ResetTile(_previousTileAssessed);
                }

                if (_currentTileBeingAssessed.GetOccupiedStatus() == false)
                {
                    ConfirmTile(_currentTileBeingAssessed);
                }
                else
                {
                    RejectTile(_currentTileBeingAssessed);
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

                ResetTile(_previousTileAssessed);
                _previousTileAssessed = null;
            }
        }
    }

    // To ensure proper material switching, material array should contain materials in this order:
    // Confirmed = 0, Rejected = 1
    private void SetIndicatorStatus(IndicatorStatus newStatus)
    {
        _status = newStatus;

        switch(_status)
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

    private void ConfirmTile(Tile tile)
    {
        tile.SetTileColorStatus(2);

        SetIndicatorStatus(IndicatorStatus.Confirmed);
    }

    private void RejectTile(Tile tile)
    {
        tile.SetTileColorStatus(3);

        SetIndicatorStatus(IndicatorStatus.Rejected);
    }

    public void ResetTile(Tile tile)
    {
        tile?.SetTileColorStatus(0);
    }

    public bool IsCurrentlyAssessedTilePlaceable()
    {
        if (_status == IndicatorStatus.Confirmed)
        {
            return true;
        }

        return false;
    }

    public Tile GetCurrentTileBeingAssessed()
    {
        return _currentTileBeingAssessed;
    }
}
