using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAngle
{
     Vector3 GetAngleCoord();
     Vector2 TilePos(Vector2 pos, int tileNum);
}
