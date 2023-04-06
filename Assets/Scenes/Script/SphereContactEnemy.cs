using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereContactEnemy : MonoBehaviour
{
    [SerializeField]
    private SphereController _sphereController;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            _sphereController.DisableSphere();
            //Appliquer des effets?
        }
    }
}
