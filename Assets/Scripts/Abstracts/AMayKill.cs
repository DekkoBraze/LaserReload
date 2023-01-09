using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AMayKill : MonoBehaviour
{
    // сюда складываются Danger тайлы этой турели
    public Tile[] dangerTiles;
    public Tile[] oldDangerTiles;

    // колличество спавнящихся Danger тайлов
    private int _trueDangerTilesNumber;

    public int dangerTilesNumber
    {
        get { return _trueDangerTilesNumber; }
        set
        {
            _trueDangerTilesNumber = value;
            if (value != 0)
            {
                dangerTiles = new Tile[value];
                oldDangerTiles = new Tile[value];
            }
        }
    }

    public virtual void Click(Tile tile)
    {
        if (Manager.playerLink.EnemyHitCheck(tile.gameObject.transform.position))
        {
            Manager.playerLink.StartPrivateCoroutine();
            int dangersNum = dangerTilesNumber;
            Tile[] dangers = dangerTiles;
            // смена хода для двигающихся тайлов
            Manager.stepCount++;
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
            dangerTilesNumber = 0;
            // изменение типа врага на Empty
            tile.gameObject.AddComponent<EmptyState>();
            Destroy(tile.gameObject.GetComponent<TurretState>());
            Destroy(tile.gameObject.GetComponent<Animator>());
            tile.state = GetComponent<EmptyState>();
            tile.state.ChangeOnSafe(tile);
            tile.gameObject.transform.eulerAngles = Manager.angle0.angleCoord;
            Messenger.Broadcast(GameEvent.NEXT_STEP);
            Messenger.Broadcast(GameEvent.DANGER_SPAWN);
        }
    }

    public void DangerTilesSpawn(Tile tile)
    {
        int dangersNum = dangerTilesNumber;
        // _isEnemyHere нужно для того, чтобы враги перекрывали Danger "лучи"
        bool _isEnemyHere = false;
        for (int tileNum = 1; tileNum <= dangersNum; tileNum++)
        {
            Vector2 pos = tile.angle.TilePos(tile.gameObject.transform.position, tileNum);
            DangerTilePlace(pos, tileNum, out _isEnemyHere);
            if (_isEnemyHere)
            {
                break;
            }
        }
    }

    public void DangerTilePlace(Vector2 pos, int i, out bool isEnemyHere)
    {
        bool canPlaceTile = false;
        isEnemyHere = false;
        // проверка тайлов на нужной позиции с помощью рейкаста
        RaycastHit2D[] hits = Physics2D.CircleCastAll(pos, 0.1f, new Vector2(0, 0));
        foreach (RaycastHit2D obj in hits)
        {
            if (obj.collider.gameObject.GetComponent<Tile>().state.GetType().ToString() == "TurretState")
            {
                canPlaceTile = false;
                isEnemyHere = true;
                break;
            }
            else
            {
                // при такой структуре свойство Danger будет второй раз даваться Danger тайлам, однако прошлая реализация, которая подразумевала
                // проверку на Danger, приводила к багам, поэтому я решил пожертвовать этой малозначительной деталью в угоду целостности своих нервных клеток
                canPlaceTile = true;
            }
        }
        if (canPlaceTile)
        {
            Tile hitTile = hits[0].collider.gameObject.GetComponent<Tile>();
            hitTile.state.ChangeOnDanger(hitTile);
            dangerTiles[i - 1] = hitTile;
            // проверка того, стоит ли игрок на изменяемом тайле и gameOver в случае true
            Vector2 player_pos = Manager.playerLink.transform.position;
            if (pos == player_pos)
            {
                Manager.link.OnPlayerDestroy();
                Destroy(Manager.playerLink.gameObject);
            }
        }
    }

    public void ChangeOnDanger(Tile tile) { }
    public void ChangeOnSafe(Tile tile) { }
}
