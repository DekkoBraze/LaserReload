using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IState
{
    void Click(Tile tile);
    void SpriteUpdate(Tile tile);
    void DangerTilesNumberUpdate(Tile tile);
    void DangerTilesSpawn(Tile tile);
    void NextMove(Tile tile);
    void ChangeStateOnDanger(Tile tile);
    void ChangeStateOnSafe(Tile tile);
}