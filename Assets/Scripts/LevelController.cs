using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using Extensions;
using UnityEngine;

public class LevelController : Singleton<LevelController>
{
    [Header("Variables")]
    [SerializeField]
    private int v_nbEnemy;

    private bool v_LevelGenerated = false;

    [Header("References")]

    [SerializeField]
    private PlayerController p_Player;


    [SerializeField]
    private List<Tile> r_LevelTileList = new List<Tile>();
    [SerializeField]
    private List<EnemyController> r_EnemiesList = new List<EnemyController>();
    [SerializeField]
    private GameObject r_TilePrefab;
    [SerializeField]
    private GameObject r_StartLevel;
    [SerializeField]
    private GameObject r_Enemy;

    [Header("TickRate")]
    [SerializeField]
    private float t_TickRate = 1.5f;
    private float t_Chrono =10f;
    private bool t_FirstTick = true;
    
    // private List<Enemy> Enemies = new List<Enemy>();
    // Start is called before the first frame update
    // void Start()
    // {
    //     // Récupérer tous les scripts Tile dans les enfants du GameObject parent "Level"
    //     GameObject level = GameObject.Find("Level");
    //     p_Player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();


    //       int childCount = level.transform.childCount;
    //     for (int i = 0; i < childCount; i++)
    //     {
    //         Transform childTransform = level.transform.GetChild(i);
    //         Tile[] tileArray =  childTransform.GetComponentsInChildren<Tile>();
    //         r_LevelTileList.AddRange(tileArray);
    //     }



    //     // Tile[] tileArray = level.GetComponentsInChildren<Tile>();
    //     // r_LevelTileList = new List<Tile>(tileArray);

    //     // Vérifier si les scripts Tile ont été récupérés
    //     if (r_LevelTileList.Count > 0)
    //     {
    //         Debug.Log("Nombre de Tile récupérés : " + r_LevelTileList.Count);
    //     }
    //     else
    //     {
    //         Debug.LogWarning("Aucun Tile trouvé !");
    //     }

    //     SpawnXEnemies(v_nbEnemy);
    // }

    private void LateUpdate() {
        if (!v_LevelGenerated)
        {
        // Récupérer tous les scripts Tile dans les enfants du GameObject parent "Level"
        GameObject level = GameObject.Find("Level");
        p_Player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();


          int childCount = level.transform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            Transform childTransform = level.transform.GetChild(i);
            Tile[] tileArray =  childTransform.GetComponentsInChildren<Tile>();
            r_LevelTileList.AddRange(tileArray);
        }



        // Tile[] tileArray = level.GetComponentsInChildren<Tile>();
        // r_LevelTileList = new List<Tile>(tileArray);

        // Vérifier si les scripts Tile ont été récupérés
        // if (r_LevelTileList.Count > 0)
        // {
        //     Debug.Log("Nombre de Tile récupérés : " + r_LevelTileList.Count);
        // }
        // else
        // {
        //     Debug.LogWarning("Aucun Tile trouvé !");
        // }

        SpawnXEnemies(v_nbEnemy);
        v_LevelGenerated = true;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (v_LevelGenerated)
        {
            
            
            if (t_Chrono < t_TickRate )
                t_Chrono += Time.deltaTime;
            else
            {   
                if(t_FirstTick)
                {
                    // Update la moitié des unites
                    int EnemiesListFirstHalf = (int)Math.Ceiling(r_EnemiesList.Count / 2f);
                    for (int i = 0; i < EnemiesListFirstHalf; i++)
                    {
                        StartCoroutine(r_EnemiesList[i].ChoseComportement());
                    }
                }
                else
                {
                    int EnemiesListLastHalf =(int) Math.Ceiling(r_EnemiesList.Count / 2f);
                    for (int j = EnemiesListLastHalf; j < r_EnemiesList.Count; j++)
                    {
                        StartCoroutine(r_EnemiesList[j].ChoseComportement());
                    }
                    // Update l'autre moitié des unites
                }

                t_Chrono = 0f;    
                t_FirstTick = !t_FirstTick;
            }
        }
    }

    // Utiliser la liste de scripts Tile récupérée
    private void UseTileList()
    {
        foreach (Tile tile in r_LevelTileList)
        {
            // Faire quelque chose avec chaque script Tile
        }
    }


    #region EnemyControl
    private void SpawnXEnemies(int nbEnemy)
    {
        int nbSpawned = nbEnemy;
        List<Tile> tempTileList = r_LevelTileList.ToList();
        tempTileList.Shuffle();
        if (r_LevelTileList.Count < nbEnemy)
        {
            nbSpawned = r_LevelTileList.Count;
        }
        for (int i = 0; i < nbSpawned; i++)
        {
            SpawnEnemyInTile(tempTileList[i]);
        }
    }

    private void SpawnEnemyInTile(Tile tile)
    {
        tile.SetIsEnemy(true);
        Transform tileTransform = tile.transform;
        Vector3 posSpawnEnemy = new Vector3(tileTransform.position.x, 1.5f ,tileTransform.position.z);
        GameObject enemy = Instantiate(r_Enemy, posSpawnEnemy, tileTransform.rotation, null);
        enemy.transform.SetParent(tileTransform);
        r_EnemiesList.Add(enemy.GetComponent<EnemyController>());
    }
    // private Tile GetTileFromPosition() // trouve une tile en fonction de sa position donné par Tiles

    //Modification possible en utilisant le Monobehaviour plutot que enemyController
    // public TilePosition GetPositionEntity(MonoBehaviour entity)
    
    public TilePosition GetPositionEntity(MonoBehaviour entity)
    {
            
        return entity.gameObject.GetComponentInParent<Tile>().GetPosition();
    }

    public Tile CanEnemyTravelToForwardTIle(EnemyController enemy)
    {
        // récupération de l'orientation de l'ennemi. Recup position de la tile. Récup la tile devant l'ennemi. Regarde si la tile possède un ennemi ou une amélioration. Si non. Renvoie la tile si oui renvoi null.
        Vector3 enemyDirection = enemy.gameObject.transform.forward;
        TilePosition enemyPosition = GetPositionEntity(enemy);
        TilePosition Next_enemyPosition = TilePosition.GetNextTilePositionWithVector3(enemyDirection, enemyPosition);
        Tile newTile =  GetTileAroundFromPosition(enemy.GetComponentInParent<Tile>(), Next_enemyPosition);
        
        if(newTile != null && !newTile.IsEnemy() && !newTile.IsUpgrade())
            return newTile;
        else return null;
        
    }

    public Tile GetTileAroundFromPosition(Tile tile, TilePosition tilePosition)
    {
        int it = 0;
        List<Tile> TilesAroundTile = tile.GetTiles(); 

        while(it < TilesAroundTile.Count)
        {
            if(TilePosition.CompareTilePosition( TilesAroundTile[it].GetPosition(), tilePosition))
            {
                return (TilesAroundTile[it]);
            }
            it++;
        }
        return null;
    }
       //Fonction à transférer dans le levelcontroller
    private void CheckEnemiesTilesAround(Tile tile)
    {
        List<TilePosition> freeTiles = new List<TilePosition>(); 
        List<Tile> TilesAroundTile = tile.GetTiles(); 
        
        foreach (Tile it_tile in TilesAroundTile)
        {
            if (!it_tile.IsEnemy())
            {
                freeTiles.Add(it_tile.GetPosition());
            }
        }
    }

    //Fonction à transférer dans le levelcontroller 

    public void SetEnemyParent(EnemyController enemy, Tile parent)
    {
        enemy.GetComponentInParent<Tile>().SetIsEnemy(false);
        enemy.gameObject.transform.SetParent(parent.transform);
        parent.SetIsEnemy(true);
    }
    #endregion

    #region PlayerControl

    public Tile CanPlayerTravelToForwardTIle()
    {
        // récupération de l'orientation de l'ennemi. Recup position de la tile. Récup la tile devant l'ennemi. Regarde si la tile possède un ennemi ou une amélioration. Si non. Renvoie la tile si oui renvoi null.
        Vector3 playerDirection = p_Player.gameObject.transform.forward;
        TilePosition playerPosition = GetPositionEntity(p_Player);
        TilePosition Next_playerPosition = TilePosition.GetNextTilePositionWithVector3(playerDirection, playerPosition);
        Tile newTile =  GetTileAroundFromPosition(p_Player.GetComponentInParent<Tile>(), Next_playerPosition);
        
        if(newTile != null)
            return newTile;
        else return null;
        
    }

    public void SetPlayerParent(Tile parent)
    {
        p_Player.GetComponentInParent<Tile>().SetIsPlayer(false);
        p_Player.gameObject.transform.SetParent(parent.transform);
        parent.SetIsPlayer(true);
    }

     
    #endregion
}
