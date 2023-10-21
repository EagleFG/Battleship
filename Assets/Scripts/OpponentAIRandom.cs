using System.Collections.Generic;
using UnityEngine;

public class OpponentAIRandom : OpponentAIBase
{
    public override Tile ChooseTileToAttack(List<Tile> _tilesAICanAttack)
    {
        int randomChoice = Random.Range(0, _tilesAICanAttack.Count);

        Tile attackedTile = _tilesAICanAttack[randomChoice];
        _tilesAICanAttack.RemoveAt(randomChoice);

        attackedTile.hasBeenAttacked = true;
        return attackedTile;
    }
}
