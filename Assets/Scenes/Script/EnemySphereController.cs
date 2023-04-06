using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemySphereController : MonoBehaviour
{
    private float _sphereDuration=5;
    private float _sphereTimer;
    [SerializeField]
    private SphereController _sphereController;
    
    void Start()
    {
        
        StartCoroutine(SphereTimer());      
    }
    
    private IEnumerator SphereTimer()
    {
        while (true)
        {

            _sphereController.ResetScale();
            _sphereTimer = _sphereDuration;
            _sphereController.PlaceSphere();
            while (_sphereTimer > 0)
            {
                if (_sphereController.isEnabled == false)
                    break;
                _sphereTimer--;
                _sphereController.ScaleSphere();
                yield return new WaitForSeconds(1);
            }
            _sphereController.DisableSphere();
            yield return new WaitForSeconds(2);
        }
        
    }
}
