using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalState : MonoBehaviour, IState
{
    Tile tile;
    public void SetTile(Tile linkedTile)
    {
        tile = linkedTile;
    }
    public void Click()
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
    public void SpriteUpdate()
    {
        tile.SetSprite(2);
    }
    public void DangerTilesNumberUpdate()
    {
        tile._dangerTilesNumber = 0;
    }
    public void SetListeners() { }
    public void DangerTilesSpawn() { }
    public void NextMove() { }
    public void DestroyListeners() { }
    public void CheckMovableTurretMove(Tile clickedTile) { }
}
