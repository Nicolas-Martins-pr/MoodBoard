using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("Variables")]
    [SerializeField]
    private float v_MovementDuration = 0.9f;

    [SerializeField]
    private bool v_FrontWall = false;
    private RaycastHit v_FrontWallHit;
    // Start is called before the first frame update
    void Start()
    {
        
    }

     void Update() {
    Move();
    }

    public void ChoseComportement()
    {
        CheckFrontWall();
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
        else return;
        // Lui donne la tile vers laquelle il se dirige et se set en fils
        // Move forward obligatoire

    }

    private void Rotate(float angle) // -1 pour -90 / +1 pour 90 
    {
        // Rotate l'ennemi dans la direction définis
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
        v_FrontWall = Physics.Raycast(transform.position, transform.forward, out v_FrontWallHit, v_WallCheckDistance, LayerMask.GetMask("Wall"));
    }
}
