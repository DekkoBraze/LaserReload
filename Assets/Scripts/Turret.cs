using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : Enemy
{
    [SerializeField] private DangerousTile dangerousTile;
    [SerializeField] private Manager _manager;
    private const int dangerousTilesNumber = 2;
    private int dangerousTilesSpawned = 0;
    private DangerousTile[] dangerousTiles;

    private void Awake()
    {
        Messenger.AddListener(GameEvent.DANGEROUS_TILES_UPDATE, DangerousTilesSpawn); 
    }

    protected override void Start()
    {
        base.Start();
        _manager = FindObjectOfType<Manager>();
        dangerousTiles = new DangerousTile[3];
        Manager.turrets.Add(this);
    }

    public void DangerousTilesSpawn()
    {
        bool isEnemyHere = false;
        for (int i = 1; i <= dangerousTilesNumber; i++)
        {
            if (transform.rotation.eulerAngles == new Vector3(0, 0, 0) && !isEnemyHere)
            {
                DangerousTilesCheck(new Vector2(transform.position.x - i, transform.position.y), i, out isEnemyHere);
            }
            else if (transform.rotation.eulerAngles == new Vector3(0, 0, 90) && !isEnemyHere)
            {
                DangerousTilesCheck(new Vector2(transform.position.x, transform.position.y - i), i, out isEnemyHere);
            }
            else if (transform.rotation.eulerAngles == new Vector3(0, 0, 180) && !isEnemyHere)
            {
                DangerousTilesCheck(new Vector2(transform.position.x + i, transform.position.y), i, out isEnemyHere);
            }
            else if (!isEnemyHere)
            {
                DangerousTilesCheck(new Vector2(transform.position.x, transform.position.y + i), i, out isEnemyHere);
            }
        }
    }

    public override void OnMouseDown()
    {
        if (player != null && player.EnemyHitCheck(this.gameObject.transform.position))
        {
            for (int i = 1; i <= dangerousTilesSpawned; i++)
            {
                if (dangerousTiles[i] != null)
                {
                    Destroy(dangerousTiles[i].gameObject);
                }
            }
            this.gameObject.tag = "Untagged";
            Messenger.RemoveListener(GameEvent.DANGEROUS_TILES_UPDATE, DangerousTilesSpawn);
            Messenger.Broadcast(GameEvent.DANGEROUS_TILES_UPDATE);
            Destroy(this.gameObject);
        }
    }

    private void DangerousTilesCheck(Vector2 pos, int i, out bool isEnemyHere)
    {
        bool can_place_tile = false;
        isEnemyHere = false;
        RaycastHit2D[] hits = Physics2D.CircleCastAll(pos, 0.1f, new Vector2(0, 0));
        foreach (RaycastHit2D obj in hits)
        {
            Debug.Log(obj.collider.gameObject.tag);
            if (obj.collider.gameObject.tag == "Enemy")
            {
                can_place_tile = false;
                isEnemyHere = true;
                break;
            }
            else if (obj.collider.gameObject.tag == "Danger")
            {
                can_place_tile = false;
                break;
            }
            else
            {
                can_place_tile = true;
            }
        }
        if (can_place_tile)
        {
            dangerousTiles[i] = Instantiate(dangerousTile);
            dangerousTiles[i].transform.position = pos;
            dangerousTilesSpawned++;
            Vector2 player_pos = player.transform.position;
            if (pos == player_pos)
            {
                _manager.OnPlayerDestroy();
                Destroy(player.gameObject);
            }
        }
    }

    private void OnDestroy()
    {
        Messenger.RemoveListener(GameEvent.DANGEROUS_TILES_UPDATE, DangerousTilesSpawn);
    }
}