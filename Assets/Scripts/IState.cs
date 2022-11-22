using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public interface IState
{
    void SetListeners();
    void Click(Tile clickedTile);
    void SpriteUpdate();
    void SetDangerTilesNumber();
    void DangerTilesSpawn();
    void NextStep();
    void DestroyListeners();
    void DestroyDangerTile();
    void CheckMovableTurretMove();
}