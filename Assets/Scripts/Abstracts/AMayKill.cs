using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AMayKill : MonoBehaviour
{
    // ���� ������������ Danger ����� ���� ������
    public Tile[] dangerTiles;
    public Tile[] oldDangerTiles;

    // ����������� ����������� Danger ������
    private int _trueDangerTilesNumber;

    private bool unclickable = false;

    public int dangerTilesNumber
    {
        get { return _trueDangerTilesNumber; }
        set
        {
            _trueDangerTilesNumber = value;
            if (value != 0)
            {
                dangerTiles = new Tile[value];
                oldDangerTiles = new Tile[value];
            }
        }
    }

    public virtual void Click(Tile tile)
    {
        if (!unclickable && Manager.playerLink.EnemyHitCheck(tile.gameObject.transform.position))
        {
            Messenger.Broadcast(GameEvent.PLAYER_SHOOT_SOUND);
            StartCoroutine(EnemyDestroy(tile));
        }
    }

    private IEnumerator EnemyDestroy(Tile tile)
    {
        tile.state.StateDestroy();
        Manager.playerLink.StartPrivateCoroutine();
        unclickable = true;

        yield return new WaitForSeconds(0.4f);

        int dangersNum = dangerTilesNumber;
        Tile[] dangers = dangerTiles;
        // ����� ���� ��� ����������� ������
        Manager.stepCount++;
        tile.state.StateDestroy();
        // ����������� Danger ������ �����
        for (int i = 0; i < dangersNum; i++)
        {
            if (dangers[i] != null)
            {
                Tile dangerTile = dangers[i].gameObject.GetComponent<Tile>();
                dangerTile.state.ChangeOnSafe(dangerTile);
                dangers[i] = null;
            }
        }
        dangerTilesNumber = 0;
        // ��������� ���� ����� �� Empty
        tile.gameObject.AddComponent<EmptyState>();
        Destroy(tile.gameObject.GetComponent<TurretState>());
        Destroy(tile.gameObject.GetComponent<Animator>());
        tile.state = GetComponent<EmptyState>();
        tile.state.ChangeOnSafe(tile);
        tile.gameObject.transform.eulerAngles = Manager.angle0.angleCoord;
        Messenger.Broadcast(GameEvent.NEXT_STEP);
        Messenger.Broadcast(GameEvent.DANGER_SPAWN);
    }

    public void DangerTilesSpawn(Tile tile)
    {
        int dangersNum = dangerTilesNumber;
        // _isEnemyHere ����� ��� ����, ����� ����� ����������� Danger "����"
        bool _isEnemyHere = false;
        for (int tileNum = 1; tileNum <= dangersNum; tileNum++)
        {
            Vector2 pos = tile.angle.TilePos(tile.gameObject.transform.position, tileNum);
            DangerTilePlace(pos, tileNum, out _isEnemyHere);
            if (_isEnemyHere)
            {
                break;
            }
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
            hitTile.state.ChangeOnDanger(hitTile, this.gameObject.GetComponent<Tile>());
            dangerTiles[i - 1] = hitTile;
            // �������� ����, ����� �� ����� �� ���������� ����� � gameOver � ������ true
            Vector2 player_pos = Manager.playerLink.transform.position;
            if (pos == player_pos)
            {
                Messenger.Broadcast(GameEvent.ENEMY_SHOOT_SOUND);
                hitTile.state.EnemyLordLink().GetComponent<Tile>().state.FireAnim();
                Manager.link.OnPlayerDestroy();
                Manager.playerLink.PlayerDestroy();
            }
        }
    }

    public void ChangeOnDanger(Tile tile, Tile enemy) { }
    public void ChangeOnSafe(Tile tile) { }

    public Tile EnemyLordLink() 
    {
        return null;
    }
    public void AfterPlayerDestroy()
    {
        foreach (Tile tile in dangerTiles)
        {
            if (tile)
            {
                tile.state.AfterPlayerDestroy();
            }
        }
    }
}
