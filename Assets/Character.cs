using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public float jumpHeight = 10f;
    public bool usingController = false;
    public Rigidbody2D RigidBody;
    public BoxCollider2D boxCollider;
    public float moveSpeed = 10f;
    public double cameraMinX = -9.5;
    public double cameraMaxX = 9.5;


    void Start()
    {
        usingController = isControllerConnected();
        RigidBody = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
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
        
        if (Input.GetAxis("Horizontal") == 0)
            {
                RigidBody.velocity = new Vector2(0, RigidBody.velocity.y);
            }
        else if (Input.GetAxis("Horizontal") > 0)
            {
                if (RigidBody.position.x < 8.3)
                {
                    transform.right = Vector2.right;
                    RigidBody.velocity = new Vector2(transform.right.x * 4, RigidBody.velocity.y);
                }
                else
                {
                    RigidBody.velocity = new Vector2(0, RigidBody.velocity.y);
                }   
            }
        else if (Input.GetAxis("Horizontal") < 0)
            {
                if (RigidBody.position.x > -8.3)
                {
                    transform.right = Vector2.left;
                    RigidBody.velocity = new Vector2(transform.right.x * 4, RigidBody.velocity.y);
                }   
                else
                {
                    RigidBody.velocity = new Vector2(0, RigidBody.velocity.y);
                }   
            }
        else
        {
            GetComponent<Rigidbody>().velocity = new Vector2(0, GetComponent<Rigidbody>().velocity.y);
        }

    }





    bool touchingPlatform() 
    {
        //currently any object grounds the character, when powerups etc are added we can filter by layer 
        return boxCollider.IsTouchingLayers();
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

