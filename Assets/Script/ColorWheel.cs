using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WheelRotating : MonoBehaviour
{
    public float rotationSpeed = 30f;
    public float rotationAmount = 45f;

    private bool rotating = false;
    private Quaternion targetRotation;
    private float totalRotation = 0f;

    private char nextRotation;
    private bool inputProcessed = false;


    void Start()
    {

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