using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
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
        LevelController.Instance.SetEnemyParent(this, newTile);
        yield return null; 
    }

    private void CheckFrontWall()
    {
        //Raycast pour voir si wall en face. Si wall obligé de rotate sinon 25% rotate 75% Move forward
    }
}
