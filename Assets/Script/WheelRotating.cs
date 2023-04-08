using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class WheelRotating : MonoBehaviour
{
    public float rotationSpeed = 30f;
    public float rotationAmount = 45f;

    private bool rotating = false;
    private Quaternion targetRotation;
    private float totalRotation = 0f;

    private char nextRotation;
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
        if (!rotating)
        {
            if (Input.GetKeyDown(KeyCode.A) && !inputProcessed)
            {
                targetRotation = transform.rotation * Quaternion.Euler(0f, 0f, rotationAmount);
                rotating = true;
                totalRotation += rotationAmount;
                inputProcessed = true;
            }
            if (Input.GetKeyDown(KeyCode.D) && !inputProcessed)
            {
                targetRotation = transform.rotation * Quaternion.Euler(0f, 0f, -rotationAmount);
                rotating = true;
                totalRotation -= rotationAmount;
                inputProcessed = true;
            }
        }
        else if (rotating)
        {
            if (Input.GetKeyDown(KeyCode.A) && !inputProcessed)
            {
                nextRotation = 'a';
                inputProcessed = true;
            }
            if (Input.GetKeyDown(KeyCode.D) && !inputProcessed)
            {
                nextRotation = 'd';
                inputProcessed = true;
            }

            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            if (Quaternion.Angle(transform.rotation, targetRotation) < 0.1f)
            {
                rotating = false;
                
                if (totalRotation >= 360f || totalRotation <= -360f)
                {
                    transform.rotation = Quaternion.identity;
                    totalRotation = 0f;
                }

/*                switch (Mathf.Round(totalRotation))
                {
                    case 0f:
                        feelingText.text = "Caring";
                        break;
                    case 45f:
                        feelingText.text = "Respected";
                        break;
                    case 90f:
                        feelingText.text = "Hopeful";
                        break;
                    case 135:
                        feelingText.text = "Affectionate";
                        break;
                    case 180:
                        feelingText.text = "Excluded";
                        break;
                    case 225:
                        feelingText.text = "Annoyed";
                        break;
                    case 270:
                        feelingText.text = "Powerless";
                        break;
                    case 315:
                        feelingText.text = "Lonely";
                        break;
                    default:
                        Debug.Log("error feeling");
                        break;
                }
                */
                if (nextRotation != '0')
                {
                    rotating = true;
                    if (nextRotation == 'a')
                    {
                        targetRotation = transform.rotation * Quaternion.Euler(0f, 0f, rotationAmount);
                        rotating = true;
                        totalRotation += rotationAmount;
                    }
                    if (nextRotation == 'd')
                    {
                        targetRotation = transform.rotation * Quaternion.Euler(0f, 0f, -rotationAmount);
                        rotating = true;
                        totalRotation -= rotationAmount;
                    }
                    nextRotation = '0';
                }
            }
        }

        if (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
        {
            inputProcessed = false;
        }
    }
    public string GetActualColor()
    {
        if (totalRotation >= 360 || totalRotation <= -360)
        {
            totalRotation = 0;
        }
        if (totalRotation >= 0 )
            return _currentColor = _pairsCouleurDegPos[totalRotation];
        else
           return _currentColor = _pairsCouleurDegNeg[totalRotation];

        
    }
    
}



/*
// Update is called once per frame
void Update()
{

    if (Input.GetKeyDown(KeyCode.A))
    {
        direction = 1;
        isRotating = true;
    }

    if (Input.GetKeyDown(KeyCode.D))
    {
        direction = -1;
        isRotating = true;
    }
    if (isRotating)
    {
        float angle = direction * speed * Time.deltaTime;

        transform.Rotate(Vector3.forward, angle);
        angleRotated += Mathf.Abs(angle);

        if (angleRotated >= 45.0f)
        {
            isRotating = false;
            angleRotated = 0.0f;
        }
    }
}*/