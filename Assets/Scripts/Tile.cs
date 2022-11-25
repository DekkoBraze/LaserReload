using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;

    public IState state;
    public IAngle angle;
    public IDanger dangerState;

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

    // ��������� ����� ��� ������ Danger
    public readonly Vector3 angle0 = new Vector3(0, 0, 0);
    public readonly Vector3 angle90 = new Vector3(0, 0, 90);
    public readonly Vector3 angle180 = new Vector3(0, 0, 180);
    public readonly Vector3 angle270 = new Vector3(0, 0, 270);

    // ������������ ����� ����� (��-��������� Empty)
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
    public TileType type;

    private void Awake()
    {
        Messenger.AddListener(GameEvent.DANGER_TILES_UPDATE, DangerTilesSpawnAwake);
        Messenger.AddListener(GameEvent.NEXT_STEP, NextMoveAwake);
        Messenger.AddListener(GameEvent.SET_STATE, SetState);
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        state.DangerTilesNumberUpdate(this);
        state.SpriteUpdate(this);
        if (transform.eulerAngles == new Vector3(0, 0, 0))
        {
            angle = Manager.link.angle0;
        }
        else if (transform.eulerAngles == new Vector3(0, 0, 90))
        {
            angle = Manager.link.angle90;
        }
        else if (transform.eulerAngles == new Vector3(0, 0, 180))
        {

            angle = Manager.link.angle180;
        }
        else
        {

            angle = Manager.link.angle270;
        }
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
            if (obj.collider.gameObject.GetComponent<Tile>().state == Manager.link.turretState)
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
            hits[0].collider.gameObject.GetComponent<Tile>().state.ChangeStateOnDanger(hits[0].collider.gameObject.GetComponent<Tile>());
            _dangerTiles[i - 1] = hits[0].collider.gameObject.GetComponent<Tile>();
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

    private void SetState()
    {
        state = Manager.link.states[(int)type];
    }

    private void DangerTilesSpawnAwake()
    {
        state.DangerTilesSpawn(this);
    }

    private void NextMoveAwake()
    {
        state.NextMove(this);
    }

    public void CheckMovableTurretMove(Tile clickedTile)
    {
        foreach (Tile oldPlayersPosition in _dangerTiles)
        {
            foreach (Tile newPlayersPosition in _oldDangerTiles)
            {
                if (oldPlayersPosition == Manager.playerLink.playersTile && newPlayersPosition == clickedTile)
                {
                    Debug.Log("You were slashed by laser!");
                    Manager.playerLink.PlayerDestroy();
                    Manager.link.OnPlayerDestroy();
                }
            }
        }
    }

    private void OnDestroy()
    {
        Messenger.RemoveListener(GameEvent.DANGER_TILES_UPDATE, DangerTilesSpawnAwake);
        Messenger.RemoveListener(GameEvent.NEXT_STEP, NextMoveAwake);
        Messenger.RemoveListener(GameEvent.SET_STATE, SetState);
    }
}

//�����������:
//1. ������� ����� enum �� ��������� ������ � ������� �������� State - ���������
//2. ����������� NonEnemyClick � ��������������� ������ - ���������
//3. ���������� �������� ��� ����� ������ �������� ���� �� ����� - ���������
//4. ������ �������� ����, ��� � ������ ����� ����������� ������ ������ ���������� ������ ������ �������� - ������������
//5. �������� ��� ���, ����� ������� ��������� ��������� � ��� ����� �������� - ���������
//6. �������� ��� ���, ����� ������� ��������� ��������� �������/��������� ��� ���� ����� ������
//7. ������� ����� ���������� ������������
//8. ������� �������������� �������� �� ������� ��� �����������, ������� ��� �� �����