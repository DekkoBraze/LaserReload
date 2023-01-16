using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ACantKill : MonoBehaviour
{
    // ��������� ����, ������� ������ ������ ��������
    public Tile enemyLord;

    // ����������� ����������� Danger ������
    public int dangerTilesNumber = 0;

    public virtual void Click(Tile tile)
    {
        Messenger.Broadcast(GameEvent.STEP_SOUND);
        // ����� ���� ��� ����������� ������
        Manager.stepCount++;
        Messenger.Broadcast(GameEvent.NEXT_STEP);
        Messenger.Broadcast(GameEvent.DANGER_SPAWN);
        if (enemyLord != null)
            {
            Messenger.Broadcast(GameEvent.ENEMY_SHOOT_SOUND);
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
        // ����������� ����������� ����� ��� ������� � ��������� ������
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
