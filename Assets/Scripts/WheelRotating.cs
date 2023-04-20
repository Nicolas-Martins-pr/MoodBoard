using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class WheelRotating : MonoBehaviour
{

    private float v_duration = 0.2f;

    private bool rotating = false;
    private Quaternion targetRotation;
    private float totalRotation = 0f;

    private bool nextRotation;
    private bool inputProcessed = false;

    private string _currentColor;

    private Dictionary<float, string> _pairsCouleurDegPos = new Dictionary<float, string>();
    private Dictionary<float, string> _pairsCouleurDegNeg = new Dictionary<float, string>();


    public void Start()
    {
        _pairsCouleurDegPos.Add(0f, "Rose");
        _pairsCouleurDegPos.Add(45f, "Rouge");
        _pairsCouleurDegPos.Add(90f, "Orange");
        _pairsCouleurDegPos.Add(135f, "Jaune");
        _pairsCouleurDegPos.Add(180f, "Vert");
        _pairsCouleurDegPos.Add(225f, "BleuC");
        _pairsCouleurDegPos.Add(270f, "BleuF");
        _pairsCouleurDegPos.Add(315f, "Violet");

        _pairsCouleurDegNeg.Add(0f, "Rose");
        _pairsCouleurDegNeg.Add(-45f, "Violet");
        _pairsCouleurDegNeg.Add(-90f, "BleuF");
        _pairsCouleurDegNeg.Add(-135f, "BleuC");
        _pairsCouleurDegNeg.Add(-180f, "Vert");
        _pairsCouleurDegNeg.Add(-225f, "Jaune");
        _pairsCouleurDegNeg.Add(-270f, "Orange");
        _pairsCouleurDegNeg.Add(-315f, "Rouge");
        
        _currentColor = "Rose";


    }

    void Update()
{
    if(LevelController.Instance.v_isInCombat)
    {
        CheckInput();
        if(!rotating && inputProcessed)
        {
            Move();
        }
       
    }
}

    private void CheckInput()
    {
        if (Input.GetKey(KeyCode.A)  && !inputProcessed)
            {

                nextRotation = true;
                inputProcessed = true;
            }
        else if (Input.GetKey(KeyCode.D) && !inputProcessed)
        {

                nextRotation =false;
                inputProcessed = true;
        }

    }


    private void Move()// Applique le bon sens de rotation : True == left and false == right
    {

        if(nextRotation)
        {

            StartCoroutine(DoMovement(true));
        }
        else 
        {

            StartCoroutine(DoMovement(false));
        }
        rotating =true;
    }
    private IEnumerator DoMovement(bool side)
    {
        if(side){
            targetRotation = transform.rotation * Quaternion.Euler(0f, 0f, -45f);
            totalRotation -= 45;
        }
        else{
            targetRotation = transform.rotation * Quaternion.Euler(0f, 0f, 45f);
            totalRotation +=45;
        }
            
        float elapsedTime = 0.0f;
        Quaternion startRotation = transform.rotation;
        while (elapsedTime < v_duration)
        {
            // Calculer le taux d'avancement en fonction du temps écoulé
            float t = elapsedTime / v_duration;

            // Interpoller entre la rotation de départ et la rotation cible
            Quaternion newRotation = Quaternion.Lerp(startRotation, targetRotation, t);

            // Appliquer la nouvelle rotation à l'objet
            transform.rotation = newRotation;

            // Attendre la prochaine frame
            yield return null;

            // Mettre à jour le temps écoulé
            elapsedTime += Time.deltaTime;
        }

        // Assurer que la rotation finale soit exactement la rotation cible
        transform.rotation = targetRotation;
        inputProcessed = false;
        rotating = false;

    }

    public string GetActualColor()
    {
        totalRotation = totalRotation%360;
        if (totalRotation >= 360 || totalRotation <= -360)
        {
            totalRotation = 0;
        }
        if (totalRotation >= 0 ){
            return _currentColor = _pairsCouleurDegPos[totalRotation];
        }
            
        else
           return _currentColor = _pairsCouleurDegNeg[totalRotation];

        
    }
    
}
