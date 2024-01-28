using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public float jumpHeight = 10f;
    public bool usingController = false; 


    void Start()
    {
        usingController = isControllerConnected();
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

            if (usingController)
            {
                float horizontalInput = Input.GetAxis("Horizontal");
                if (horizontalInput > 0)
                {
                    transform.right = Vector2.right;
                }
                else if (horizontalInput < 0)
                {
                    transform.right = Vector2.left;
                }
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    transform.right = Vector2.left;
                }
                if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    transform.right = Vector2.right;

                }
            }
        }

    }

    bool touchingPlatform() 
    {
        CapsuleCollider2D capsuleCollider = GetComponent<CapsuleCollider2D>();
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

