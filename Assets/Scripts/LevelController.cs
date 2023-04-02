using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : Singleton<LevelController>
{
    [Header("References")]
    private List<Tile> LevelTileList = new List<Tile>();
    [SerializeField]
    private GameObject TilePrefab;
    [SerializeField]
    private List<Enemy> Enemies = new List<Enemy>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // private Tile GetTileFromPosition() // trouve une tile en fonction de sa position donné par Tiles
}
