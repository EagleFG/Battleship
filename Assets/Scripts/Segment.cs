using UnityEngine;

public class Segment : MonoBehaviour
{
    [SerializeField]
    private PlacementIndicator _placementIndicator;

    [SerializeField]
    private Tile _occupiedTile;

    public void EnablePlacementIndicator()
    {
        _placementIndicator.enabled = true;
    }

    public void DisablePlacementIndicator()
    {
        _placementIndicator.enabled = false;
    }

    public bool IsPlaceable()
    {
        return _placementIndicator.IsCurrentlyAssessedTilePlaceable();
    }

    public Tile GetOccupiedTile()
    {
        return _occupiedTile;
    }

    public void SetPlacementOnBoard()
    {
        _occupiedTile = _placementIndicator.GetCurrentTileBeingAssessed();

        _occupiedTile.SetOccupiedStatus(true);
    }

    public void RemovePlacementOnBoard()
    {
        if (_occupiedTile != null)
        {
            _occupiedTile.SetOccupiedStatus(false);

            _occupiedTile = null;
        }
    }
}