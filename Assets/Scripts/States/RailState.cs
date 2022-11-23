using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RailState : IState
{
    public void Click(Tile tile) { }
    public void SpriteUpdate(Tile tile)
    {
        tile.SetSprite(6);
    }
    public void DangerTilesNumberUpdate(Tile tile)
    {
        tile._dangerTilesNumber = 0;
    }
    public void DangerTilesSpawn(Tile tile) { }
    public void NextMove(Tile tile) { }
    public void ChangeStateOnDanger(Tile hit) { }
    public void ChangeStateOnSafe(Tile tile) { }
}

