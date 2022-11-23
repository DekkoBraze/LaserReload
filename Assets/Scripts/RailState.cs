using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RailState : MonoBehaviour, IState
{
    Tile tile;
    public void SetTile(Tile linkedTile)
    {
        tile = linkedTile;
    }
    public void Click() { }
    public void SpriteUpdate()
    {
        tile.SetSprite(6);
    }
    public void DangerTilesNumberUpdate()
    {
        tile._dangerTilesNumber = 0;
    }
    public void SetListeners() { }
    public void DangerTilesSpawn() { }
    public void NextMove() { }
    public void DestroyListeners() { }
    public void CheckMovableTurretMove(Tile clickedTile) { }
}

