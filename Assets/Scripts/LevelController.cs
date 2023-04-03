using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Extensions;
using UnityEngine;

public class LevelController : Singleton<LevelController>
{
    [Header("References")]
    [SerializeField]
    private List<Tile> r_LevelTileList;
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
    void Start()
    {
        // Récupérer tous les scripts Tile dans les enfants du GameObject parent "Level"
        GameObject level = GameObject.Find("Level");
        Tile[] tileArray = level.GetComponentsInChildren<Tile>();
        r_LevelTileList = new List<Tile>(tileArray);

        // Vérifier si les scripts Tile ont été récupérés
        if (r_LevelTileList.Count > 0)
        {
            Debug.Log("Nombre de Tile récupérés : " + r_LevelTileList.Count);
        }
        else
        {
            Debug.LogWarning("Aucun Tile trouvé !");
        }

        SpawnXEnemies(3);
    }

    // Update is called once per frame
    void Update()
    {
        if (t_Chrono < t_TickRate )
            t_Chrono += Time.deltaTime;
        else
        {   
            if(t_FirstTick)
            {
                // Update la moitié des unites
            }
            else
            {
                // Update l'autre moitié des unites
            }

            t_Chrono = 0f;    
            t_FirstTick = !t_FirstTick;
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
        Transform tileTransform = tile.gameObject.transform;
        Vector3 posSpawnEnemy = new Vector3(tileTransform.position.x, 1.5f ,tileTransform.position.z);
        GameObject enemy = Instantiate(r_Enemy, posSpawnEnemy, tileTransform.rotation, null);
        enemy.transform.SetParent(tileTransform);
        r_EnemiesList.Add(enemy.GetComponent<EnemyController>());
    }
    // private Tile GetTileFromPosition() // trouve une tile en fonction de sa position donné par Tiles

}
