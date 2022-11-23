using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableTurretState : IState
{
    public void Click(Tile tile)
    {
        if (Manager.playerLink.EnemyHitCheck(tile.gameObject.transform.position))
        {
            int dangersNum = tile._dangerTilesNumber;
            Tile[] dangers = tile._dangerTiles;
            // смена хода для двигающихся тайлов
            Manager.stepCount++;
            Messenger.Broadcast(GameEvent.NEXT_STEP);
            // уничтожение Danger тайлов врага
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
            // изменение типа врага на Empty
            tile.state = Manager.link.emptyState;
            tile.SetSprite(0);
            Messenger.Broadcast(GameEvent.DANGER_TILES_UPDATE);
        }
    }
    public void SpriteUpdate(Tile tile)
    {
        tile.SetSprite(5);
    }
    public void DangerTilesNumberUpdate(Tile tile)
    {
        tile._dangerTilesNumber = 2;
    }
    public void DangerTilesSpawn(Tile tile)
    {
        int dangersNum = tile._dangerTilesNumber;
        // _isEnemyHere нужно для того, чтобы враги перекрывали Danger "лучи"
        bool _isEnemyHere = false;
        for (int tileNum = 1; tileNum <= dangersNum; tileNum++)
        {
            if (tile.gameObject.transform.rotation.eulerAngles == tile.angle0)
            {
                Vector2 pos = new Vector2(tile.gameObject.transform.position.x - tileNum, tile.gameObject.transform.position.y);
                //МЕТОД В ТАЙЛЕ НУЖНО ОТРЕДАКТИРОВАТЬ!!!
                tile.DangerTilePlace(pos, tileNum, out _isEnemyHere);
                if (_isEnemyHere)
                {
                    break;
                }
            }
            else if (tile.gameObject.transform.rotation.eulerAngles == tile.angle90)
            {
                Vector2 pos = new Vector2(tile.gameObject.transform.position.x, tile.gameObject.transform.position.y - tileNum);
                tile.DangerTilePlace(pos, tileNum, out _isEnemyHere);
                if (_isEnemyHere)
                {
                    break;
                }
            }
            else if (tile.gameObject.transform.rotation.eulerAngles == tile.angle180)
            {
                Vector2 pos = new Vector2(tile.gameObject.transform.position.x + tileNum, tile.gameObject.transform.position.y);
                tile.DangerTilePlace(pos, tileNum, out _isEnemyHere);
                if (_isEnemyHere)
                {
                    break;
                }
            }
            else
            {
                Vector2 pos = new Vector2(tile.gameObject.transform.position.x, tile.gameObject.transform.position.y + tileNum);
                tile.DangerTilePlace(pos, tileNum, out _isEnemyHere);
                if (_isEnemyHere)
                {
                    break;
                }
            }
        }
    }
    public void NextMove(Tile tile) 
    {
        Vector2[] teleports = tile._teleportTiles;
        Tile[] dangers = tile._dangerTiles;

        if (teleports.Length > 0)
        {
            if (teleports[Manager.stepCount % teleports.Length] != null)
            {
                tile.gameObject.transform.position = teleports[Manager.stepCount % teleports.Length];
            }
            else
            {
                tile.gameObject.transform.position = teleports[0];
            }
            for (int num = 0; num < dangers.Length; num++)
            {
                if (dangers[num] != null)
                {
                    tile._oldDangerTiles[num] = tile;
                    dangers[num].state.ChangeStateOnSafe(dangers[num]);
                    dangers[num] = null;
                }
            }
            Messenger.Broadcast(GameEvent.DANGER_TILES_UPDATE);
        }
    }
    public void ChangeStateOnDanger(Tile hit) { }
    public void ChangeStateOnSafe(Tile tile) { }
}
