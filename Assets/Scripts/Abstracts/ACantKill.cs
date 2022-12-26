using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ACantKill : MonoBehaviour
{
    protected bool isDanger = false;

    // ����������� ����������� Danger ������
    public int dangerTilesNumber = 0;

    public virtual void Click(Tile tile)
    {
        // ����� ���� ��� ����������� ������
        Manager.stepCount++;
        Messenger.Broadcast(GameEvent.NEXT_STEP);
        Messenger.Broadcast(GameEvent.DANGER_SPAWN);
        if (isDanger)
            {
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
}
