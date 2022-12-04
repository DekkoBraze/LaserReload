using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;

    public IState state;
    public IAngle angle;    

    // сюда складываютс€ Danger тайлы этой турели
    public Tile[] _dangerTiles;
    public Tile[] _oldDangerTiles;
    public Vector2[] _teleportTiles;
    
    // колличество спавн€щихс€ Danger тайлов
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

    private void Awake()
    {
        Messenger.AddListener(GameEvent.NEXT_STEP, NextMoveAwake);
        Messenger.AddListener(GameEvent.DANGER_SPAWN, DangerTileSpawnAwake);
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
            if (obj.collider.gameObject.GetComponent<Tile>().state.GetType().ToString() == "TurretState")
            {
                canPlaceTile = false;
                isEnemyHere = true;
                break;
            }
            else
            {
                // при такой структуре свойство Danger будет второй раз даватьс€ Danger тайлам, однако прошла€ реализаци€, котора€ подразумевала
                // проверку на Danger, приводила к багам, поэтому € решил пожертвовать этой малозначительной деталью в угоду целостности своих нервных клеток
                canPlaceTile = true;
            }
        }
        if (canPlaceTile)
        {
            Tile hitTile = hits[0].collider.gameObject.GetComponent<Tile>();
            hitTile.state.ChangeOnDanger(hitTile);
            _dangerTiles[i - 1] = hitTile;
            // проверка того, стоит ли игрок на измен€емом тайле и gameOver в случае true
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

    public void SetState()
    {
        state = GetComponent<IState>();
        SetSprite(state.GetSpriteNum());
    }

    private void NextMoveAwake()
    {
        state.NextMove(this);
    }

    private void DangerTileSpawnAwake()
    {
        state.DangerTilesSpawn(this);
    }

    public void AwakeCheckMovableTurretMove()
    {
        state.CheckMovableTurretMove(this);
    }

    private void OnDestroy()
    {
        Messenger.RemoveListener(GameEvent.NEXT_STEP, NextMoveAwake);
        Messenger.RemoveListener(GameEvent.DANGER_SPAWN, DangerTileSpawnAwake);
        Messenger.RemoveListener(GameEvent.SET_STATE, SetState);
        Messenger.RemoveListener(GameEvent.CHECK_MOVABLE_TURRET, AwakeCheckMovableTurretMove); 
    }
}
// 1. ѕровер€ть нахождение стейта в инспекторе вместо того, чтобы смотреть енум - ¬џѕќЋЌ≈Ќќ
// 2. —делать методы, которые позвол€ют проверить, существует ли булева€ переменна€ isDanger в стейте или нет и перенести еЄ из тайла в стейты - ¬џѕќЋЌ≈Ќќ
// 3. –азделить все стейты на стрел€ющие и нестрел€ющие с помощью двух абстрактных классов AMayKill и ACantKill, которые предопредел€ют общие дл€ стейтов действи€
// 4. ѕеренести все пол€, относ€щиес€ к стрел€ющим тайлам, из тайла в соответствующие стейты
// 5. ѕеренести спрайты из менеджера в соответствующие префабы объектов (в стейты)