using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField ] public Player player;

    protected virtual void Start()
    {
        player = FindObjectOfType<Player>();
    }

    public void OnMouseDown()
    {
        if (player != null)
        {
            player.Move(this);
        }
    }
}
