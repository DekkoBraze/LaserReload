using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptyState : MonoBehaviour, IState
{
    private int spriteNum = 0;
    private bool isDanger = false;

    public void Click(Tile tile)
    {
        Vector2 tilePos = tile.transform.position;
        if (Manager.playerLink.MoveCheck(tilePos))
        {
            Manager.link.clickedTile = tile;
            Manager.playerLink.PlayerChangePosition(tilePos);
            // ����� ���� ��� ����������� ������
            Manager.stepCount++;
            Messenger.Broadcast(GameEvent.NEXT_STEP);
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
        isDanger = true;
        tile.SetDangerSprite(spriteNum);
    }
    public void ChangeOnSafe(Tile tile)
    {
        isDanger = false;
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
