using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

 
    private ProceduralLevelBuilderEnum v_State = ProceduralLevelBuilderEnum.Room;

    private ProceduralLevelBuilderEnum v_LastState;

    private int v_SameState = 0;

    [SerializeField]
    private List<GameObject> _Level = new List<GameObject>();


    [SerializeField]
    private List<LevelBloc> _LevelBlocs = new List<LevelBloc>();


    [Header("références")]

    [SerializeField]
    private LevelBloc r_Start;

    [SerializeField]
    private GameObject r_End;

    [SerializeField]
    private List<LevelBloc> r_Connectors;

    [SerializeField]
    private List<LevelBloc> r_Rooms;



    // Start is called before the first frame update
    void Start()
    {
        r_Start.InstancePrefab(new Vector3(0, 0, 0));
        _LevelBlocs.Add(r_Start);
        _Level.Add(r_Start.MyGO);

    }

  
    private void BuildLevel()
    {

    }


    private GameObject GetAnExit(int it)
    {
        int nbExists = _LevelBlocs[it].Exits.Count;
        return _LevelBlocs[it].Exits[Random.Range(0, nbExists -1)];
    }

}
