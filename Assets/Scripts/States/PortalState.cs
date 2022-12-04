using System.Collections;
using System.Collections.Generic;
using UnityEngine;

<<<<<<< HEAD
<<<<<<< HEAD
public class PortalState : IState
{
    private int spriteNum = 1;
=======
public class PortalState : MonoBehaviour, IState
{
    private int spriteNum = 1;
    private bool isDanger = false;

>>>>>>> parent of 9828e7c (New states habe been completed.)
=======
public class PortalState : MonoBehaviour, IState
{
    private int spriteNum = 1;
    private bool isDanger = false;

>>>>>>> parent of 9828e7c (New states habe been completed.)
    public void Click(Tile tile)
    {
        Vector2 tilePos = tile.transform.position;
        if (Manager.playerLink.MoveCheck(tilePos))
        {
            Manager.link.clickedTile = tile;
            Manager.playerLink.PlayerChangePosition(tilePos);
<<<<<<< HEAD
<<<<<<< HEAD
<<<<<<< Updated upstream
            // смена хода для двигающихся тайлов
            Manager.stepCount++;
            Messenger.Broadcast(GameEvent.NEXT_STEP);
            Manager.link.CompleteTextAppear();
            Manager.link.isItOver = true;
            if (Manager.playerLink.energy < 4)
=======
            base.Click(tile);
            if (!isDanger && !Manager.link.isItOver)
>>>>>>> Stashed changes
            {
                Manager.playerLink.energy++;
                Manager.link.EnergyUpdate();
            }
=======
            // смена хода для двигающихся тайлов
            Manager.stepCount++;
            Messenger.Broadcast(GameEvent.NEXT_STEP);
            if (isDanger)
            {
                Manager.link.OnPlayerDestroy();
                Manager.playerLink.PlayerDestroy();
            }
=======
            // смена хода для двигающихся тайлов
            Manager.stepCount++;
            Messenger.Broadcast(GameEvent.NEXT_STEP);
            if (isDanger)
            {
                Manager.link.OnPlayerDestroy();
                Manager.playerLink.PlayerDestroy();
            }
>>>>>>> parent of 9828e7c (New states habe been completed.)
            Manager.link.CompleteTextAppear();
            Manager.link.isItOver = true;
            if (Manager.playerLink.energy < 4)
            {
                Manager.playerLink.energy++;
                Manager.link.EnergyUpdate();
            }
<<<<<<< HEAD
>>>>>>> parent of 9828e7c (New states habe been completed.)
=======
>>>>>>> parent of 9828e7c (New states habe been completed.)
            Messenger.Broadcast(GameEvent.CHECK_MOVABLE_TURRET);
            // уничтожение предыдущего тайла под игроком и установка нового
            Manager.playerLink.PlayerTileChange(tile);
        }
    }
    public void SpriteUpdate(Tile tile)
    {
        tile.SetSprite(spriteNum);
    }
    public void DangerTilesNumberUpdate(Tile tile)
    {
        tile._dangerTilesNumber = 0;
    }
    public void ChangeOnDanger(Tile tile)
    {
<<<<<<< HEAD
        tile.isDanger = true;
=======
        isDanger = true;
<<<<<<< HEAD
>>>>>>> parent of 9828e7c (New states habe been completed.)
=======
>>>>>>> parent of 9828e7c (New states habe been completed.)
        tile.SetDangerSprite(spriteNum);
    }
    public void ChangeOnSafe(Tile tile)
    {
<<<<<<< HEAD
        tile.isDanger = false;
=======
        isDanger = false;
<<<<<<< HEAD
>>>>>>> parent of 9828e7c (New states habe been completed.)
=======
>>>>>>> parent of 9828e7c (New states habe been completed.)
        tile.SetSprite(spriteNum);
    }
    public void DangerTilesSpawn(Tile tile) { }
    public void NextMove(Tile tile) { }
    public void CheckMovableTurretMove(Tile tile) { }
    public int GetSpriteNum()
    {
        return spriteNum;
    }
}
