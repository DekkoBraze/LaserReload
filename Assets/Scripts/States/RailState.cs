using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RailState : ACantKill, IState
{
    public Sprite tileSprite;

    public override void Click(Tile tile) { }
    public void SpriteUpdate(Tile tile)
    {
        tile.SetSprite(tileSprite);
    }
    public void DangerTilesNumberUpdate(Tile tile)
    {
        dangerTilesNumber = 0;
    }
    public void ChangeOnDanger(Tile tile) { }
    public void ChangeOnSafe(Tile tile) { }
    public Sprite GetSprite()
    {
        return tileSprite;
    }
}
