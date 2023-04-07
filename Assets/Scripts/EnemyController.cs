using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("Variables")]
    [SerializeField]
    private float v_MovementDuration = 0.9f;

    [Header("Detections")]

    [SerializeField]
    
    private bool d_FrontWall = false;
    private RaycastHit d_FrontWallHit;

    [SerializeField]
    private float d_WallCheckDistance = 3f;
    // Start is called before the first frame update

    private LayerMask d_Mask;
    void Start()
    {
        d_Mask = LayerMask.GetMask("Wall") + LayerMask.GetMask("WallLimiter");
    }


    public IEnumerator ChoseComportement()
    {
        if(CheckFrontWall())
        {
            Rotate();
        }
        else
        {
            float rand = Random.value;

            if(rand < 0.70f)
            {
                Move();
            }
            else if (rand < 0.90f)
            {
                Rotate();
            }
            else
            {
                
            }
        }
        yield return null;
        //Tir un aléatoire, si valeur <= proba Move alors récup tile;
        // Move();
        //Sinon
        //Rotate();
        //Sinon Stay();
    }

    private void Move()
    {
        Tile tile = LevelController.Instance.CanEnemyTravelToForwardTIle(this);
        if (tile != null)
        {
            StartCoroutine(DoMovement(tile));
        }
        else Rotate();
        // Lui donne la tile vers laquelle il se dirige et se set en fils
        // Move forward obligatoire

    }

    private void Rotate()
    {
        StartCoroutine(DoRotate());
    }

    private IEnumerator DoRotate()
    {
        // Determine la direction de la rotation
        float rand = Random.value;
        float sign = rand < 0.5f ? -1f : 1f;

        // Rotation initiale du transform
        Quaternion startRotation = transform.rotation;

        // Rotation cible
        Quaternion targetRotation = Quaternion.AngleAxis(90f * sign, Vector3.up) * startRotation;

        // Durée de la rotation
        float duration = 0.5f;

        // Temps écoulé depuis le début de la rotation
        float time = 0f;

        while (time < duration)
        {
            // Calcul de l'interpolation de rotation
            float t = time / duration;
            transform.rotation = Quaternion.Lerp(startRotation, targetRotation, t);

            // Attend une frame
            yield return null;

            // Mise à jour du temps écoulé
            time += Time.deltaTime;
        }

        // Assure que la rotation est bien effectuée à la fin
        transform.rotation = targetRotation;
    }

    private IEnumerator DoMovement(Tile newTile)
    {
        //While tant que pas arrivé à la position.
        //SetParent quand arrivé comtroller
        float time = 0f;
        Vector3 targetPosition = new Vector3(newTile.transform.position.x, 1.5f ,newTile.transform.position.z);
        Transform tileTransform = this.GetComponentInParent<Tile>().transform;
        Vector3 startPosition = new Vector3(tileTransform.position.x, 1.5f ,tileTransform.position.z);
        
        while(time < v_MovementDuration)
        {
            time += Time.deltaTime;
            float t = Mathf.Clamp01(time / v_MovementDuration);
            transform.position = Vector3.Lerp(startPosition, targetPosition, t);
            yield return null;
        }
        LevelController.Instance.SetEnemyParent(this, newTile);
        transform.position =  targetPosition;
    }

    private bool CheckFrontWall()
    {
        //Raycast pour voir si wall en face. Si wall obligé de rotate sinon 25% rotate 75% Move forward
        return  Physics.Raycast(transform.position, transform.forward, out d_FrontWallHit, d_WallCheckDistance, d_Mask);
    }
}
