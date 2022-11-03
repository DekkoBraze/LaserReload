using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour
{
    // _playersTile - тайл, который в данный момент находится под игроком
    [SerializeField] private Tile _playersTile;
    [SerializeField] private Manager _manager;

    public static int energy;
    // _isItOver нужно для того, чтобы игрок не мог двигаться при входе в портал
    private bool _isItOver;
    

    void Start()
    {
        _manager = FindObjectOfType<Manager>();
        _isItOver = false;
        energy = 0;
        _manager.EnergyUpdate();
        FirstTileSearch();
    }
    public void NonEnemyClick(Tile tile)
    {
        Vector2 tilePos = tile.gameObject.transform.position;
        // в if высчитывается, может ли игрок походить на тайл
        if (!_isItOver && (tilePos.y == this.gameObject.transform.position.y && Mathf.Abs(tilePos.x - this.gameObject.transform.position.x) == 1 ||
            tilePos.x == this.gameObject.transform.position.x && Mathf.Abs(tilePos.y - this.gameObject.transform.position.y) == 1))
        {
            this.gameObject.transform.position = tilePos;
            if (tile.type == Tile.TileType.Portal)
            {
                _manager.CompleteTextAppear();
                _isItOver = true;
            }
            if (tile.type == Tile.TileType.Danger || tile.type == Tile.TileType.DangerPortal)
            {
                _manager.OnPlayerDestroy();
                Destroy(this.gameObject);
            }
            if (energy < 4)
            {
                energy++;
                _manager.EnergyUpdate();
            }
            // уничтожение предыдущего тайла под игроком и установка нового
            Destroy(_playersTile.gameObject);
            _playersTile = tile;
        }
    }

    public bool EnemyHitCheck(Vector2 enemyPos)
    {
        // проверка, может ли игрок поразить цель (по x и по y)
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

    // метод для поиска тайла под игроком во время старта сцены
    public void FirstTileSearch()
    {
        Vector2 point = new Vector2(this.gameObject.transform.position.x, this.gameObject.transform.position.y);
        RaycastHit2D hit = Physics2D.Raycast(point, point);
        if (hit.collider != null)
        {
            _playersTile = hit.transform.gameObject.GetComponent<Tile>();
        }
    }
}
