using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;

    public IState state;

    // сюда складываются Danger тайлы этой турели
    private Tile[] _dangerTiles;
    private Tile[] _oldDangerTiles;
    public Vector2[] _teleportTiles;
    
    // колличество спавнящихся Danger тайлов
    private int _dangerTilesNumber = 0;

    // константы углов для спавна Danger
    public readonly Vector3 angle0 = new Vector3(0, 0, 0);
    public readonly Vector3 angle90 = new Vector3(0, 0, 90);
    public readonly Vector3 angle180 = new Vector3(0, 0, 180);
    public readonly Vector3 angle270 = new Vector3(0, 0, 270);

    // перечисление типов тайла (по-умолчанию Empty)
    public enum TileType
    {
        EmptyTile = 0,
        Danger = 1,
        Portal = 2,
        DangerPortal = 3,
        Turret = 4,
        MovableTurret = 5,
        Rail = 6
    }
    public TileType type = TileType.EmptyTile;

    private void Awake()
    {
        Messenger.AddListener(GameEvent.DANGER_TILES_UPDATE, DangerTilesSpawn);
        Messenger.AddListener(GameEvent.NEXT_STEP, NextStep);
    }

    private void Start()
    {   
        _spriteRenderer = GetComponent<SpriteRenderer>();

        if (type == TileType.Turret || type == TileType.MovableTurret)
        {
            _dangerTilesNumber = 2;
        }

        _dangerTiles = new Tile[_dangerTilesNumber];
        if (type == TileType.MovableTurret)
        {
            _oldDangerTiles = new Tile[_dangerTilesNumber];
        }

        Debug.Log(Manager.link);
        _spriteRenderer.sprite = Manager.link.tileSprites[(int)type];

        DangerTilesSpawn();
    }

    public void OnMouseDown()
    {
        if (Manager.playerLink != null)
        {
            if (type == TileType.Turret || type == TileType.MovableTurret)
            {
                EnemyClick();
            }
            else if (type == TileType.EmptyTile || type == TileType.Danger || type == TileType.Portal || type == TileType.DangerPortal)
            {
                
                Manager.playerLink.NonEnemyClick(this);
            }
        }
    }

    private void EnemyClick()
    {
        if (Manager.playerLink.EnemyHitCheck(this.gameObject.transform.position))
        {
            // смена хода для двигающихся тайлов
            Manager.stepCount++;
            Messenger.Broadcast(GameEvent.NEXT_STEP);
            // уничтожение Danger тайлов врага
            for (int i = 0; i < _dangerTilesNumber; i++)
            {
                DestroyDangerTile(this, _dangerTiles[i], i);
            }
            _dangerTilesNumber = 0;
            // изменение типа врага на Empty
            type = TileType.EmptyTile;
            _spriteRenderer.sprite = Manager.link.tileSprites[(int)TileType.EmptyTile];
            Messenger.Broadcast(GameEvent.DANGER_TILES_UPDATE);
        }
    }

    public void DangerTilesSpawn()
    {
        if (type == TileType.Turret || type == TileType.MovableTurret)
        {
            // _isEnemyHere нужно для того, чтобы враги перекрывали Danger "лучи"
            bool _isEnemyHere = false;
            for (int tileNum = 1; tileNum <= _dangerTilesNumber; tileNum++)
            {
                if (transform.rotation.eulerAngles == angle0)
                {
                    Vector2 pos = new Vector2(transform.position.x - tileNum, transform.position.y);
                    DangerTilesCheck(pos, tileNum, out _isEnemyHere);
                    if (_isEnemyHere)
                    {
                        break;
                    }
                }
                else if (transform.rotation.eulerAngles == angle90)
                {
                    Vector2 pos = new Vector2(transform.position.x, transform.position.y - tileNum);
                    DangerTilesCheck(pos, tileNum, out _isEnemyHere);
                    if (_isEnemyHere)
                    {
                        break;
                    }
                }
                else if (transform.rotation.eulerAngles == angle180)
                {
                    Vector2 pos = new Vector2(transform.position.x + tileNum, transform.position.y);
                    DangerTilesCheck(pos, tileNum, out _isEnemyHere);
                    if (_isEnemyHere)
                    {
                        break;
                    }
                }
                else
                {
                    Vector2 pos = new Vector2(transform.position.x, transform.position.y + tileNum);
                    DangerTilesCheck(pos, tileNum, out _isEnemyHere);
                    if (_isEnemyHere)
                    {
                        break;
                    }
                }
            }
        }
    }

    public void DangerTilesCheck(Vector2 pos, int i, out bool isEnemyHere)
    {
        bool canPlaceTile = false;
        isEnemyHere = false;
        // проверка тайлов на нужной позиции с помощью рейкаста
        RaycastHit2D[] hits = Physics2D.CircleCastAll(pos, 0.1f, new Vector2(0, 0));
        foreach (RaycastHit2D obj in hits)
        {
            if (obj.collider.gameObject.GetComponent<Tile>().type == TileType.Turret)
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
            if (hits[0].collider.gameObject.GetComponent<Tile>().type == TileType.EmptyTile)
            {
                hits[0].collider.gameObject.GetComponent<Tile>().type = TileType.Danger;
                hits[0].collider.gameObject.GetComponent<SpriteRenderer>().sprite = Manager.link.tileSprites[(int)TileType.Danger];
            }
            else if (hits[0].collider.gameObject.GetComponent<Tile>().type == TileType.Portal)
            {
                hits[0].collider.gameObject.GetComponent<Tile>().type = TileType.DangerPortal;
                hits[0].collider.gameObject.GetComponent<SpriteRenderer>().sprite = Manager.link.tileSprites[(int)TileType.DangerPortal];
            }
            _dangerTiles[i - 1] = hits[0].collider.gameObject.GetComponent<Tile>();
            // проверка того, стоит ли игрок на изменяемом тайле и gameOver в случае true
            Vector2 player_pos = Manager.playerLink.transform.position;
            if (pos == player_pos)
            {
                Manager.link.OnPlayerDestroy();
                Destroy(Manager.playerLink.gameObject);
            }
        }
    }

    private void NextStep()
    {
        if (type == TileType.MovableTurret && _teleportTiles.Length > 0)
        {
            if (_teleportTiles[Manager.stepCount % _teleportTiles.Length] != null)
            {
                transform.position = _teleportTiles[Manager.stepCount % _teleportTiles.Length];
                for (int num = 0; num < _dangerTiles.Length; num++)
                {
                    DestroyDangerTile(this, _dangerTiles[num], num);
                    
                }
                Messenger.Broadcast(GameEvent.DANGER_TILES_UPDATE);
            }
            else
            {
                transform.position = _teleportTiles[0];
                for (int num = 0; num < _dangerTiles.Length; num++)
                {
                    DestroyDangerTile(this, _dangerTiles[num], num);
                }
                Messenger.Broadcast(GameEvent.DANGER_TILES_UPDATE);
            }
        }
    }

    public void SetSprite(int spriteNum)
    {
        _spriteRenderer.sprite = Manager.link.tileSprites[spriteNum];
    }

    public void SetDangerTilesNumber(int tilesNum)
    {
        _dangerTilesNumber = tilesNum;
        if (tilesNum != 0)
        {
            _dangerTiles = new Tile[_dangerTilesNumber];
        }
    }

    public int GetDangerTilesNumber()
    {
        return _dangerTilesNumber;
    }

    public Tile[] GetDangerTilesArray()
    {
        return _dangerTiles;
    }

    private void OnDestroy()
    {
        Messenger.RemoveListener(GameEvent.DANGER_TILES_UPDATE, DangerTilesSpawn);
        Messenger.RemoveListener(GameEvent.NEXT_STEP, NextStep);
    }

    private void DestroyDangerTile(Tile turret, Tile tile, int num)
    {
        if (tile != null)
        {
            // добавление уничтожаемого тайла в массив старых тайлов
            if (turret.type == TileType.MovableTurret)
            {
                turret._oldDangerTiles[num] = tile;
            }
            if (tile.gameObject.GetComponent<Tile>().type == TileType.Danger)
            {
                tile.gameObject.GetComponent<Tile>().type = TileType.EmptyTile;
                tile.gameObject.GetComponent<SpriteRenderer>().sprite = Manager.link.tileSprites[(int)TileType.EmptyTile];
            }
            else if (tile.gameObject.GetComponent<Tile>().type == TileType.DangerPortal)
            {
                tile.gameObject.GetComponent<Tile>().type = TileType.Portal;
                tile.gameObject.GetComponent<SpriteRenderer>().sprite = Manager.link.tileSprites[(int)TileType.Portal];
            }
            tile = null;
        }
    }

    public void CheckMovableTurretMove(Tile ClickedTile)
    {
        if (type == TileType.MovableTurret)
        {
            foreach (Tile oldPlayersPosition in _dangerTiles)
            {
                foreach (Tile newPlayersPosition in _oldDangerTiles)
                {
                    if (oldPlayersPosition == Manager.playerLink.playersTile && newPlayersPosition == ClickedTile)
                    {
                        Debug.Log("You were slashed by laser!");
                        Destroy(Manager.playerLink.gameObject);
                        Manager.link.OnPlayerDestroy();
                    }
                }
            }
        }
    }
}

//Рефакторинг:
//1. Разбить члены enum на отдельные классы с помощью паттерна State
//2. Переместить NonEnemyClick в соответствующие классы
//3. Попытаться отвязать как можно больше объектов друг от друга
//4. Решить проблему того, что с каждым новым пополнением списка тайлов приходится менять список спрайтов
//5. Подумать над тем, чтобы сделать отдельные состояния и для углов поворота