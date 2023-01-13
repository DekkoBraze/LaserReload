using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalState : ACantKill, IState
{
    public Sprite tileSprite;
    public Sprite dangerTileSprite;

    public void StateStart() { }

    public override void Click(Tile tile)
    {
        Vector2 tilePos = tile.transform.position;
        if (Manager.playerLink.MoveCheck(tilePos))
        {
            Manager.link.clickedTile = tile;
            Manager.playerLink.PlayerChangePosition(tilePos);
            base.Click(tile);
            if (enemyLord == null && !Manager.link.isItOver)
            {
                Manager.playerLink.PlayerDisappearAnim();
                Manager.link.CompleteTextAppear();
                Manager.link.isItOver = true;
            }
        }
    }
    public void SpriteUpdate(Tile tile)
    {
        tile.SetSprite(tileSprite);
    }
    public void DangerTilesNumberUpdate(Tile tile)
    {
        dangerTilesNumber = 0;
    }
    public void ChangeOnDanger(Tile tile, GameObject enemy)
    {
        enemyLord = enemy;
        tile.SetSprite(dangerTileSprite);
    }
    public void ChangeOnSafe(Tile tile)
    {
        enemyLord = null;
        tile.SetSprite(tileSprite);
    }
    public Sprite GetSprite()
    {
        return tileSprite;
    }
    public void ChangeAngle(IAngle angle) { }
    public void StateDestroy() { }
}
