using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private Player _player;
    private SpriteRenderer _spriteRenderer;
    private Manager _manager;
    // сюда складываются Danger тайлы этой турели
    private Tile[] _dangerTiles;
    // колличество спавнящихся Danger тайлов
    private int _dangerTilesNumber = 0;

    // константы углов для спавна Danger
    private readonly Vector3 angle0 = new Vector3(0, 0, 0);
    private readonly Vector3 angle90 = new Vector3(0, 0, 90);
    private readonly Vector3 angle180 = new Vector3(0, 0, 180);
    private readonly Vector3 angle270 = new Vector3(0, 0, 270);

    // перечисление типов тайла (по-умолчанию Empty)
    public enum TileType
    {
        EmptyTile = 0,
        Danger = 1,
        Portal = 2,
        DangerPortal = 3,
        Turret = 4
    }
    public TileType type = TileType.EmptyTile;

    private void Awake()
    {
        Messenger.AddListener(GameEvent.DANGER_TILES_UPDATE, DangerTilesSpawn);
    }

    private void Start()
    {
        _player = FindObjectOfType<Player>();
        _manager = FindObjectOfType<Manager>();
        _spriteRenderer = GetComponent<SpriteRenderer>();

        if (type == TileType.Turret)
        {
            _dangerTilesNumber = 2;
        }

        _dangerTiles = new Tile[_dangerTilesNumber];

        _spriteRenderer.sprite = _manager.tileSprites[(int)type];

        DangerTilesSpawn();
    }

    public void OnMouseDown()
    {
        if (_player != null)
        {
            if (type == TileType.Turret)
            {
                EnemyClick();
            }
            else if (type == TileType.EmptyTile || type == TileType.Danger || type == TileType.Portal || type == TileType.DangerPortal)
            {
                _player.NonEnemyClick(this);
            }
        }
    }

    private void EnemyClick()
    {
        if (_player.EnemyHitCheck(this.gameObject.transform.position))
        {
            // уничтожение Danger тайлов врага
            for (int i = 0; i < _dangerTilesNumber; i++)
            {
                if (_dangerTiles[i] != null)
                {
                    if (_dangerTiles[i].gameObject.GetComponent<Tile>().type == TileType.Danger)
                    {
                        _dangerTiles[i].gameObject.GetComponent<Tile>().type = TileType.EmptyTile;
                        _dangerTiles[i].gameObject.GetComponent<SpriteRenderer>().sprite = _manager.tileSprites[(int)TileType.EmptyTile];
                    }
                    else if (_dangerTiles[i].gameObject.GetComponent<Tile>().type == TileType.DangerPortal)
                    {
                        _dangerTiles[i].gameObject.GetComponent<Tile>().type = TileType.Portal;
                        _dangerTiles[i].gameObject.GetComponent<SpriteRenderer>().sprite = _manager.tileSprites[(int)TileType.Portal];
                    }
                    _dangerTiles[i] = null;
                }
            }
            _dangerTilesNumber = 0;
            // изменение типа врага на Empty
            type = TileType.EmptyTile;
            _spriteRenderer.sprite = _manager.tileSprites[(int)TileType.EmptyTile];
            // запрос апдейта Danger тайлов
            Messenger.Broadcast(GameEvent.DANGER_TILES_UPDATE);
        }
    }

    private void DangerTilesSpawn()
    {
        if (type == TileType.Turret)
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

    private void DangerTilesCheck(Vector2 pos, int i, out bool isEnemyHere)
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
                hits[0].collider.gameObject.GetComponent<SpriteRenderer>().sprite = _manager.tileSprites[(int)TileType.Danger];
            }
            else if (hits[0].collider.gameObject.GetComponent<Tile>().type == TileType.Portal)
            {
                hits[0].collider.gameObject.GetComponent<Tile>().type = TileType.DangerPortal;
                hits[0].collider.gameObject.GetComponent<SpriteRenderer>().sprite = _manager.tileSprites[(int)TileType.DangerPortal];
            }
            _dangerTiles[i - 1] = hits[0].collider.gameObject.GetComponent<Tile>();
            // проверка того, стоит ли игрок на изменяемом тайле и gameOver в случае true
            Vector2 player_pos = _player.transform.position;
            if (pos == player_pos)
            {
                _manager.OnPlayerDestroy();
                Destroy(_player.gameObject);
            }
        }
    }

    private void OnDestroy()
    {
        Messenger.RemoveListener(GameEvent.DANGER_TILES_UPDATE, DangerTilesSpawn);
    }
}