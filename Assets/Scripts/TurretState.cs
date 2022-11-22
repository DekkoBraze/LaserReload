using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretState : MonoBehaviour, IState
{
    Tile tile;
    public void SetTile(Tile linkedTile)
    {
        tile = linkedTile;
    }
    public void SetListeners()
    {
        Messenger.AddListener(GameEvent.DANGER_TILES_UPDATE, DangerTilesSpawn);
    }
    public void Click()
    {
        if (Manager.playerLink.EnemyHitCheck(this.gameObject.transform.position))
        {
            int dangersNum = tile.GetDangerTilesNumber();
            Tile[] dangers = tile.GetDangerTilesArray();
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
            tile.SetDangerTilesNumber(0);
            // изменение типа врага на Empty
            tile.state = Manager.link.emptyState;
            tile.SetSprite(0);
            Messenger.Broadcast(GameEvent.DANGER_TILES_UPDATE);
        }
    }
    public void SpriteUpdate()
    {
        tile.SetSprite(4);
    }
    public void DangerTilesNumberUpdate()
    {
        tile.SetDangerTilesNumber(2);
    }
    public void DangerTilesSpawn() 
    {
        int dangersNum = tile.GetDangerTilesNumber();
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
    public void NextMove() { }
    public void DestroyListeners() 
    {
        Messenger.RemoveListener(GameEvent.DANGER_TILES_UPDATE, DangerTilesSpawn);
    }
    public void CheckMovableTurretMove() { }
}