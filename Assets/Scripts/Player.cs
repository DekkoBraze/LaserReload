using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour
{
    // _playersTile - тайл, который в данный момент находится под игроком
    [SerializeField] public Tile playersTile;
    [SerializeField] private Manager _manager;


    public static int energy;
    // _isItOver нужно для того, чтобы игрок не мог двигаться при входе в портал
    

    void Start()
    {
        _manager = FindObjectOfType<Manager>();
        energy = 0;
        Manager.link.EnergyUpdate();
        FirstTileSearch();
    }

    public bool MoveCheck(Vector2 tilePos)
    {
        if (!Manager.link.isItOver && (tilePos.y == this.gameObject.transform.position.y && Mathf.Abs(tilePos.x - this.gameObject.transform.position.x) == 1 ||
            tilePos.x == this.gameObject.transform.position.x && Mathf.Abs(tilePos.y - this.gameObject.transform.position.y) == 1))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    
    public void PlayerChangePosition(Vector2 tilePos)
    {
        this.gameObject.transform.position = tilePos;
    }

    public void PlayerDestroy()
    {
        Destroy(this.gameObject);
    }

    public void PlayerTileChange(Tile ClickedTile)
    {
        Destroy(playersTile.gameObject);
        playersTile = ClickedTile;
    }

    public bool EnemyHitCheck(Vector2 enemyPos)
    {
        // проверка, может ли игрок поразить цель (по x и по y)
        if (enemyPos.y == this.gameObject.transform.position.y && Mathf.Abs(enemyPos.x - this.gameObject.transform.position.x) <= energy)    
        {
            energy = (int)(energy - Mathf.Abs(enemyPos.x - this.gameObject.transform.position.x));
            Manager.link.EnergyUpdate();
            return true;
        }
        else if (enemyPos.x == this.gameObject.transform.position.x && Mathf.Abs(enemyPos.y - this.gameObject.transform.position.y) <= energy)
        {
            energy = (int)(energy - Mathf.Abs(enemyPos.y - this.gameObject.transform.position.y));
            Manager.link.EnergyUpdate();
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
            playersTile = hit.transform.gameObject.GetComponent<Tile>();
        }
    }
}
