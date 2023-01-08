using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretState : AMayKill, IState
{
    public Sprite tileSprite;

    // Нужно для запуска анимаций
    int angleNum = 0;
    int blinkTime = 0;

    public bool isInfinite;

    public void StateStart() 
    {
        SpawnBackground();
        blinkTime = RandomBlinkTimeCalculate();
    }

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
                angleNum = 0;
                break;
            case 90:
                tileSprite = Manager.link.turretTiles[1];
                angleNum = 1;
                break;
            case 180:
                tileSprite = Manager.link.turretTiles[2];
                angleNum = 2;
                break;
            case 270:
                tileSprite = Manager.link.turretTiles[3];
                angleNum = 3;
                break;
        }
        this.gameObject.GetComponent<Tile>().SetSprite(tileSprite);
    }

    private void SpawnBackground()
    {
        GameObject background = Instantiate(Manager.link.backgroundEmptyTile);
        background.transform.position = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, 11);
        background.transform.parent = this.gameObject.transform;
    }

    private int RandomBlinkTimeCalculate()
    {
        return Random.Range(1, 10);
    }
}