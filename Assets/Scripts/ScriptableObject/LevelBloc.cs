using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "LevelBloc", menuName = "DungeonCrawler/LevelBloc", order = 0)]
public class LevelBloc : ScriptableObject {
    
    public GameObject Prefab;

    public GameObject MyGO;
    public GameObject Enter;
    public List<GameObject> Exits;
    public GameObject PowerUPZone; 


    private void OnEnable() {
       ClearAll();
       
        
    }

    private void ClearAll()
    {
        Exits.Clear();
    }

    public void InstancePrefab(Vector3 position)
    {
        MyGO = Instantiate(Prefab, position, Quaternion.identity);
        GetInfoPrefab();
    }

    private void GetInfoPrefab()
    {
        // Exits = Prefab.FindGameObjectsWithTag("BlocExit");
        // PowerUPZone = Prefab.FindGameObjectWithTag("BlocUpgrade");
        Transform[] children = MyGO.GetComponentsInChildren<Transform>(true);
        foreach (Transform child in children)
        {
            Debug.Log(child.name);
            if (child.gameObject.tag == "BlocExit")
            {
                Exits.Add(child.gameObject);
            }
            if (child.gameObject.tag == "BlocUpgrade")
            {
                PowerUPZone = child.gameObject;
            }
            if (child.gameObject.tag == "BlocEnter")
            {
                Enter = child.gameObject;
            } 
        }
    }

}
