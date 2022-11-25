using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonDanger : IDanger
{
    public void ChangeDangerState(Tile tile, int spriteNum)
    {
        tile.SetDangerSprite(spriteNum);
    }

    public void DestroyPlayerOrNot() { }
}
