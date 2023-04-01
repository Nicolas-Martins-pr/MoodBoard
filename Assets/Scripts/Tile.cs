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
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
}
