using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretState : IState
{
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
                dangers[i].gameObject.GetComponent<Tile>().state = Manager.link.emptyState;
                dangers[i].gameObject.GetComponent<SpriteRenderer>().sprite = Manager.link.tileSprites[0];
                dangers[i] = null;
                }
            }
            tile._dangerTilesNumber = 0;
            // ��������� ���� ����� �� Empty
            tile.state = Manager.link.emptyState;
            tile.SetSprite(0);
            Messenger.Broadcast(GameEvent.DANGER_TILES_UPDATE);
        }
    }
    public void SpriteUpdate(Tile tile)
    {
        tile.SetSprite(4);
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

    public void PlaceTile(Tile tile) { }
    public void NextMove(Tile tile) { }
    public void ChangeStateOnDanger(Tile hit) { }
    public void ChangeStateOnSafe(Tile tile) { }
}