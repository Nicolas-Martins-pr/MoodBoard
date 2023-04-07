using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Extensions;


   public enum ProceduralLevelBuilderEnum
    {
        Corridor,
        Room
    }

public class ProceduralLevelBuilder : MonoBehaviour
{

    [Header("Variables")]

    [SerializeField]
    private float v_NbRooms;

    [SerializeField]
    private float v_NbCorridorsMax= 2;

    [SerializeField]
    private int v_LevelSizeMax= 12;

 
    private ProceduralLevelBuilderEnum v_State = ProceduralLevelBuilderEnum.Room;

    private ProceduralLevelBuilderEnum v_LastState;

    private int v_SameState = 0;

    private int v_NBSameState; // Nombre de corridor à mettre avant de changer

    [SerializeField]
    private Vector3 v_StartPosition = new Vector3(0,0,0);

    [Header("BuildingStructure")]

    [SerializeField]
    private int _ItHeadStructure = 0;

    private Vector3 _directionNewPos;
    private Vector3 _NewPos;

    private LevelBloc _ActualLevelBloc;
    


    [SerializeField]
    private List<GameObject> _Level = new List<GameObject>();


    [SerializeField]
    private List<LevelBloc> _LevelBlocs = new List<LevelBloc>();

    [SerializeField]
    private List<Vector3> _LevelBlocPos = new List<Vector3>();

    [Header("références")]

    [SerializeField]
    private LevelBloc r_Start;

    [SerializeField]
    private LevelBloc r_End;

    [SerializeField]
    private List<LevelBloc> r_Connectors;

    [SerializeField]
    private List<LevelBloc> r_Rooms;



    // Start is called before the first frame update
    void Start()
    {

        //Pose du premier bloc
        r_Start.InstancePrefab(v_StartPosition);
        _LevelBlocs.Add(r_Start);
        _Level.Add(r_Start.MyGO);
        _LevelBlocPos.Add(v_StartPosition);
        //
        BuildLevel();
    }

  
    private void BuildLevel()
    {
        //On trouve une sortie du cube. On compare la position de la sortie au centre pour savoir la direction du prochain bloc puis on le pose et on enchaine.
        GameObject ExitActualBloc = GetAnExit(_ItHeadStructure);
        if(_LevelBlocs.Count == v_LevelSizeMax) CreateFinalZones();
        else if (ExitActualBloc != null)
        {
            //Création new Bloc
            ChooseNewBloc();
            CreateNewBloc(_NewPos, _directionNewPos, _ActualLevelBloc);

            BuildLevel();
        }
        else
        {
            // reduit la tete de lecture et on recommence si la tete de lecture arrive à 0 et plus rien on sort et on met une fin
            _ItHeadStructure--;
            BuildLevel();
        }
    }

    private void CreateNewBloc(Vector3 position, Vector3 direction, LevelBloc bloc)
    {
        LevelBloc newBloc = ScriptableObject.Instantiate(bloc) as LevelBloc;
        //
        newBloc.InstancePrefab(position);

        float angle = Vector3.SignedAngle(direction, GetDirectionNewBloc(position,newBloc.Enter.transform.position),Vector3.up);
        newBloc.MyGO.transform.Rotate(new Vector3(0f,- angle,0f));

        if (_ItHeadStructure == _LevelBlocs.Count -1)
        {
            _LevelBlocs.Add(newBloc);
            _Level.Add(newBloc.MyGO);
            _LevelBlocPos.Add(position);
            _ItHeadStructure++;
        }
        else
        {
            _LevelBlocs.Insert(_ItHeadStructure,newBloc);
            _Level.Insert(_ItHeadStructure,newBloc.MyGO);
            _LevelBlocPos.Insert(_ItHeadStructure,position);
            _ItHeadStructure++;
        }
        //



    }

    private void ChooseNewBloc()
    {
        if (v_State == ProceduralLevelBuilderEnum.Room)
        {
            ChooseNbCorridor();
            if (v_NBSameState > 0) // doit poser un corridor ou plus
                v_State = ProceduralLevelBuilderEnum.Corridor;
            else // on remet directement une room
            {
                v_State = ProceduralLevelBuilderEnum.Room;
            }
            SetNewBloc();
            v_SameState = 1;
        }
        else if (v_State == ProceduralLevelBuilderEnum.Corridor && v_SameState < v_NBSameState)
        {
            // On repose un corridor 
            SetNewBloc();
            v_SameState++;
        }
        else if (v_State == ProceduralLevelBuilderEnum.Corridor && v_SameState == v_NBSameState)
        {
            //On switch to room
            v_State = ProceduralLevelBuilderEnum.Room;
            SetNewBloc();
        }
    }

    private void ChooseNbCorridor()
    {
        v_NBSameState =  Mathf.RoundToInt(Random.Range(0, v_NbCorridorsMax + 1));
    }
    private void SetNewBloc()
    {
        //récupère le state et fait un random sur la list et récupère l'un deux dans une variable
        switch (v_State)
        {
            case ProceduralLevelBuilderEnum.Room:
            _ActualLevelBloc = r_Rooms[ Mathf.RoundToInt(Random.Range(0, r_Rooms.Count))];
            // Random dans la list room et set actualBLoc
            break;
            case ProceduralLevelBuilderEnum.Corridor:
            // Random dans la list Corridor et set actualBLoc
            _ActualLevelBloc = r_Connectors[ Mathf.RoundToInt(Random.Range(0, r_Connectors.Count))];

            break;
            default:
            // Random dans la list room et set actualBLoc
            _ActualLevelBloc = r_Rooms[ Mathf.RoundToInt(Random.Range(0, r_Rooms.Count))];

            break;
        }

    }

    private GameObject GetAnExit(int it)
    {
    //     int nbExists = _LevelBlocs[it].Exits.Count;
    //     return _LevelBlocs[it].Exits[Random.Range(0, nbExists -1)];
        //IsAlreadyBlocInPosition 
        Vector3 posExit;
        

        _LevelBlocs[it].Exits.Shuffle();
        int jt = 0;
        while (jt < _LevelBlocs[it].Exits.Count)
        {
            _directionNewPos = GetDirectionNewBloc(_LevelBlocs[it].Exits[jt].transform.position,_LevelBlocPos[it]);
            _NewPos =GetPositionNewBloc(_LevelBlocPos[it], _directionNewPos);
            if (!IsAlreadyBlocInPosition(_NewPos))
            {
                return _LevelBlocs[it].Exits[jt];
            }
            jt++;   
        }
        return null;
    }

    private Vector3 GetDirectionNewBloc(Vector3 positionExit, Vector3 positionCenter)
    {
        Vector3 directionNewBloc =  new Vector3(positionExit.x - positionCenter.x , positionExit.y - positionCenter.y, positionExit.z - positionCenter.z);
        directionNewBloc.Normalize();
        return directionNewBloc;
    }

    private Vector3 GetPositionNewBloc(Vector3 positionCenter, Vector3 direction)
    {
        return new Vector3(positionCenter.x + 12* direction.x , positionCenter.y + 12* direction.y , positionCenter.z + 12* direction.z);
    }

    private bool IsAlreadyBlocInPosition(Vector3 position)
    {
        int it = 0;
        while (it <_LevelBlocPos.Count)
        {
            
            if(Vector3.Equals(_LevelBlocPos[it], position))
                return true;
            it++;
        }
        return false;
    }

    private void CreateFinalZones()
    {
        int itFarestPos = 0;
        float distance = 0;
        float distanceTemp = 0;
        for (int i = 0; i < _LevelBlocPos.Count; i++)
        {
            distanceTemp = Vector3.Distance(v_StartPosition, _LevelBlocPos[i]);
            if (distance < distanceTemp)
            {
                
                GameObject ExitActualBloc= GetAnExit(i);
                if (ExitActualBloc != null)
                {
                    distance = distanceTemp;
                    itFarestPos = i;
                }
            }
        }
        
        
        CreateNewBloc(_NewPos,_directionNewPos, r_End);
        
        // else
        // {
        //     _directionNewPos = GetDirectionNewBloc(_LevelBlocs[itFarestPos].Enter.transform.position,_LevelBlocPos[itFarestPos]);
        //     _NewPos =GetPositionNewBloc(_LevelBlocs[itFarestPos].Enter.transform.position, _directionNewPos);
        // }

    }

}
