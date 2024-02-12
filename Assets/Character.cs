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
    private HingeJoint2D swingJoint;
    public GameObject hook;
    GameObject curHook;
    private Animator animator;
    


    void Start()
    {
        usingController = isControllerConnected();
        RigidBody = GetComponent<Rigidbody2D>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        animator = GetComponent<Animator>();
    }


    void Update()
    {
        checkMovementInput();
        
        //if(Input.GetMouseButtonDown(0))
        //{
        //    Vector2 dest = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //    curHook = (GameObject)Instantiate(hook, transform.position, Quaternion.identity);
            // traveling
        //    curHook.GetComponent<Hook>().dest = dest; // set the transform of the hook to the actual destination determined by the mouse

        //}
        
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
        
        if (Input.GetKey(KeyCode.LeftArrow) && Input.GetKey(KeyCode.RightArrow))
            {
                RigidBody.velocity = new Vector2(0, RigidBody.velocity.y);
            }
        else if (Input.GetAxis("Horizontal") > 0)
            {
                transform.right = Vector2.right;
                RigidBody.velocity = new Vector2(transform.right.x * 4, RigidBody.velocity.y);
                //animator.SetBool("Grounded", true);
            }
        else if (Input.GetAxis("Horizontal") < 0)
            {
                transform.right = Vector2.left;
                RigidBody.velocity = new Vector2(transform.right.x * 4, RigidBody.velocity.y);
            }
        else
            {
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
        //animator.SetBool("IsJumping", true);
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
 

