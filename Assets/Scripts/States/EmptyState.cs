using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptyState : IState
{
    public void Click(Tile tile)
    {
        Vector2 tilePos = tile.transform.position;
        if (Manager.playerLink.MoveCheck(tilePos))
        {
            Manager.playerLink.PlayerChangePosition(tilePos);
            // ����� ���� ��� ����������� ������
            Manager.stepCount++;
            Messenger.Broadcast(GameEvent.NEXT_STEP);
            if (Player.energy < 4)
            {
                Player.energy++;
                Manager.link.EnergyUpdate();
            }
            Manager.link.StartCheckMovableTurret(tile);
            // ����������� ����������� ����� ��� ������� � ��������� ������
            Manager.playerLink.PlayerTileChange(tile);
        }
    }
    public void SpriteUpdate(Tile tile)
    {
        tile.SetSprite(0);
    }
    public void DangerTilesNumberUpdate(Tile tile)
    {
        tile._dangerTilesNumber = 0;
    }
    public void DangerTilesSpawn(Tile tile) { }
    public void NextMove(Tile tile) { }
    public void ChangeStateOnDanger(Tile hit)
    {
        hit.state = Manager.link.dangerState;
        hit.SetSprite(1);
    }
    public void ChangeStateOnSafe(Tile tile) { }
}
