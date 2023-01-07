using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Angle90 : IAngle
{
    public Vector3 angleCoord = new Vector3(0, 0, 90);
    public Vector3 GetAngleCoord()
    {
        return angleCoord;
    }
    public Vector2 TilePos(Vector2 pos, int tileNum)
    {
        return new Vector2(pos.x, pos.y - tileNum);
    }
}
