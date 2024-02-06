using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public float jumpHeight = 10f;
    public bool usingController = false;
    public Rigidbody2D RigidBody;
    public CapsuleCollider2D capsuleCollider;
    public float moveSpeed = 10f;
    public double cameraMinX = -9.5;
    public double cameraMaxX = 9.5;


    void Start()
    {
        usingController = isControllerConnected();
        RigidBody = GetComponent<Rigidbody2D>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
    }


    void Update()
    {
        checkMovementInput();
    }

    void checkMovementInput()
    { 
        //can only move if touching another object
        //essentially prevents flying-like movement
        if (touchingPlatform())
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Jump();
            }
        }
        if (usingController)
        {
            float horizontalInput = Input.GetAxis("Horizontal");
            if (horizontalInput != 0)
            {
                double newPosX = transform.position.x + horizontalInput * moveSpeed * Time.deltaTime;
                newPosX = Math.Clamp(newPosX, cameraMinX, cameraMaxX);

                transform.position = new Vector3((float)newPosX, transform.position.y, transform.position.z);
            }

            RigidBody.velocity = new Vector2(0, RigidBody.velocity.y);
        }
        else
        {
            float horizontalInput = 0;

            if (Input.GetKey(KeyCode.RightArrow))
            {
                horizontalInput = 1;
            }
            else if (Input.GetKey(KeyCode.LeftArrow))
            {
                horizontalInput = -1;
            }

            if (horizontalInput != 0)
            {
                double newPosX = transform.position.x + horizontalInput * moveSpeed * Time.deltaTime;
                newPosX = Math.Clamp(newPosX, cameraMinX, cameraMaxX);

                transform.position = new Vector3((float)newPosX, transform.position.y, transform.position.z);
            }

            RigidBody.velocity = new Vector2(0, RigidBody.velocity.y);
        }



    }

    bool touchingPlatform() 
    {
        //currently any object grounds the character, when powerups etc are added we can filter by layer 
        return capsuleCollider.IsTouchingLayers(); 
    }

    void Jump()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        float jumpAngle = 80f;
        if(transform.right == Vector3.left)
        {
            jumpAngle = 100f;

        }
        float jumpRadians = jumpAngle * Mathf.Deg2Rad;
        Vector3 jumpDirection = new Vector2(Mathf.Cos(jumpRadians), Mathf.Sin(jumpRadians));
        rb.AddForce(jumpDirection.normalized * jumpHeight, ForceMode2D.Impulse);
    }

    bool isControllerConnected()
    {
        string[] joystickNames = Input.GetJoystickNames();
        foreach (string joystickName in joystickNames)
        {
            if (!string.IsNullOrEmpty(joystickName))
            {
                return true;
            }
        }
        return false;
    }
}

