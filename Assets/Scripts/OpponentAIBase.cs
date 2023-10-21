using System.Collections.Generic;
using UnityEngine;

public class OpponentAIBase : MonoBehaviour
{
    public virtual Tile ChooseTileToAttack(List<Tile> _tilesAICanAttack)
    {
        Tile attackedTile = _tilesAICanAttack[0];
        _tilesAICanAttack.RemoveAt(0);

        return attackedTile;
    }
}
