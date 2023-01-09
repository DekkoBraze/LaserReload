using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretState : AMayKill, IState
{
    public Sprite tileSprite;

    private Animator _anim;
    // Нужно для запуска анимаций
    int angleNum = 0;

    public bool isInfinite;

    private void Awake()
    {
        _anim = gameObject.GetComponent<Animator>();
    }

    public void StateStart() 
    {
        SpawnBackground();
        StartCoroutine(StartBlinkAnim());
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
                angleNum = 90;
                _anim.SetInteger("AngleNum", angleNum);
                break;
            case 180:
                tileSprite = Manager.link.turretTiles[2];
                angleNum = 180;
                _anim.SetInteger("AngleNum", angleNum);
                break;
            case 270:
                tileSprite = Manager.link.turretTiles[3];
                angleNum = 270;
                _anim.SetInteger("AngleNum", angleNum);
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

    private IEnumerator StartBlinkAnim()
    {
        while (true)
        {
            int blinkTime = Random.Range(5, 30);

            yield return new WaitForSeconds(blinkTime);

            string animName = "TurretTileBlink" + angleNum;
            _anim.Play(animName);
        }
    }
}