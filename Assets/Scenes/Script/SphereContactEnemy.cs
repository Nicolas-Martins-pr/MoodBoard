using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereContactEnemy : MonoBehaviour
{
    [SerializeField]
    private SphereMethod _sphereMethod;

    //Une mécompréhension
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            _sphereMethod.DisableSphere();
            //Appliquer des effets?
        }
    }
}
