using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ACantKill : MonoBehaviour
{
    // Сохраняет тайл, который делает данный денжером
    public Tile enemyLord;

    // колличество спавнящихся Danger тайлов
    public int dangerTilesNumber = 0;

    public virtual void Click(Tile tile)
    {
        // смена хода для двигающихся тайлов
        Manager.stepCount++;
        Messenger.Broadcast(GameEvent.NEXT_STEP);
        Messenger.Broadcast(GameEvent.DANGER_SPAWN);
        if (enemyLord != null)
            {
            enemyLord.state.FireAnim();
            enemyLord.state.AfterPlayerDestroy();
                Manager.link.OnPlayerDestroy();
                Manager.playerLink.PlayerDestroy();
            }
        if (Manager.playerLink.energy < 4)
            {
                Manager.playerLink.energy++;
                Manager.link.EnergyUpdate();
            }
        Messenger.Broadcast(GameEvent.CHECK_MOVABLE_TURRET);
        // уничтожение предыдущего тайла под игроком и установка нового
        Manager.playerLink.PlayerTileChange(tile);
    }

    public void DangerTilesSpawn(Tile tile) { }
    public void NextMove(Tile tile) { }
    public void CheckMovableTurretMove(Tile tile) { }
    public void FireAnim() { }

    public Tile EnemyLordLink()
    {
        return enemyLord;
    }
    public virtual void AfterPlayerDestroy()
    {

    }
}
