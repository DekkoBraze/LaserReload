using System.Collections;
using System.Collections.Generic;
using UnityEngine;

<<<<<<< HEAD
<<<<<<< HEAD
public class MovableTurretState : IState
{
    private int spriteNum = 3;
=======
public class MovableTurretState : MonoBehaviour, IState
{
    private int spriteNum = 3;
    public bool isInfinite;

>>>>>>> parent of 9828e7c (New states habe been completed.)
=======
public class MovableTurretState : MonoBehaviour, IState
{
    private int spriteNum = 3;
    public bool isInfinite;

>>>>>>> parent of 9828e7c (New states habe been completed.)
    public void Click(Tile tile)
    {
        if (Manager.playerLink.EnemyHitCheck(tile.gameObject.transform.position))
        {
            int dangersNum = tile._dangerTilesNumber;
            Tile[] dangers = tile._dangerTiles;
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
            tile._dangerTilesNumber = 0;
            // изменение типа врага на Empty
<<<<<<< HEAD
<<<<<<< HEAD
            tile.state = Manager.emptyState;
=======
            tile.gameObject.AddComponent<EmptyState>();
            Destroy(tile.gameObject.GetComponent<TurretState>());
            tile.state = GetComponent<EmptyState>();
>>>>>>> parent of 9828e7c (New states habe been completed.)
=======
            tile.gameObject.AddComponent<EmptyState>();
            Destroy(tile.gameObject.GetComponent<TurretState>());
            tile.state = GetComponent<EmptyState>();
>>>>>>> parent of 9828e7c (New states habe been completed.)
            tile.SetSprite(0);
            Messenger.Broadcast(GameEvent.NEXT_STEP);
        }
    }
    public void SpriteUpdate(Tile tile)
    {
        tile.SetSprite(spriteNum);
    }
    public void DangerTilesNumberUpdate(Tile tile)
    {
<<<<<<< HEAD
<<<<<<< HEAD
<<<<<<< Updated upstream
=======
>>>>>>> parent of 9828e7c (New states habe been completed.)
=======
>>>>>>> parent of 9828e7c (New states habe been completed.)
        tile._dangerTilesNumber = 2;
    }
    public void DangerTilesSpawn(Tile tile)
    {
        int dangersNum = tile._dangerTilesNumber;
        // _isEnemyHere нужно для того, чтобы враги перекрывали Danger "лучи"
        bool _isEnemyHere = false;
        for (int tileNum = 1; tileNum <= dangersNum; tileNum++)
        {
            Vector2 pos = tile.angle.TilePos(tile.gameObject.transform.position, tileNum);
            tile.DangerTilePlace(pos, tileNum, out _isEnemyHere);
            if (_isEnemyHere)
            {
                break;
            }
<<<<<<< HEAD
<<<<<<< HEAD
=======
        if (isInfinite)
        {
            dangerTilesNumber = 50;
        }
        else
        {
            dangerTilesNumber = 2;
>>>>>>> Stashed changes
=======
>>>>>>> parent of 9828e7c (New states habe been completed.)
=======
>>>>>>> parent of 9828e7c (New states habe been completed.)
        }
    }
    public void NextMove(Tile tile) 
    {
        Vector2[] teleports = tile._teleportTiles;
        Tile[] dangers = tile._dangerTiles;

        if (teleports.Length > 0)
        {
            if (teleports[Manager.stepCount % teleports.Length] != null)
            {
                tile.gameObject.transform.position = teleports[Manager.stepCount % teleports.Length];
            }
            else
            {
                tile.gameObject.transform.position = teleports[0];
            }
            for (int num = 0; num < dangers.Length; num++)
            {
                if (dangers[num] != null)
                {
                    tile._oldDangerTiles[num] = dangers[num];
                    Tile dangerTile = dangers[num].gameObject.GetComponent<Tile>();
                    dangerTile.state.ChangeOnSafe(dangerTile);
                    dangers[num] = null;
                }
            }
        }
    }
    public void ChangeOnDanger(Tile tile) { }
    public void ChangeOnSafe(Tile tile) { }
    public void CheckMovableTurretMove(Tile tile)
    {
        foreach (Tile oldPlayersPosition in tile._dangerTiles)
        {
            foreach (Tile newPlayersPosition in tile._oldDangerTiles)
            {
                if (oldPlayersPosition == Manager.playerLink.playersTile && newPlayersPosition == Manager.link.clickedTile)
                {
                    Debug.Log("You were slashed by laser!");
                    Manager.playerLink.PlayerDestroy();
                    Manager.link.OnPlayerDestroy();
                    Manager.link.isItOver = true;
                    return;
                }
            }
        }
    }
    public int GetSpriteNum()
    {
        return spriteNum;
    }
}
