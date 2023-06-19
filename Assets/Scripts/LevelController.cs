using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using Extensions;
using UnityEngine;
using Unity.VisualScripting;
using TMPro;

public class LevelController : Singleton<LevelController>
{
    [Header("Variables")]
    [SerializeField]
    private int v_nbEnemy;
    public bool v_isInCombat = false;

    private bool v_LevelGenerated = false;

    [SerializeField]
    private int v_DodgedNbRoom = 30;

    [Header("References")]

    [SerializeField]
    private PlayerController p_Player;

    [SerializeField]
    private Tile r_BossTile;


    [SerializeField]
    private List<Tile> r_LevelTileList = new List<Tile>();
    [SerializeField]
    private List<EnemyController> r_EnemiesList = new List<EnemyController>();
    [SerializeField]
    private GameObject r_TilePrefab;
    [SerializeField]
    private GameObject r_StartLevel;
    [SerializeField]
    public GameObject r_Enemy;

    [SerializeField]
    public GameObject r_Upgrade;
    [SerializeField]
    private WheelRotating _colorWheel;
    [SerializeField]
    private GameObject _combatSystemGO;
    public CombatSystem _combatSystem;

    [SerializeField]
    private HealthBar _healthBar;

    private TextMeshProUGUI timerText;


    [Header("TickRate")]
    [SerializeField]
    private float t_TickRate = 1.5f;
    private float t_Chrono =10f;
    private bool t_FirstTick = true;


    public void Start()
    {
        timerText = _healthBar.gameObject.transform.parent.Find("Timer").gameObject.GetComponent<TextMeshProUGUI>();
    }

    private void LateUpdate() {
        if (!v_LevelGenerated)
        {
        // Récupérer tous les scripts Tile dans les enfants du GameObject parent "Level"
        GameObject level = GameObject.Find("Level");
        p_Player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        r_BossTile = GameObject.FindWithTag("Boss").GetComponent<Tile>();
        _combatSystemGO = p_Player.gameObject.transform.GetComponentInChildren<CombatSystem>().gameObject;
        _combatSystem = _combatSystemGO.GetComponent<CombatSystem>();
            _combatSystemGO.SetActive(false);

            int childCount = level.transform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            Transform childTransform = level.transform.GetChild(i);
            Tile[] tileArray =  childTransform.GetComponentsInChildren<Tile>();
            r_LevelTileList.AddRange(tileArray);
        }
        
        SpawnXEnemies(v_nbEnemy);
        SpawnUpgrade(3,v_DodgedNbRoom);
        SpawnBoss();
        v_LevelGenerated = true;

        // StartCoroutine(MiniMapController.Instance.CreateConnectionsBloc());

        }
    }


    // Update is called once per frame
    void Update()
    {
        if (v_LevelGenerated && !_combatSystem.isActiveAndEnabled)
        {          
            // Pas en combat
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
                        if (r_EnemiesList[i] != null)
                        {
                            StartCoroutine(r_EnemiesList[i].ChoseComportement());
                        }
                        
                    }
                }
                else
                {
                    int EnemiesListLastHalf =(int) Math.Ceiling(r_EnemiesList.Count / 2f);
                    for (int j = EnemiesListLastHalf; j < r_EnemiesList.Count; j++)
                    {
                        if (r_EnemiesList[j] != null)
                        {
                        StartCoroutine(r_EnemiesList[j].ChoseComportement());
                        }
                    }
                    // Update l'autre moitié des unites
                }

                t_Chrono = 0f;    
                t_FirstTick = !t_FirstTick;
            }

        }
        if(v_LevelGenerated && _combatSystem.isActiveAndEnabled)
        {
            // En combat
            _healthBar.SetSliderValue(_combatSystem.GetHealth());
            timerText.text = _combatSystem.GetTimeRemaining().ToString();

            
            
        }


        CheckVictory();



    }

    // Utiliser la liste de scripts Tile récupérée
    public bool TileListPosition(TilePosition tilePosition)
    {
        int i = 0;
        while (!TilePosition.CompareTilePosition(r_LevelTileList[i].GetPosition(), tilePosition) && i < r_LevelTileList.Count-1)
        {
            
            i++;
        }
        if (TilePosition.CompareTilePosition(r_LevelTileList[i].GetPosition(), tilePosition))
        { 
            return true;
        }
        else
        {

            return false;
        }
        
    }


    private void SpawnUpgrade(int nbUpgrade, int DodgeNbRoom)
    {
        List<Tile> tempTileList = r_LevelTileList.GetRange(DodgeNbRoom, r_LevelTileList.Count - DodgeNbRoom);
        tempTileList.Shuffle();
        int it = 0;
        int upgradeSet = 0;
        while (it < tempTileList.Count && upgradeSet != nbUpgrade)
        {
            if(tempTileList[it].gameObject.tag == "BlocUpgrade")
            {
                GameObject up = Instantiate(r_Upgrade, new Vector3 (tempTileList[it].gameObject.transform.position.x,1.5f ,tempTileList[it].gameObject.transform.position.z), tempTileList[it].gameObject.transform.rotation, null);
                switch (upgradeSet)
                {
                    case 0:
                    up.GetComponent<Upgrade>().SetUpgrateType(UpgradeType.VitesseUp);
                    break;
                    case 1:
                    up.GetComponent<Upgrade>().SetUpgrateType(UpgradeType.ExtraShot);
                    break;
                    case 2:
                    up.GetComponent<Upgrade>().SetUpgrateType(UpgradeType.SlowBlackCombat);
                    break;
                    default:
                    up.GetComponent<Upgrade>().SetUpgrateType(UpgradeType.VitesseUp);
                    break;
                }
                upgradeSet++;
            }

            it++;
        }

    }

    #region EnemyControl

    private void SpawnBoss()
    {
        Transform tileTransform = r_BossTile.transform;
        Vector3 posSpawnBoss = new Vector3(tileTransform.position.x, 2f ,tileTransform.position.z);
        GameObject enemy = Instantiate(r_Enemy, posSpawnBoss, tileTransform.rotation, null);
        enemy.transform.localScale = new Vector3(2f,2f,2f);
        r_BossTile.SetIsEnemy(true,enemy.gameObject);
        enemy.transform.SetParent(tileTransform);

    }

    private void CheckVictory()
    {
        if (v_LevelGenerated && !r_BossTile.IsEnemy())
        {
            //Load end scene
            Debug.Log("Win");

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
        Transform tileTransform = tile.transform;
        Vector3 posSpawnEnemy = new Vector3(tileTransform.position.x, 1.5f ,tileTransform.position.z);
        GameObject enemy = Instantiate(r_Enemy, posSpawnEnemy, tileTransform.rotation, null);

        tile.SetIsEnemy(true,enemy.gameObject);
        enemy.transform.SetParent(tileTransform);
        r_EnemiesList.Add(enemy.GetComponent<EnemyController>());
    }
    

    //Modification possible en utilisant le Monobehaviour plutot que enemyController
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
        
        if(newTile != null && !newTile.IsEnemy() && !newTile.IsUpgrade() && !newTile.IsPlayer())
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
    public void EnemyMovementImmunity(EnemyController enemy)
    {
        enemy.GetComponentInParent<Tile>().SetIsEnemy(false, enemy.gameObject);

    }
    public void SetEnemyParent(EnemyController enemy, Tile parent)
    {
        // enemy.GetComponentInParent<Tile>().SetIsEnemy(false, enemy.gameObject);
        enemy.gameObject.transform.SetParent(parent.transform);
        parent.SetIsEnemy(true,enemy.gameObject);
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
        if (newTile != null && newTile.IsEnemy())
        {
            //TODO: Déclencle le combat
            
            newTile.r_Enemy.transform.LookAt(p_Player.transform);
            
            _combatSystemGO.SetActive(true);
            _combatSystem.SetColorWheel(_colorWheel);
            _combatSystem.SetEnemy(newTile.r_Enemy);
            newTile.SetIsEnemy(false,newTile.r_Enemy.gameObject);
            v_isInCombat = true;
            ShowUI.Instance.ActivateUI(true);
            return null;

        }
        else if(newTile != null)
            return newTile;
        else return null;
        
    }

    public void SetPlayerParent(Tile parent)
    {
        p_Player.GetComponentInParent<Tile>().SetIsPlayer(false);
        p_Player.gameObject.transform.SetParent(parent.transform);
        parent.SetIsPlayer(true);
    }

    public void EndCombat()
    {
        v_isInCombat = false;
        ShowUI.Instance.ActivateUI(false);
    }

     
    #endregion
}
