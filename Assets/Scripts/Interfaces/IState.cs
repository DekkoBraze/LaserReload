using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IState
{
    void Click(Tile tile);
    void DangerTilesNumberUpdate(Tile tile);
    void DangerTilesSpawn(Tile tile);
    void NextMove(Tile tile);
    void ChangeOnDanger(Tile tile);
    void ChangeOnSafe(Tile tile);
    void CheckMovableTurretMove(Tile tile);
    Sprite GetSprite();
}