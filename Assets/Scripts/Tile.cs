using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct TilePosition
{
    public int x;
    public int y;
}

public class Tile : MonoBehaviour
{
    [Header("Variables")]
    [SerializeField]
    private TilePosition v_position;
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
    private GameObject r_Enemy;
    [SerializeField]
    private GameObject r_PowerUp;
    [SerializeField]
    private List<Tile> r_TilesAround = new List<Tile>();
    
    // Start is called before the first frame update
    void Start()
    {
        // CheckTilesAround();
    }

    // Update is called once per frame
    void Update()
    {
        CheckTilesAround();
    }

    private void CheckEnemiesTilesAround()
    {
        List<TilePosition> freeTiles = new List<TilePosition>(); 
        foreach (Tile tile in r_TilesAround)
        {
            if (!tile.IsEnemy())
            {
                freeTiles.Add(tile.GetPosition());
            }
        }
    }

    public bool IsEnemy()
    {
        return v_isEnemy;
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
                Debug.Log("Tile around: ");
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
}
