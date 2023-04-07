using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SphereController : MonoBehaviour
{
    private float _sphereDuration=5;
    private float _sphereTimer;
    [SerializeField]
    private SphereMethod _sphereMethod;
    
    public void OnEnable() { 
        _sphereMethod = GetComponent<SphereMethod>();
        StartCoroutine(SphereTimer());      
    }


    //Gère le timer de la sphere, et fait appel au fonction de la sphere pour la faire grossir, disparaitre et réapparaitre
    private IEnumerator SphereTimer()
    {
        while (true)
        {

            _sphereMethod.ResetScale();
            _sphereTimer = _sphereDuration;
            _sphereMethod.PlaceSphere();
            while (_sphereTimer > 0)
            {
                if (_sphereMethod.isEnabled == false)
                    break;
                _sphereTimer--;
                _sphereMethod.ScaleSphere();
                yield return new WaitForSeconds(1);
            }
            _sphereMethod.DisableSphere();
            yield return new WaitForSeconds(2);
        }
        
    }
}
