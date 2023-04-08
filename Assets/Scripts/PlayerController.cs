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
    [Header("Action")]
    private bool _Action= false;

    [Header("Variables")]

    [SerializeField] 
    private MovementPlayerState v_MovementPlayerState;

    [SerializeField]
    private float v_MovementDuration = 0.9f;
    [SerializeField]
    private float v_RotationDuration = 0.5f;

    [SerializeField]
    private bool v_MoveSpeedUp = false;

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
        if(!LevelController.Instance.v_isInCombat)
        {
            CheckDuration();
            CheckInput();
            //Prise en compte des inputs
            if (d_Chrono < d_TickRate )
                d_Chrono += Time.deltaTime;
                
            else
            {   
                if (_Action)
                    MakeMovement();
            //DoMovement selected
            //    d_Chrono = 0f; 
            }
        }
    }

    //Vérifie que les actions se résolvent toujours plus rapidement que le TickRate !! Secours !!
    private void CheckDuration()
    {
        if (d_TickRate <= v_MovementDuration || d_TickRate <= v_RotationDuration)
        {
            v_MovementDuration = d_TickRate - 0.1f;
            v_RotationDuration = d_TickRate - 0.1f;
        }
    }

    private void CheckInput()
    {
        if (Input.GetButtonDown("RotateRight"))
        {
            v_MovementPlayerState = MovementPlayerState.RotateRight;
            _Action = true;
        }
        else if (Input.GetButtonDown("RotateLeft"))
        {
            v_MovementPlayerState = MovementPlayerState.RotateLeft; 
            _Action = true;
        }
        else if (Input.GetButtonDown("Move"))
        {
            v_MovementPlayerState = MovementPlayerState.MoveForward;   
            _Action = true;
        }
        else if (Input.anyKeyDown&& !Input.GetMouseButton(0)&& !Input.GetMouseButton(1)&& !Input.GetMouseButton(2))
        {
            v_MovementPlayerState = MovementPlayerState.Immobile;
            _Action = false;
        }
    }

    private void MakeMovement()
    {
        switch (v_MovementPlayerState)
        {
            case MovementPlayerState.Immobile:
            break;
            case MovementPlayerState.MoveForward:
            Move();
            break;
            case MovementPlayerState.RotateLeft:
            Rotate(-1);
            break;
            case MovementPlayerState.RotateRight:
            Rotate(1);
            break;
            default:
            break;
        }

        v_MovementPlayerState =  MovementPlayerState.Immobile;
        d_Chrono = 0f;
        _Action = false;
    }

    private void Move()
    {
        if(CheckFrontWall()) return;
        Tile tile = LevelController.Instance.CanPlayerTravelToForwardTIle();
        if (tile != null)
        {
            StartCoroutine(DoMovement(tile));
        }
        else return;
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


    private void Rotate(float side) // -1 left, 1 right
    {
        StartCoroutine(DoRotate(side));
    }

    private IEnumerator DoRotate(float side)
    {


        // Rotation initiale du transform
        Quaternion startRotation = transform.rotation;

        // Rotation cible
        Quaternion targetRotation = Quaternion.AngleAxis(90f * side, Vector3.up) * startRotation;


        // Temps écoulé depuis le début de la rotation
        float time = 0f;

        while (time < v_RotationDuration)
        {
            // Calcul de l'interpolation de rotation
            float t = time / v_RotationDuration;
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

    private void OnTriggerEnter(Collider other) {
        if (other.tag == "BlocUpgrade")
        {
            // Donner le power up
            if(other.gameObject.GetComponent<Upgrade>().GetUpgradeType() == UpgradeType.VitesseUp)
            {
                v_MoveSpeedUp = true;
                d_TickRate = 0.5f;
            }
            else if(other.gameObject.GetComponent<Upgrade>().GetUpgradeType() == UpgradeType.ExtraShot)
            {
                LevelController.Instance._combatSystem._amelioration.hasBadAimAnnulator = true;
            }
            else if(other.gameObject.GetComponent<Upgrade>().GetUpgradeType() == UpgradeType.SlowBlackCombat)
            {
                LevelController.Instance._combatSystem._amelioration.hasTimerBonus = true;

            }
            Destroy(other.gameObject);
        }
    }
}
