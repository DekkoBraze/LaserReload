using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Angle180 : IAngle
{
    public Vector2 TilePos(Vector2 pos, int tileNum)
    {
        return new Vector2(pos.x + tileNum, pos.y);
    }
}
