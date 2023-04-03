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

    }

    private void Move(Tile tile)
    {
        // Lui donne la tile vers laquelle il se dirige et se set en fils
        // Move forward obligatoire
    }

    private void Rotate(float angle) // -1 pour -90 / +1 pour 90 
    {
        // Rotate l'ennemi dans la direction définis
    }

    private IEnumerator DoMovement()
    {
        //While tant que pas arrivé à la position.
        //SetParent quand arrivé
        yield return null; 
    }
}
