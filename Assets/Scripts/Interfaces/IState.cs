using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IState
{
    void StateStart();
    void Click(Tile tile);
    void DangerTilesNumberUpdate(Tile tile);
    void DangerTilesSpawn(Tile tile);
    void NextMove(Tile tile);
    void ChangeOnDanger(Tile tile, Tile enemy);
    void ChangeOnSafe(Tile tile);
    void CheckMovableTurretMove(Tile tile);
    void ChangeAngle(IAngle angle);
    Sprite GetSprite();
    void FireAnim();
    Tile EnemyLordLink();
    void StateDestroy();
    void AfterPlayerDestroy();
}