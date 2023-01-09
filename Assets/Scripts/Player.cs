using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour
{
    // _playersTile - тайл, который в данный момент находится под игроком
    [SerializeField] public Tile playersTile;

    private Animator _anim;

    public int energy { get; set; }

    void Start()
    {
        _anim = GetComponent<Animator>();
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
        playersTile.gameObject.AddComponent<LavaState>();
        Destroy(playersTile.gameObject.GetComponent<EmptyState>());
        playersTile.state = playersTile.gameObject.GetComponent<LavaState>();
        playersTile.SetSprite(playersTile.state.GetSprite());
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

    public void ChangePlayerAnim()
    {
        _anim.SetInteger("Energy", energy);
    }

    public void StartPrivateCoroutine()
    {
        StartCoroutine(StartAttackAnim());
    }

    public IEnumerator StartAttackAnim()
    {
        _anim.SetBool("Attack", true);

        yield return new WaitForSeconds(0.05f);

        _anim.SetBool("Attack", false);
    }
}
