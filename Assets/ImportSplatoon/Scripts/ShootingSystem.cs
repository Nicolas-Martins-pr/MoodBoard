using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ShootingSystem : MonoBehaviour
{

    

    [SerializeField] ParticleSystem inkParticle;
    [SerializeField] Transform parentController;
    [SerializeField] Transform splatGunNozzle;

    void Update()
    {
        Vector3 angle = parentController.localEulerAngles;
        
        bool pressing = Input.GetMouseButton(0);

 
        if (Input.GetMouseButtonDown(0))
            inkParticle.Play();
        else if (Input.GetMouseButtonUp(0))
            inkParticle.Stop();

       
    }

   
    float RemapCamera(float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }
}
