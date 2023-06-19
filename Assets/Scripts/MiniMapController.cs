using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public struct MiniMapBloc
{
    public Vector3 b_Position;
    public List<Vector3> b_Connectors;
}
public class MiniMapController : Singleton<MiniMapController>
{
    [Header("Références")]
    [SerializeField]
    private GameObject r_MiniMapBlock;

    [SerializeField]
    private GameObject r_MapContainer;

    [SerializeField]
    private List<LevelBloc> r_LevelBloc;

    [Header("Variables")]
    private Vector3 v_PlayerPosition;
    
    private Dictionary<Vector3, GameObject > MiniMapControllerPositions = new Dictionary<Vector3, GameObject>();

    [SerializeField]
    private List<List<Vector3>> v_Connections = new List<List<Vector3>>();

    // private List<Tuple<Vector3, Vector3,Vector3, Vector3>> MiniMapBlocLiaison = new List<Tuple<Vector3, Vector3, Vector3, Vector3>>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // CreateConnectionsBlock();
    }

    public void CreateNewBloc(Vector3 position)
    {
        GameObject bloc = Instantiate(r_MiniMapBlock, new Vector3(position.x/3, position.y - 20, position.z/3), Quaternion.identity);
        MiniMapControllerPositions.Add(new Vector3(position.x/12, 0f, position.z/12), bloc);
        // Debug.Log(new Vector3(position.x/12, 0f, position.z/12));
        
        bloc.transform.SetParent(r_MapContainer.transform);
    }

    public IEnumerator CreateConnectionsBloc() 
    {
        // Debug.Log(r_LevelBloc[2].Exits.Count);
        // Debug.Log(r_LevelBloc[2].MyGO.transform.position);
        // Debug.Log(r_LevelBloc[2].Exits[0].GetComponent<Tile>().GetPosition().x);
        // Debug.Log(r_LevelBloc[2].Exits[0].GetComponent<Tile>().GetPosition().z);
        // Debug.Log(r_LevelBloc[2].Exits[0].transform.position);
        // Vector3 v = ( r_LevelBloc[2].Exits[0].transform.position -  r_LevelBloc[2].MyGO.transform.position );
        
        // v.Normalize();
        // Debug.Log(v );
        
        for (int i = 0; i < r_LevelBloc.Count - 1; i++)
        {
            foreach (GameObject exit in r_LevelBloc[i].Exits)
            {
                Vector3 levelBLocPos = r_LevelBloc[i].MyGO.transform.position;
                Vector3 v = (exit.transform.position -  levelBLocPos );
                v.Normalize();
                Vector3 newBlocPos = new Vector3(levelBLocPos.x + v.x,levelBLocPos.y + v.y,levelBLocPos.z + v.z );
                GameObject newBloc;
                if (MiniMapControllerPositions.TryGetValue(newBlocPos, out newBloc))
                {
                    TilePosition tp=exit.GetComponent<Tile>().GetPosition();
                    TilePosition tpnew = new TilePosition(tp.x + (int)v.x, tp.z + (int)v.z);
                    Debug.Log("A chercher : " + tpnew.x + " " + tpnew.z + " " );
                    if(LevelController.Instance.TileListPosition(new TilePosition(tp.x + (int)v.x, tp.z + (int)v.z)))
                    {
                        // add newBLocPos to v_connections[actualBlocIndex]
                        if(v_Connections[i] == null)
                        {
                            v_Connections.Add(new List<Vector3>());
                            v_Connections[i].Add(newBlocPos);
                            Debug.Log("in");
                        }
                        else
                        {
                            v_Connections[i].Add(newBlocPos);
                            Debug.Log("in");
                        }
                    }
                }
            }
        }
        yield return null;

    }

    public void SetLevelBloc(List<LevelBloc> _LevelBlocs)
    {
        r_LevelBloc = _LevelBlocs;
    }

    private Vector3 GetDirectionNewBloc(Vector3 positionExit, Vector3 positionCenter)
    {
        Vector3 directionNewBloc =  new Vector3(positionExit.x - positionCenter.x , positionExit.y - positionCenter.y, positionExit.z - positionCenter.z);
        directionNewBloc.Normalize();
        return directionNewBloc;
    }
}
