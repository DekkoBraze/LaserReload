using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;

    public IState state;
    public IAngle angle;    

    // ���� ������������ Danger ����� ���� ������
    public Tile[] _dangerTiles;
    public Tile[] _oldDangerTiles;
    public Vector2[] _teleportTiles;
    
    // ����������� ����������� Danger ������
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
        // �������� ������ �� ������ ������� � ������� ��������
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
                // ��� ����� ��������� �������� Danger ����� ������ ��� �������� Danger ������, ������ ������� ����������, ������� �������������
                // �������� �� Danger, ��������� � �����, ������� � ����� ������������ ���� ���������������� ������� � ����� ����������� ����� ������� ������
                canPlaceTile = true;
            }
        }
        if (canPlaceTile)
        {
            Tile hitTile = hits[0].collider.gameObject.GetComponent<Tile>();
            hitTile.state.ChangeOnDanger(hitTile);
            _dangerTiles[i - 1] = hitTile;
            // �������� ����, ����� �� ����� �� ���������� ����� � gameOver � ������ true
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
// 1. ��������� ���������� ������ � ���������� ������ ����, ����� �������� ���� - ���������
// 2. ������� ������, ������� ��������� ���������, ���������� �� ������� ���������� isDanger � ������ ��� ��� � ��������� � �� ����� � ������ - ���������
// 3. ��������� ��� ������ �� ���������� � ������������ � ������� ���� ����������� ������� AMayKill � ACantKill, ������� �������������� ����� ��� ������� ��������
// 4. ��������� ��� ����, ����������� � ���������� ������, �� ����� � ��������������� ������
// 5. ��������� ������� �� ��������� � ��������������� ������� �������� (� ������)