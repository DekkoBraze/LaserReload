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
        if (transform.eulerAngles == Manager.angle0.angleCoord)
        {
            angle = Manager.angle0;
            state.ChangeAngle(angle);
        }
        else if (transform.eulerAngles == Manager.angle90.angleCoord)
        {
            angle = Manager.angle90;
            state.ChangeAngle(angle);
        }
        else if (transform.eulerAngles == Manager.angle180.angleCoord)
        {
            angle = Manager.angle180;
            state.ChangeAngle(angle);
        }
        else
        {
            angle = Manager.angle270;
            state.ChangeAngle(angle);
        }
        state.StateStart();
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
// 1. Разобраться, как сделать в палитре удаление любого объекта при выборе ластика (без постоянного переключения по вкладкам)
// 2. Сделать новый менеджер, который будет отвечать только за спрайты (чтобы не загружать основу)
// 3. Переделать спрайты для турелей так, чтобы сзади них были голубые линии