using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct TilePosition
{
    public int x;
    public int z;

    
    public TilePosition(int x, int z)
    {
        this.x = x;
        this.z = z;
    }

    public static bool CompareTilePosition(TilePosition tile1, TilePosition tile2)
    {
        return (tile1.x == tile2.x && tile1.z == tile2.z);
    }

    public static TilePosition GetNextTilePositionWithVector3(Vector3 orientation, TilePosition position)
    {
        return new TilePosition((int)orientation.x + position.x, (int)orientation.z + position.z);
    }
}


public class Tile : MonoBehaviour
{
    [Header("Variables")]
    [SerializeField]
    private TilePosition v_position;
    [SerializeField]
    private bool v_isPlayer = false;
    [SerializeField]
    private bool v_isEnemy = false;
    [SerializeField]
    private bool v_isPowerUp = false;

    [Header("Detection")]
    private bool isTileAround;
    public float TileCheckAroundDistance;
    private RaycastHit TileAroundHit;

    [Header("References")]
    [SerializeField]
    private GameObject r_Player;
    [SerializeField]
    private GameObject r_Enemy;
    [SerializeField]
    private GameObject r_Upgrade;
    [SerializeField]
    private List<Tile> r_TilesAround = new List<Tile>();
    
    // Start is called before the first frame update
    void Start()
    {
        CheckTilesAround();
        v_position.x = (int) transform.position.x / 4;
        v_position.z = (int) transform.position.z / 4;
    }

    // Update is called once per frame
    void Update()
    {
        // CheckTilesAround();
        
    }


 
    


    public bool IsEnemy()
    {
        return v_isEnemy;
    }
    public bool IsUpgrade()
    {
        return r_Upgrade;
    }
    public TilePosition GetPosition()
    {
        return v_position;
    }
    public void SetPosition(TilePosition pos)
    {
        v_position = pos;
    }

    private void CheckTilesAround()
    {
        r_TilesAround.Clear();
        Tile tile;
        isTileAround = Physics.Raycast(transform.position, transform.forward, out TileAroundHit, TileCheckAroundDistance, LayerMask.GetMask("Tile"));
        Debug.DrawLine( transform.position, transform.position + transform.forward * TileCheckAroundDistance, Color.green);
        if (isTileAround)
        {
            tile = TileAroundHit.collider.GetComponentInParent<Tile>();
            if (tile != null)
                r_TilesAround.Add(tile);
        }
        isTileAround = Physics.Raycast(transform.position, - transform.forward, out TileAroundHit, TileCheckAroundDistance, LayerMask.GetMask("Tile"));
        Debug.DrawLine( transform.position, transform.position - transform.forward * TileCheckAroundDistance, Color.green);
        if (isTileAround)
        {
            tile = TileAroundHit.collider.GetComponentInParent<Tile>();
            if (tile != null)
            r_TilesAround.Add(tile);
        }
        isTileAround = Physics.Raycast(transform.position, transform.right, out TileAroundHit, TileCheckAroundDistance, LayerMask.GetMask("Tile"));
        Debug.DrawLine( transform.position, transform.position + transform.right* TileCheckAroundDistance, Color.green);
        if (isTileAround)
        {
            tile = TileAroundHit.collider.GetComponentInParent<Tile>();
            if (tile != null)
            r_TilesAround.Add(tile);
        }
        isTileAround = Physics.Raycast(transform.position, - transform.right, out TileAroundHit, TileCheckAroundDistance, LayerMask.GetMask("Tile"));
        Debug.DrawLine( transform.position, transform.position - transform.right * TileCheckAroundDistance, Color.green);
        if (isTileAround)
        {
            tile = TileAroundHit.collider.GetComponentInParent<Tile>();
            if (tile != null)
            r_TilesAround.Add(tile);
        }
        
    }

    public void SetIsEnemy(bool isEnemy)
    {
        v_isEnemy = isEnemy;
    }

    public void SetIsPlayer(bool isPlayer)
    {
        v_isPlayer = isPlayer;
    }

    public List<Tile> GetTiles()
    {
        return r_TilesAround;
    }
}
