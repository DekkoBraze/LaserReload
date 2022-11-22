using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptyState : MonoBehaviour, IState
{
    public void SetListeners() { }
    public void Click(Tile clickedTile)
    {
        Vector2 tilePos = clickedTile.transform.position;
        if (Manager.playerLink.MoveCheck(tilePos))
        {
            Manager.playerLink.PlayerChangePosition(tilePos);
            // смена хода для двигающихся тайлов
            Manager.stepCount++;
            Messenger.Broadcast(GameEvent.NEXT_STEP);
            if (Player.energy < 4)
            {
                Player.energy++;
                Manager.link.EnergyUpdate();
            }
            Manager.link.StartCheckMovableTurret(clickedTile);
            // уничтожение предыдущего тайла под игроком и установка нового
            Manager.playerLink.PlayerTileChange(clickedTile);
        }
    }
    public void SpriteUpdate()
    {

    }
    public void SetDangerTilesNumber()
    {

    }
    public void DangerTilesSpawn()
    {

    }
    public void NextStep()
    {

    }
    public void DestroyListeners()
    {

    }
    public void DestroyDangerTile()
    {

    }
    public void CheckMovableTurretMove()
    {

    }
}
