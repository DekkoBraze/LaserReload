using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptyState : ACantKill, IState
{
    public Sprite tileSprite;
    public Sprite dangerTileSprite;

    public override void Click(Tile tile)
    {
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
    public void ChangeOnDanger(Tile tile)
    {
        spriteCheck();
        isDanger = true;
        tile.SetSprite(dangerTileSprite);
    }
    public void ChangeOnSafe(Tile tile)
    {
        spriteCheck();
        isDanger = false;
        tile.SetSprite(tileSprite);
    }

    public Sprite GetSprite()
    {
        return tileSprite;
    }

    private void spriteCheck()
    {
        if (tileSprite == null)
        {
            tileSprite = Manager.link.emtyTileSprite;
        }
        if (dangerTileSprite == null)
        {
            dangerTileSprite = Manager.link.dangerEmptyTileSprite;
        }
    }
}
