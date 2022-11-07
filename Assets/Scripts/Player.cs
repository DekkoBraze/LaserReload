using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour
{
    // _playersTile - ����, ������� � ������ ������ ��������� ��� �������
    [SerializeField] public Tile playersTile;
    [SerializeField] private Manager _manager;
    private GameObject _tilesFolder;

    public static int energy;
    // _isItOver ����� ��� ����, ����� ����� �� ��� ��������� ��� ����� � ������
    private bool _isItOver;
    

    void Start()
    {
        _manager = FindObjectOfType<Manager>();
        _tilesFolder = GameObject.FindGameObjectWithTag("Folder");
        _isItOver = false;
        energy = 0;
        _manager.EnergyUpdate();
        FirstTileSearch();
    }
    public void NonEnemyClick(Tile ClickedTile)
    {
        Vector2 tilePos = ClickedTile.gameObject.transform.position;
        // � if �������������, ����� �� ����� �������� �� ����
        if (!_isItOver && (tilePos.y == this.gameObject.transform.position.y && Mathf.Abs(tilePos.x - this.gameObject.transform.position.x) == 1 ||
            tilePos.x == this.gameObject.transform.position.x && Mathf.Abs(tilePos.y - this.gameObject.transform.position.y) == 1))
        {
            this.gameObject.transform.position = tilePos;
            // ����� ���� ��� ����������� ������
            Manager.stepCount++;
            Messenger.Broadcast(GameEvent.NEXT_STEP);
            if (ClickedTile.type == Tile.TileType.Portal)
            {
                _manager.CompleteTextAppear();
                _isItOver = true;
            }
            if (ClickedTile.type == Tile.TileType.Danger || ClickedTile.type == Tile.TileType.DangerPortal)
            {
                _manager.OnPlayerDestroy();
                Destroy(this.gameObject);
            }
            if (energy < 4)
            {
                energy++;
                _manager.EnergyUpdate();
            }
            _tilesFolder.BroadcastMessage("CheckMovableTurretMove", ClickedTile);
            // ����������� ����������� ����� ��� ������� � ��������� ������
            Destroy(playersTile.gameObject);
            playersTile = ClickedTile;
        }
    }

    public bool EnemyHitCheck(Vector2 enemyPos)
    {
        // ��������, ����� �� ����� �������� ���� (�� x � �� y)
        if (enemyPos.y == this.gameObject.transform.position.y && Mathf.Abs(enemyPos.x - this.gameObject.transform.position.x) <= energy)    
        {
            energy = (int)(energy - Mathf.Abs(enemyPos.x - this.gameObject.transform.position.x));
            _manager.EnergyUpdate();
            return true;
        }
        else if (enemyPos.x == this.gameObject.transform.position.x && Mathf.Abs(enemyPos.y - this.gameObject.transform.position.y) <= energy)
        {
            energy = (int)(energy - Mathf.Abs(enemyPos.y - this.gameObject.transform.position.y));
            _manager.EnergyUpdate();
            return true;
        }
        else
        {
            return false;
        }
    }

    // ����� ��� ������ ����� ��� ������� �� ����� ������ �����
    public void FirstTileSearch()
    {
        Vector2 point = new Vector2(this.gameObject.transform.position.x, this.gameObject.transform.position.y);
        RaycastHit2D hit = Physics2D.Raycast(point, point);
        if (hit.collider != null)
        {
            playersTile = hit.transform.gameObject.GetComponent<Tile>();
        }
    }
}
