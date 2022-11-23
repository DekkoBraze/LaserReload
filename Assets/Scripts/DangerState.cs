using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DangerState : MonoBehaviour, IState
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
            Manager.link.OnPlayerDestroy();
            Manager.playerLink.PlayerDestroy();
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
        tile.SetSprite(1);
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