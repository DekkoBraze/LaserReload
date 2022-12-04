using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalState : IState
{
    private int spriteNum = 1;
    public void Click(Tile tile)
    {
        Vector2 tilePos = tile.transform.position;
        if (Manager.playerLink.MoveCheck(tilePos))
        {
            Manager.link.clickedTile = tile;
            Manager.playerLink.PlayerChangePosition(tilePos);
<<<<<<< Updated upstream
            // смена хода для двигающихся тайлов
            Manager.stepCount++;
            Messenger.Broadcast(GameEvent.NEXT_STEP);
            Manager.link.CompleteTextAppear();
            Manager.link.isItOver = true;
            if (Manager.playerLink.energy < 4)
=======
            base.Click(tile);
            if (!isDanger && !Manager.link.isItOver)
>>>>>>> Stashed changes
            {
                Manager.playerLink.energy++;
                Manager.link.EnergyUpdate();
            }
            Messenger.Broadcast(GameEvent.CHECK_MOVABLE_TURRET);
            // уничтожение предыдущего тайла под игроком и установка нового
            Manager.playerLink.PlayerTileChange(tile);
        }
    }
    public void SpriteUpdate(Tile tile)
    {
        tile.SetSprite(spriteNum);
    }
    public void DangerTilesNumberUpdate(Tile tile)
    {
        tile._dangerTilesNumber = 0;
    }
    public void ChangeOnDanger(Tile tile)
    {
        tile.isDanger = true;
        tile.SetDangerSprite(spriteNum);
    }
    public void ChangeOnSafe(Tile tile)
    {
        tile.isDanger = false;
        tile.SetSprite(spriteNum);
    }
    public void DangerTilesSpawn(Tile tile) { }
    public void NextMove(Tile tile) { }
    public void CheckMovableTurretMove(Tile tile) { }
    public int GetSpriteNum()
    {
        return spriteNum;
    }
}
