using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ColorWheel : MonoBehaviour
{
    float timeCounter = 0;
    bool isRotatingLeft = false;
    bool isRotatingRight = false;
    float speed;

    int numRotating = 0;


    // Start is called before the first frame update
    void Start()
    {
        speed = 150;        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A) && !isRotatingRight)
        {
            isRotatingLeft = true;
        }
        if (isRotatingLeft && !isRotatingRight) { 
            rotation('L');
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            isRotatingRight = true;
        }
        if (isRotatingRight && !isRotatingLeft)
        {
            rotation('R');
        }



    }

        void rotation(char r)
    {
        Debug.Log(timeCounter);
        if (r == 'L')
        {
            timeCounter += (Time.deltaTime * speed);
        }
        else
        {
            timeCounter += (Time.deltaTime * -speed);
        }
        
        if (numRotating >= 45)
        {

            
            isRotatingLeft = false;
            isRotatingRight = false;
            timeCounter = ((int)Math.Round(timeCounter) % 360);
            numRotating = 0;
        }
        else
        {
            numRotating = (int)Math.Round(Math.Abs(timeCounter) % 45);
            
        }
        transform.rotation = Quaternion.Euler(0, 0, timeCounter);
        
    }









/*    void rotationLeft()
    {
        timeCounter += (Time.deltaTime * speed);
        if (numRotating >= 45)
        {

            Debug.Log(timeCounter);
            isRotatingLeft = false;
            timeCounter = ((int)Math.Round(timeCounter) % 360);
            numRotating = 0;
        }
        else
        {
            numRotating = (int)Math.Round(timeCounter % 45);
            
        }
        transform.rotation = Quaternion.Euler(0, 0, timeCounter);
        
    }

    void rotationRight()
    {
        timeCounter += (Time.deltaTime * -speed);
        if (numRotating >= 45)
        {

            Debug.Log(timeCounter);
            isRotatingRight = false;
            timeCounter = ((int)Math.Round(timeCounter) % 360);
            numRotating = 0;
        }
        else
        {
            numRotating = (int)Math.Round(Math.Abs(timeCounter) % 45);

        }
        Debug.Log(-timeCounter);
        transform.rotation = Quaternion.Euler(0, 0, timeCounter);

    }*/

}
