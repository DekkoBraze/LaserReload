using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableTurretState : MonoBehaviour, IState
{
    Tile tile;
    public void SetTile(Tile linkedTile)
    {
        tile = linkedTile;
    }
    public void SetListeners()
    {
        Messenger.AddListener(GameEvent.DANGER_TILES_UPDATE, DangerTilesSpawn);
        Messenger.AddListener(GameEvent.NEXT_STEP, NextMove);
    }
    public void Click()
    {
        if (Manager.playerLink.EnemyHitCheck(this.gameObject.transform.position))
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
    public void SpriteUpdate()
    {
        tile.SetSprite(5);
    }
    public void DangerTilesNumberUpdate()
    {
        tile._dangerTilesNumber = 2;
    }
    public void DangerTilesSpawn()
    {
        int dangersNum = tile._dangerTilesNumber;
        // _isEnemyHere нужно для того, чтобы враги перекрывали Danger "лучи"
        bool _isEnemyHere = false;
        for (int tileNum = 1; tileNum <= dangersNum; tileNum++)
        {
            if (transform.rotation.eulerAngles == tile.angle0)
            {
                Vector2 pos = new Vector2(transform.position.x - tileNum, transform.position.y);
                //МЕТОД В ТАЙЛЕ НУЖНО ОТРЕДАКТИРОВАТЬ!!!
                tile.DangerTilesCheck(pos, tileNum, out _isEnemyHere);
                if (_isEnemyHere)
                {
                    break;
                }
            }
            else if (transform.rotation.eulerAngles == tile.angle90)
            {
                Vector2 pos = new Vector2(transform.position.x, transform.position.y - tileNum);
                tile.DangerTilesCheck(pos, tileNum, out _isEnemyHere);
                if (_isEnemyHere)
                {
                    break;
                }
            }
            else if (transform.rotation.eulerAngles == tile.angle180)
            {
                Vector2 pos = new Vector2(transform.position.x + tileNum, transform.position.y);
                tile.DangerTilesCheck(pos, tileNum, out _isEnemyHere);
                if (_isEnemyHere)
                {
                    break;
                }
            }
            else
            {
                Vector2 pos = new Vector2(transform.position.x, transform.position.y + tileNum);
                tile.DangerTilesCheck(pos, tileNum, out _isEnemyHere);
                if (_isEnemyHere)
                {
                    break;
                }
            }
        }
    }
    public void NextMove() 
    {
        Vector2[] teleports = tile._teleportTiles;
        Tile[] dangers = tile._dangerTiles;

        if (teleports.Length > 0)
        {
            if (teleports[Manager.stepCount % teleports.Length] != null)
            {
                transform.position = teleports[Manager.stepCount % teleports.Length];
            }
            else
            {
                transform.position = teleports[0];
            }
            for (int num = 0; num < dangers.Length; num++)
            {
                if (dangers[num] != null)
                {
                    tile._oldDangerTiles[num] = tile;
                }
                if (dangers[num].gameObject.GetComponent<Tile>().state == Manager.link.dangerState)
                {
                    dangers[num].gameObject.GetComponent<Tile>().state = Manager.link.emptyState;
                    dangers[num].SetSprite(0);
                }
                else if (dangers[num].gameObject.GetComponent<Tile>().state == Manager.link.dangerPortalState)
                {
                    dangers[num].gameObject.GetComponent<Tile>().state = Manager.link.portalState;
                    dangers[num].SetSprite(2);
                }
                dangers[num] = null;
            }
            Messenger.Broadcast(GameEvent.DANGER_TILES_UPDATE);
        }
    }
    public void DestroyListeners()
    {
        Messenger.RemoveListener(GameEvent.DANGER_TILES_UPDATE, DangerTilesSpawn);
        Messenger.RemoveListener(GameEvent.NEXT_STEP, NextMove);
    }
    public void CheckMovableTurretMove(Tile clickedTile) 
    {
        foreach (Tile oldPlayersPosition in tile._dangerTiles)
        {
            foreach (Tile newPlayersPosition in tile._oldDangerTiles)
            {
                if (oldPlayersPosition == Manager.playerLink.playersTile && newPlayersPosition == clickedTile)
                {
                    Debug.Log("You were slashed by laser!");
                    Destroy(Manager.playerLink.gameObject);
                    Manager.link.OnPlayerDestroy();
                }
            }
        }
    }
}
