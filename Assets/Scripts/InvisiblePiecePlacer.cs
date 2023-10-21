using UnityEngine;

public abstract class InvisiblePiecePlacer : MonoBehaviour
{
    public static void PlacePiece(Board board, int numberOfSegments)
    {
        Tile[] tilesToOccupy = new Tile[numberOfSegments];
        bool isPlacementValid = false;

        while (isPlacementValid == false)
        {
            tilesToOccupy = TryPlacePiece(board, numberOfSegments);

            if (VerifyPlacementValidity(tilesToOccupy) == true)
            {
                isPlacementValid = true;
            }
        }

        for (int i = 0; i < tilesToOccupy.Length; i++)
        {
            tilesToOccupy[i].SetOccupiedStatus(true);
        }
    }

    private static Tile[] TryPlacePiece(Board board, int numberOfSegments)
    {
        int orientation = Random.Range(0, 2);
        Tile[] _occupiedTiles = new Tile[numberOfSegments];

        // horizontal placement
        if (orientation == 0)
        {
            int startingColumn = Random.Range(0, board.GetBoardWidth() - numberOfSegments + 1);
            int row = Random.Range(0, board.GetBoardHeight());

            for (int i = 0; i < numberOfSegments; i++)
            {
                _occupiedTiles[i] = board.GetTile(new Vector2Int(startingColumn + i, row));
            }
        }
        // vertical placement
        else
        {
            int column = Random.Range(0, board.GetBoardWidth());
            int startingRow = Random.Range(0, board.GetBoardHeight() - numberOfSegments + 1);

            for (int i = 0; i < numberOfSegments; i++)
            {
                _occupiedTiles[i] = board.GetTile(new Vector2Int(column, startingRow + i));
            }
        }

        return _occupiedTiles;
    }

    private static bool VerifyPlacementValidity(Tile[] tilesToOccupy)
    {
        for (int i = 0; i < tilesToOccupy.Length; i++)
        {
            if (tilesToOccupy[i].GetOccupiedStatus() == true)
            {
                return false;
            }
        }

        return true;
    }
}
