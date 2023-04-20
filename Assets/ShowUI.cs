using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowUI : Singleton<ShowUI>
{
    // Start is called before the first frame update

    private void Start() {
        ActivateUI(false);
    }
    public void ActivateUI(bool activated)
    {
        
        Transform parentTransform = transform;

        // Parcourir tous les enfants de l'objet parent et les désactiver
        for (int i = 0; i < parentTransform.childCount; i++)
        {
            Transform childTransform = parentTransform.GetChild(i);
            childTransform.gameObject.SetActive(activated);
        }
    }
}
