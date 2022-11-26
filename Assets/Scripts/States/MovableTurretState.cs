using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableTurretState : IState
{
    private int spriteNum = 3;
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
                    Tile dangerTile = dangers[i].gameObject.GetComponent<Tile>();
                    dangerTile.state.ChangeOnSafe(dangerTile);
                    dangers[i] = null;
                }
            }
            tile._dangerTilesNumber = 0;
            // изменение типа врага на Empty
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
        // _isEnemyHere нужно для того, чтобы враги перекрывали Danger "лучи"
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
                    tile._oldDangerTiles[num] = dangers[num];
                    Tile dangerTile = dangers[num].gameObject.GetComponent<Tile>();
                    dangerTile.state.ChangeOnSafe(dangerTile);
                    dangers[num] = null;
                }
            }
            Messenger.Broadcast(GameEvent.DANGER_TILES_UPDATE);
        }
    }
    public void ChangeOnDanger(Tile tile) { }
    public void ChangeOnSafe(Tile tile) { }
    public void CheckMovableTurretMove(Tile tile)
    {
        foreach (Tile oldPlayersPosition in tile._dangerTiles)
        {
            foreach (Tile newPlayersPosition in tile._oldDangerTiles)
            {
                if (oldPlayersPosition == Manager.playerLink.playersTile && newPlayersPosition == Manager.link.clickedTile)
                {
                    Debug.Log("You were slashed by laser!");
                    Manager.playerLink.PlayerDestroy();
                    Manager.link.OnPlayerDestroy();
                    break;
                }
            }
        }
    }
    public int GetSpriteNum()
    {
        return spriteNum;
    }
}
