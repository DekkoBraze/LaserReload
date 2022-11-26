using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;

    public IState state;
    public IAngle angle;
    public bool isDanger { get; set; }

    // сюда складываются Danger тайлы этой турели
    public Tile[] _dangerTiles;
    public Tile[] _oldDangerTiles;
    public Vector2[] _teleportTiles;
    
    // колличество спавнящихся Danger тайлов
    private int _trueDangerTilesNumber;
    public int _dangerTilesNumber 
    {
        get { return _trueDangerTilesNumber; } 
        set 
        {
            _trueDangerTilesNumber = value;
            if (value != 0)
            {
                _dangerTiles = new Tile[value];
                _oldDangerTiles = new Tile[value];
            }
        } 
    }

    // перечисление типов тайла (по-умолчанию Empty) - сделано для того, чтобы состояние можно было выбирать в инспекторе
    public enum TileType
    {
        EmptyTile = 0,
        Portal = 1,
        Turret = 2,
        MovableTurret = 3,
        Rail = 4
    }
    public TileType type;

    private void Awake()
    {
        Messenger.AddListener(GameEvent.DANGER_TILES_UPDATE, DangerTilesSpawnAwake);
        Messenger.AddListener(GameEvent.NEXT_STEP, NextMoveAwake);
        Messenger.AddListener(GameEvent.SET_STATE, SetState);
        Messenger.AddListener(GameEvent.CHECK_MOVABLE_TURRET, AwakeCheckMovableTurretMove);
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        if (transform.eulerAngles == new Vector3(0, 0, 0))
        {
            angle = Manager.angle0;
        }
        else if (transform.eulerAngles == new Vector3(0, 0, 90))
        {
            angle = Manager.angle90;
        }
        else if (transform.eulerAngles == new Vector3(0, 0, 180))
        {
            angle = Manager.angle180;
        }
        else
        {
            angle = Manager.angle270;
        }
        state.DangerTilesNumberUpdate(this);
        state.DangerTilesSpawn(this);
    }

    public void OnMouseDown()
    {
        if (Manager.playerLink != null)
        {
            state.Click(this);
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
            if (obj.collider.gameObject.GetComponent<Tile>().state == Manager.turretState)
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
            _dangerTiles[i - 1] = hitTile;
            // проверка того, стоит ли игрок на изменяемом тайле и gameOver в случае true
            Vector2 player_pos = Manager.playerLink.transform.position;
            if (pos == player_pos)
            {
                Manager.link.OnPlayerDestroy();
                Destroy(Manager.playerLink.gameObject);
            }
        }
    }

    public void SetSprite(int spriteNum)
    {
        _spriteRenderer.sprite = Manager.link.tileSprites[spriteNum];
    }

    public void SetDangerSprite(int spriteNum)
    {
        _spriteRenderer.sprite = Manager.link.dangerTileSprites[spriteNum];
    }

    private void SetState()
    {
        state = Manager.link.states[(int)type];
        SetSprite(state.GetSpriteNum());
        isDanger = false;
    }

    private void DangerTilesSpawnAwake()
    {
        state.DangerTilesSpawn(this);
    }

    private void NextMoveAwake()
    {
        state.NextMove(this);
    }

    public void AwakeCheckMovableTurretMove()
    {
        state.CheckMovableTurretMove(this);
    }

    private void OnDestroy()
    {
        Messenger.RemoveListener(GameEvent.DANGER_TILES_UPDATE, DangerTilesSpawnAwake);
        Messenger.RemoveListener(GameEvent.NEXT_STEP, NextMoveAwake);
        Messenger.RemoveListener(GameEvent.SET_STATE, SetState);
        Messenger.RemoveListener(GameEvent.CHECK_MOVABLE_TURRET, AwakeCheckMovableTurretMove); 
    }
}

//Рефакторинг:
//1. Разбить члены enum на отдельные классы с помощью паттерна State - ВЫПОЛНЕНО
//2. Переместить NonEnemyClick в соответствующие классы - ВЫПОЛНЕНО
//3. Попытаться отвязать как можно больше объектов друг от друга - ВЫПОЛНЕНО
//4. Решить проблему того, что с каждым новым пополнением списка тайлов приходится менять список спрайтов - НЕВЫПОЛНЯЕМО
//5. Подумать над тем, чтобы сделать отдельные состояния и для углов поворота - ВЫПОЛНЕНО
//6. Подумать над тем, чтобы сделать отдельные состояния опасный/неопасный для всех видов тайлов - ВЫПОЛНЕНО
//7. Сделать везде адекватную инкапсуляцию - ВЫПОЛНЕНО
//8. Сделать необязательную подписку на события для компонентов, которым это не нужно
//9. Объединить послания мессенджера о следующем ходе в одно послание