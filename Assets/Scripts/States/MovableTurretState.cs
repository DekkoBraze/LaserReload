using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableTurretState : AMayKill, IState
{
    public Sprite tileSprite;

    public bool isInfinite;

    public Vector2[] teleportTiles;

    public void SpriteUpdate(Tile tile)
    {
        tile.SetSprite(tileSprite);
    }
    public void DangerTilesNumberUpdate(Tile tile)
    {
        dangerTilesNumber = 2;
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
                    Debug.Log("You were slashed by laser!");
                    Manager.playerLink.PlayerDestroy();
                    Manager.link.OnPlayerDestroy();
                    break;
                }
            }
        }
    }
    public Sprite GetSprite()
    {
        return tileSprite;
    }
}
