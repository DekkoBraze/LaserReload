using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour
{
    public static int energy;
    private bool _isItOver;
    private BoxCollider _collider;
    [SerializeField] private Tile players_tile;
    [SerializeField] private Manager _manager;

    void Start()
    {
        _collider = GetComponent<BoxCollider>();
        _manager = FindObjectOfType<Manager>();
        energy = 0;
        _manager.EnergyUpdate();
        _isItOver = false;
        FirstTileSearch();
    }
    public void Move(Tile tile)
    {
        Vector2 tile_pos = tile.gameObject.transform.position;
        string tile_tag = tile.gameObject.tag;
        if (!_isItOver && (tile_pos.y == this.gameObject.transform.position.y && Mathf.Abs(tile_pos.x - this.gameObject.transform.position.x) == 1 ||
            tile_pos.x == this.gameObject.transform.position.x && Mathf.Abs(tile_pos.y - this.gameObject.transform.position.y) == 1))
        {
            this.gameObject.transform.position = tile_pos;
            if (tile_tag == "Finish")
            {
                _manager.CompleteTextAppear();
                _isItOver = true;
            }
            if (tile_tag == "Danger")
            {
                _manager.OnPlayerDestroy();
                Destroy(this.gameObject);
            }
            if (energy < 4)
            {
                energy++;
                _manager.EnergyUpdate();
            }
            Destroy(players_tile.gameObject);
            players_tile = tile;
        }
    }

    public bool EnemyHitCheck(Vector2 enemy_pos)
    {
        if (enemy_pos.y == this.gameObject.transform.position.y && Mathf.Abs(enemy_pos.x - this.gameObject.transform.position.x) <= energy)    
        {
            energy = (int)(energy - Mathf.Abs(enemy_pos.x - this.gameObject.transform.position.x));
            _manager.EnergyUpdate();
            return true;
        }
        else if (enemy_pos.x == this.gameObject.transform.position.x && Mathf.Abs(enemy_pos.y - this.gameObject.transform.position.y) <= energy)
        {
            energy = (int)(energy - Mathf.Abs(enemy_pos.y - this.gameObject.transform.position.y));
            _manager.EnergyUpdate();
            return true;
        }
        else
        {
            return false;
        }
    }

    public void FirstTileSearch()
    {
        Vector2 point = new Vector2(this.gameObject.transform.position.x, this.gameObject.transform.position.y);
        RaycastHit2D hit = Physics2D.Raycast(point, point);
        if (hit.collider != null)
        {
            players_tile = hit.transform.gameObject.GetComponent<Tile>();
        }
    }
}
