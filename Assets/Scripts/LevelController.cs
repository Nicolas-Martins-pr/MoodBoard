using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : Singleton<LevelController>
{
    [Header("References")]
    private List<Tile> LevelTileList;
    [SerializeField]
    private GameObject TilePrefab;
    [SerializeField]
    private GameObject StartLevel;
    [SerializeField]
    // private List<Enemy> Enemies = new List<Enemy>();
    // Start is called before the first frame update
    void Start()
    {
        // Récupérer tous les scripts Tile dans les enfants du GameObject parent "Level"
        GameObject level = GameObject.Find("Level");
        Tile[] tileArray = level.GetComponentsInChildren<Tile>();
        LevelTileList = new List<Tile>(tileArray);

        // Vérifier si les scripts Tile ont été récupérés
        if (LevelTileList.Count > 0)
        {
            Debug.Log("Nombre de Tile récupérés : " + LevelTileList.Count);
        }
        else
        {
            Debug.LogWarning("Aucun Tile trouvé !");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Utiliser la liste de scripts Tile récupérée
    private void UseTileList()
    {
        foreach (Tile tile in LevelTileList)
        {
            // Faire quelque chose avec chaque script Tile
        }
    }

    // private Tile GetTileFromPosition() // trouve une tile en fonction de sa position donné par Tiles

}
