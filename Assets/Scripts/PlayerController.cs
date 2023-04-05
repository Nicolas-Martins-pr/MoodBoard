using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public enum MovementPlayerState
{
    Immobile,
    RotateLeft,
    RotateRight,
    MoveForward
}

public class PlayerController : Singleton<PlayerController>
{
    [Header("Variables")]

    [SerializeField] 
    private MovementPlayerState v_MovementPlayerState;

    [SerializeField]
    private float v_MovementDuration = 0.9f;

    [Header("DelayMovement")]
    [SerializeField]
    private float d_TickRate = 1.5f; // Delay entre 2 movmement
    private float d_Chrono =10f;
    private bool d_FirstTick = true;

    [Header("Detections")]

    [SerializeField]
    
    private bool d_FrontWall = false;
    private RaycastHit d_FrontWallHit;

    [SerializeField]
    private float d_WallCheckDistance = 3f;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Prise en compte des inputs
        if (d_Chrono < d_TickRate )
            d_Chrono += Time.deltaTime;
            
        else
        {   
           //DoMovement selected
        //    d_Chrono = 0f; 
        }
    }

    private void Move()
    {
        Tile tile = LevelController.Instance.CanPlayerTravelToForwardTIle();
        if (tile != null)
        {
            StartCoroutine(DoMovement(tile));
        }
        else Rotate();
        // Lui donne la tile vers laquelle il se dirige et se set en fils
        // Move forward obligatoire

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
        LevelController.Instance.SetPlayerParent(newTile);
        transform.position =  targetPosition;
    }


    private void Rotate()
    {
        StartCoroutine(DoRotate());
    }

    private IEnumerator DoRotate()
    {
        // Determine la direction de la rotation
        float rand = UnityEngine.Random.value;
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

    

    private bool CheckFrontWall()
    {
        //Raycast pour voir si wall en face. Si wall obligé de rotate sinon 25% rotate 75% Move forward
        return  Physics.Raycast(transform.position, transform.forward, out d_FrontWallHit, d_WallCheckDistance, LayerMask.GetMask("Wall"));
    }
}
