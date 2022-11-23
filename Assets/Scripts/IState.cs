using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public interface IState
{
    void SetTile(Tile linkedTile);
    void SetListeners();
    void Click();
    void SpriteUpdate();
    void DangerTilesNumberUpdate();
    void DangerTilesSpawn();
    void NextMove();
    void DestroyListeners();
    void CheckMovableTurretMove(Tile clickedTile);
}