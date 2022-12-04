using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;

    public IState state;
    public IAngle angle;

    private void Awake()
    {
        Messenger.AddListener(GameEvent.NEXT_STEP, NextMoveAwake);
        Messenger.AddListener(GameEvent.DANGER_SPAWN, DangerSpawnAwake);
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

    public void SetSprite(Sprite sprite)
    {
        _spriteRenderer.sprite = sprite;
    }

    public void SetDangerSprite(Sprite sprite)
    {
        _spriteRenderer.sprite = sprite;
    }

    public void SetState()
    {
        state = GetComponent<IState>();
        SetSprite(state.GetSprite());
    }

    private void NextMoveAwake()
    {
        state.NextMove(this);
    }

    private void DangerSpawnAwake()
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
        Messenger.RemoveListener(GameEvent.DANGER_SPAWN, DangerSpawnAwake);
        Messenger.RemoveListener(GameEvent.SET_STATE, SetState);
        Messenger.RemoveListener(GameEvent.CHECK_MOVABLE_TURRET, AwakeCheckMovableTurretMove); 
    }
}
// 1. Проверять нахождение стейта в инспекторе вместо того, чтобы смотреть енум - ВЫПОЛНЕНО
// 2. Сделать методы, которые позволяют проверить, существует ли булевая переменная isDanger в стейте или нет и перенести её из тайла в стейты - ВЫПОЛНЕНО
// 3. Разделить все стейты на стреляющие и нестреляющие с помощью двух абстрактных классов AMayKill и ACantKill,
// которые предопределяют общие для стейтов действия - ВЫПОЛНЕНО
// 4. Перенести все поля, относящиеся к стреляющим тайлам, из тайла в соответствующие стейты - ВЫПОЛНЕНО
// 5. Перенести спрайты из менеджера в соответствующие префабы объектов (в стейты) - ВЫПОЛНЕНО
// 6. Поправить имена переменных - ВЫПОЛНЕНО
// 7. Разобраться с багом, при котором пропадают спрайты (связано с новой системой спрайтов) - ВЫПОЛНЕНО
// 8. Придумать, как убрать из менеджера спрайты окончательно