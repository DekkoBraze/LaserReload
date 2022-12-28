using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaState : ACantKill, IState
{
    public Sprite tileSprite;

    public override void Click(Tile tile)
    {
        isDanger = true;
        Vector2 tilePos = tile.transform.position;
        if (Manager.playerLink.MoveCheck(tilePos))
        {
            Manager.link.clickedTile = tile;
            Manager.playerLink.PlayerChangePosition(tilePos);
            base.Click(tile);
        }
    }

    public void DangerTilesNumberUpdate(Tile tile)
    {
        dangerTilesNumber = 0;
    }
    
    public Sprite GetSprite()
    {
        SpriteCheck();
        return tileSprite;
    }

    public void ChangeOnDanger(Tile tile) { }
    public void ChangeOnSafe(Tile tile) { }

    private void SpriteCheck()
    {
        if (tileSprite == null)
        {
            tileSprite = Manager.link.lavaTileSprite;
        }
    }
}
