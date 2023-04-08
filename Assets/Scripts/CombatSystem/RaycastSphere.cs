using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastSphere : MonoBehaviour
{
    // Start is called before the first frame update


    public float maxDistance = 10f; // Distance maximale du raycast
    public float coneAngle = 30f; // Angle d'ouverture du c�ne en degr�s
    public float sphereRadius = 0.5f; // Rayon de la sph�re de d�tection
    public LayerMask layerMask; // Couches � prendre en compte pour la d�tection

    [SerializeField]
    private List<GameObject> potentialTargets = new List<GameObject>();

   
    public List<GameObject> GetTarget()
    {
        potentialTargets.Clear();
        Vector3 rayDirection = transform.forward;
        
        // Calcul de l'angle du c�ne
        float halfConeAngle = coneAngle / 2f;

        // Calcul des angles de d�but et de fin du c�ne
        Quaternion coneRotation = Quaternion.LookRotation(rayDirection);
        Quaternion coneLeftRotation = Quaternion.AngleAxis(-halfConeAngle, transform.up) * coneRotation;
        Quaternion coneRightRotation = Quaternion.AngleAxis(halfConeAngle, transform.up) * coneRotation;

        // Lancement des raycasts
        RaycastHit[] leftHits = Physics.SphereCastAll(transform.position, sphereRadius, coneLeftRotation * Vector3.forward, maxDistance, layerMask);
        RaycastHit[] forwardHits = Physics.SphereCastAll(transform.position, sphereRadius, rayDirection, maxDistance, layerMask);
        RaycastHit[] rightHits = Physics.SphereCastAll(transform.position, sphereRadius, coneRightRotation * Vector3.forward, maxDistance, layerMask);

        // Fusion des r�sultats des raycasts
        RaycastHit[] hits = new RaycastHit[leftHits.Length + forwardHits.Length + rightHits.Length];
        forwardHits.CopyTo(hits, 0);
        leftHits.CopyTo(hits, forwardHits.Length);
        rightHits.CopyTo(hits, forwardHits.Length + leftHits.Length);

        // Traitement des r�sultats des raycasts
        foreach (RaycastHit hit in hits)
        {
            // Faites ici ce que vous voulez avec les collisions d�tect�es
            potentialTargets.Add(hit.collider.gameObject);
            // Dessin des lignes de d�bogage pour chaque raycast
            Debug.DrawLine(transform.position, hit.point, Color.red);
        }
            Debug.Log(potentialTargets.Count);
            if (potentialTargets.Count > 0)
            {
                Debug.Log(potentialTargets);
                return potentialTargets;
            }
            else
            {
                return null;
            }
        }

    }

