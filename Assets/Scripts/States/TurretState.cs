using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretState : AMayKill, IState
{
    public Sprite tileSprite;

    public bool isInfinite;

    public void StateStart() { }

    public void SpriteUpdate(Tile tile)
    {
        tile.SetSprite(tileSprite);
    }
    public void DangerTilesNumberUpdate(Tile tile)
    {
        if (!isInfinite)
        {
            dangerTilesNumber = 2;
        }
        else
        {
            dangerTilesNumber = 50;
        }
    }
    public void NextMove(Tile tile) { }
    public void CheckMovableTurretMove(Tile tile) { }
    public Sprite GetSprite()
    {
        return tileSprite;
    }
    public void ChangeAngle(IAngle angle)
    {
        int angleInt = (int)angle.GetAngleCoord().z;
        switch (angleInt)
        {
            case 0:
                tileSprite = Manager.link.turretTiles[0];
                break;
            case 90:
                tileSprite = Manager.link.turretTiles[1];
                break;
            case 180:
                tileSprite = Manager.link.turretTiles[2];
                break;
            case 270:
                tileSprite = Manager.link.turretTiles[3];
                break;
        }
        this.gameObject.GetComponent<Tile>().SetSprite(tileSprite);
    }
}