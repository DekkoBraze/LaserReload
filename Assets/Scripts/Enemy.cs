using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
   [SerializeField] public Player player;

    protected virtual void Start()
    {
        player = FindObjectOfType<Player>();
        this.gameObject.transform.position = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, this.gameObject.transform.position.z - 1);
    }
    public virtual void OnMouseDown()
    {
        if (player != null && player.EnemyHitCheck(this.gameObject.transform.position))
        {
                Destroy(this.gameObject);
        }
    }
}
