using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RailState : IState
{
    private int spriteNum = 4;
    public void Click(Tile tile) { }
    public void SpriteUpdate(Tile tile)
    {
        tile.SetSprite(spriteNum);
    }
    public void DangerTilesNumberUpdate(Tile tile)
    {
        tile._dangerTilesNumber = 0;
    }
    public void DangerTilesSpawn(Tile tile) { }
    public void NextMove(Tile tile) { }
    public void ChangeOnDanger(Tile tile) { }
    public void ChangeOnSafe(Tile tile) { }
    public void CheckMovableTurretMove(Tile tile) { }
    public int GetSpriteNum()
    {
        return spriteNum;
    }
}

