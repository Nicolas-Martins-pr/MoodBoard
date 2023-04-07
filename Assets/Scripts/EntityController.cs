using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Classe mère de playerController et EnemyController Cependant pas encore implémenté et pas sur de le faire
public class EntityController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    protected virtual void Move()
    {


    }

    protected virtual void Rotate()
    {
    }

    protected virtual IEnumerator DoRotate()
    {
       yield return null;
    }

    protected virtual IEnumerator DoMovement(Tile newTile)
    {
        yield return null;
    }

    // protected bool CheckFrontWall()
    // {
    //     //Raycast pour voir si wall en face. Si wall obligé de rotate sinon 25% rotate 75% Move forward
    //     return  Physics.Raycast(transform.position, transform.forward, out v_FrontWallHit, v_WallCheckDistance, LayerMask.GetMask("Wall"));
    // }
}
