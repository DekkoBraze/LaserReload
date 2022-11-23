using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalState : IState
{
    public void Click(Tile tile)
    {
        Vector2 tilePos = tile.transform.position;
        if (Manager.playerLink.MoveCheck(tilePos))
        {
            Manager.playerLink.PlayerChangePosition(tilePos);
            // смена хода для двигающихся тайлов
            Manager.stepCount++;
            Messenger.Broadcast(GameEvent.NEXT_STEP);
            Manager.link.CompleteTextAppear();
            Manager.link.isItOver = true;
            if (Player.energy < 4)
            {
                Player.energy++;
                Manager.link.EnergyUpdate();
            }
            Manager.link.StartCheckMovableTurret(tile);
            // уничтожение предыдущего тайла под игроком и установка нового
            Manager.playerLink.PlayerTileChange(tile);
        }
    }
    public void SpriteUpdate(Tile tile)
    {
        tile.SetSprite(2);
    }
    public void DangerTilesNumberUpdate(Tile tile)
    {
        tile._dangerTilesNumber = 0;
    }
    public void DangerTilesSpawn(Tile tile) { }
    public void NextMove(Tile tile) { }
    public void ChangeStateOnDanger(Tile tile)
    {
        tile.state = Manager.link.dangerPortalState;
        tile.SetSprite(3);
    }
    public void ChangeStateOnSafe(Tile tile) { }
}
