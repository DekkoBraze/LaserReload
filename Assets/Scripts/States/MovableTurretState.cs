using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableTurretState : AMayKill, IState
{
    public Sprite tileSprite;

    public bool isInfinite;

    private Animator _anim;

    public Vector2[] teleportTiles;

    public Vector2 firstTeleportTile;
    public Vector2 lastTeleportTile;

    private void Awake()
    {
        _anim = gameObject.GetComponent<Animator>();
    }

    public void StateStart()
    {
        CalculateMovablePath();
        _anim.SetBool("isInfinite", isInfinite);
        StartCoroutine(StartBlinkAnim());
    }

    public void SpriteUpdate(Tile tile)
    {
        tile.SetSprite(tileSprite);
    }
    public void DangerTilesNumberUpdate(Tile tile)
    {
        if (!isInfinite)
        {
            dangerTilesNumber = 2;
        }
        else
        {
            dangerTilesNumber = 50;
        }
    }

    public override void Click(Tile tile)
    {
        if (Manager.playerLink.EnemyHitCheck(tile.gameObject.transform.position))
        {
            Debug.Log("You can't destroy this!");
            int dangersNum = dangerTilesNumber;
            Tile[] dangers = dangerTiles;
            // смена хода для двигающихся тайлов
            Manager.stepCount++;
            // уничтожение Danger тайлов врага
            for (int i = 0; i < dangersNum; i++)
            {
                if (dangers[i] != null)
                {
                    Tile dangerTile = dangers[i].gameObject.GetComponent<Tile>();
                    dangerTile.state.ChangeOnSafe(dangerTile);
                    dangers[i] = null;
                }
            }
            Messenger.Broadcast(GameEvent.NEXT_STEP);
            Messenger.Broadcast(GameEvent.DANGER_SPAWN);
        }
    }

    public void NextMove(Tile tile) 
    {
        if (teleportTiles.Length > 0)
        {
            if (teleportTiles[Manager.stepCount % teleportTiles.Length] != null)
            {
                tile.gameObject.transform.position = teleportTiles[Manager.stepCount % teleportTiles.Length];
            }
            else
            {
                tile.gameObject.transform.position = teleportTiles[0];
            }
            for (int num = 0; num < dangerTiles.Length; num++)
            {
                if (dangerTiles[num] != null)
                {
                    oldDangerTiles[num] = dangerTiles[num];
                    Tile dangerTile = dangerTiles[num].gameObject.GetComponent<Tile>();
                    dangerTile.state.ChangeOnSafe(dangerTile);
                    dangerTiles[num] = null;
                }
            }
        }
    }

    public void CheckMovableTurretMove(Tile tile)
    {
        foreach (Tile oldPlayersPosition in dangerTiles)
        {
            foreach (Tile newPlayersPosition in oldDangerTiles)
            {
                if (oldPlayersPosition == Manager.playerLink.playersTile && newPlayersPosition == Manager.link.clickedTile)
                {
                    FireAnim();
                    Manager.playerLink.PlayerDestroy();
                    Manager.link.OnPlayerDestroy();
                    Manager.link.isItOver = true;
                    return;
                }
            }
        }
    }
    public Sprite GetSprite()
    {
        return tileSprite;
    }

    public void CalculateMovablePath()
    {
        int teleportTilesNum = 0;
        if (firstTeleportTile.x == lastTeleportTile.x)
        {
            teleportTilesNum = (int)Mathf.Abs(firstTeleportTile.y - lastTeleportTile.y) + 1;
            teleportTiles = new Vector2[teleportTilesNum];
            if (firstTeleportTile.y < lastTeleportTile.y)
            {
                for (int i = 0; i < teleportTilesNum; i++)
                {
                    Vector2 currentVector = new Vector2(firstTeleportTile.x, firstTeleportTile.y + i);
                    teleportTiles[i] = currentVector;
                }
            }
            else
            {
                for (int i = 0; i < teleportTilesNum; i++)
                {
                    Vector2 currentVector = new Vector2(firstTeleportTile.x, firstTeleportTile.y - i);
                    teleportTiles[i] = currentVector;
                }
            }
        }
        else
        {
            teleportTilesNum = (int)Mathf.Abs(firstTeleportTile.x - lastTeleportTile.x) + 1;
            teleportTiles = new Vector2[teleportTilesNum];
            if (firstTeleportTile.x < lastTeleportTile.x)
            {
                for (int i = 0; i < teleportTilesNum; i++)
                {
                    Vector2 currentVector = new Vector2(firstTeleportTile.x + i, firstTeleportTile.y);
                    teleportTiles[i] = currentVector;
                }
            }
            else
            {
                for (int i = 0; i < teleportTilesNum; i++)
                {
                    Vector2 currentVector = new Vector2(firstTeleportTile.x - i, firstTeleportTile.y);
                    teleportTiles[i] = currentVector;
                }
            }
        }
    }

    public void ChangeAngle(IAngle angle)
    {

    }
    public void StateDestroy() { }

    private IEnumerator StartBlinkAnim()
    {
        while (true)
        {
            int blinkTime = Random.Range(5, 30);

            yield return new WaitForSeconds(blinkTime);

            string animName;
            if (isInfinite)
            {
                animName = "InfiniteMovableBlink";
            }
            else
            {
                animName = "MovableBlink";
            }
            _anim.Play(animName);
        }
    }

    public void FireAnim() 
    {
        string animName;
        if (isInfinite)
        {
            animName = "InfiniteMovableFire";
        }
        else
        {
            animName = "MovableFire";
        }
        _anim.Play(animName);
    }
}
