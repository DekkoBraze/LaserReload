using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretState : IState
{
    private int spriteNum = 2;
    public void Click(Tile tile)
    {
        if (Manager.playerLink.EnemyHitCheck(tile.gameObject.transform.position))
        {
            int dangersNum = tile._dangerTilesNumber;
            Tile[] dangers = tile._dangerTiles;
            // ����� ���� ��� ����������� ������
            Manager.stepCount++;
            Messenger.Broadcast(GameEvent.NEXT_STEP);
            // ����������� Danger ������ �����
            for (int i = 0; i < dangersNum; i++)
            {
                if (dangers[i] != null)
                {
                    Tile dangerTile = dangers[i].gameObject.GetComponent<Tile>();
                    dangerTile.state.ChangeOnSafe(dangerTile);
                    dangers[i] = null;
                }
            }
            tile._dangerTilesNumber = 0;
            // ��������� ���� ����� �� Empty
            tile.state = Manager.emptyState;
            tile.SetSprite(0);
            Messenger.Broadcast(GameEvent.DANGER_TILES_UPDATE);
        }
    }
    public void SpriteUpdate(Tile tile)
    {
        tile.SetSprite(spriteNum);
    }
    public void DangerTilesNumberUpdate(Tile tile)
    {
        tile._dangerTilesNumber = 2;
    }
    public void DangerTilesSpawn(Tile tile) 
    {
        int dangersNum = tile._dangerTilesNumber;
        // _isEnemyHere ����� ��� ����, ����� ����� ����������� Danger "����"
        bool _isEnemyHere = false;
        for (int tileNum = 1; tileNum <= dangersNum; tileNum++)
        {
            Vector2 pos = tile.angle.TilePos(tile.gameObject.transform.position, tileNum);
            tile.DangerTilePlace(pos, tileNum, out _isEnemyHere);
            if (_isEnemyHere)
            {
                break;
            }
        }
    }
    public void ChangeOnDanger(Tile tile) { }
    public void ChangeOnSafe(Tile tile) { }
    public void NextMove(Tile tile) { }
    public void CheckMovableTurretMove(Tile tile) { }
    public int GetSpriteNum()
    {
        return spriteNum;
    }
}