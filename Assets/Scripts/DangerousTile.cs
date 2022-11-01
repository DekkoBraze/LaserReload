using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DangerousTile : Tile
{
    protected override void Start()
    {
        base.Start();
        this.gameObject.transform.position = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, -1);
    }
}
