using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereContactEnemy : MonoBehaviour
{
    [SerializeField]
    private SphereMethod _sphereMethod;

    //Une m�compr�hension
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            _sphereMethod.DisableSphere();
            //Appliquer des effets?
        }
    }
}
