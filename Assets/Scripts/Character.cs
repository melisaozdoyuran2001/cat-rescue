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
    public double cameraMinX = -9.59;
    public double cameraMaxX = 9.59;
    private HingeJoint2D swingJoint;
    public GameObject hook;
    GameObject curHook;
    private Animator animator;
    public bool endGame = false;
    public float jump_buffer = .15f;
    private bool isGrappling = false;
    private Vector2 grapplePoint;


    void Start()
    {
        usingController = isControllerConnected();
        RigidBody = GetComponent<Rigidbody2D>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        animator = GetComponent<Animator>();
    }


  void Update()
{
    if(RigidBody.position.x > cameraMaxX)
        {
            transform.position = new Vector2(RigidBody.position.x - 0.5f, RigidBody.position.y);

        }
    if(RigidBody.position.x < cameraMinX)
        {
            transform.position = new Vector2(RigidBody.position.x + 0.5f, RigidBody.position.y);
        }


        checkMovementInput();
    jump_buffer -= Time.deltaTime;

  
    

}

public void DetachHook()
{
    if (curHook != null)
    {
        // Perform any cleanup if necessary, like disabling the hinge joint or removing the rope visuals
        // ...

        curHook.GetComponent<Hook>().DestroyRope();; // Destroy the current hook
        isGrappling = false;
        curHook = null; // Nullify the reference to allow new hook instantiation

    }
}
void Swing(Vector2 anchorPoint)
{
    //if (swingJoint == null)
    //{
     //   swingJoint = gameObject.AddComponent<HingeJoint2D>();
      //  swingJoint.autoConfigureConnectedAnchor = false;
      //  swingJoint.connectedAnchor = anchorPoint;
       // swingJoint.enableCollision = true;
   // }
   // else
    //{
     //   swingJoint.connectedAnchor = anchorPoint;
  
    if (curHook == null)
    {   Debug.Log("Instantiating hook.");
        // Instantiate the hook prefab at the character's position
        curHook = Instantiate(hook, transform.position, Quaternion.identity);
        Hook hookScript = curHook.GetComponent<Hook>();

      
        hookScript.dest = anchorPoint;
        hookScript.player = gameObject;
       
        // Optionally, you can also set the line renderer reference here
        // hookScript.lineRenderer = ...;
        grapplePoint = anchorPoint;
    }
}
public void Grapple()
{
    // Start grappling only if the hook is attached
    if (curHook != null)
    {
        isGrappling = true;
    }
}


    void checkMovementInput()
    { 
        //can only move if touching another object
        //essentially prevents flying-like movement
       

        if (Input.GetAxis("Horizontal") == 0)
        {
            RigidBody.velocity = new Vector2(0, RigidBody.velocity.y);
        }
        else if (Input.GetAxis("Horizontal") > 0)
        {
            if (RigidBody.position.x < cameraMaxX)
            {
                transform.right = Vector2.right;
                RigidBody.velocity = new Vector2(transform.right.x * 4, RigidBody.velocity.y);
            }
        }
        else if (Input.GetAxis("Horizontal") < 0)
        {
            if (RigidBody.position.x > cameraMinX)
            {
                transform.right = Vector2.left;
                RigidBody.velocity = new Vector2(transform.right.x * 4, RigidBody.velocity.y);
            }
        }
        else
        {
            //GetComponent<Rigidbody>().velocity = new Vector2(0, GetComponent<Rigidbody>().velocity.y);
        }
        // Check for grapple input
        if (!endGame && Input.GetMouseButtonDown(0))
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction = (mousePosition - (Vector2)transform.position).normalized;
            // Offset the ray start position slightly in the direction of the ray to avoid hitting character's collider
            float colliderAvoidanceOffset = 1f; // Adjust this value as needed
            Vector2 rayStart = (Vector2)transform.position + direction * colliderAvoidanceOffset;
            Debug.DrawRay(rayStart, direction * 3000f, Color.red, 5f);
            RaycastHit2D hit = Physics2D.Raycast(rayStart, direction, 7f);


            if (hit.collider != null && (hit.collider.gameObject.CompareTag("Branch") || (hit.collider.gameObject.CompareTag("Leaf"))))
            {
                Swing(hit.point);
            }

        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
            DetachHook(); // Call to detach the hook when jumping
        }
        if (isGrappling)
        {
            // Move towards the grapple point
            float step = 20f * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, grapplePoint, step);

            // Update the rope visual to follow the character
            if (curHook != null)
            {
                curHook.GetComponent<Hook>().UpdateRopeVisual();
            }

            // Stop grappling if the character reaches the grapple point
            if ((Vector2)transform.position == grapplePoint)
            {
                isGrappling = false;
                DetachHook();
            }
        }
            if (Input.GetKeyDown(KeyCode.G))
    {
        Grapple();
    }
    }

     


    

    bool touchingPlatform() 
    {
        //currently any object grounds the character, when powerups etc are added we can filter by layer 
        return capsuleCollider.IsTouchingLayers(); 
    }

    void Jump()
    {   
         if (touchingPlatform() || jump_buffer > 0)
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
        if (swingJoint != null)
    {
        Destroy(swingJoint);
        swingJoint = null;
    }
        }
        if (isGrappling)
    {
        isGrappling = false;
    }
        
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

    private void OnCollisionExit2D(Collision2D collision)
    {
        jump_buffer = .15f;
    }

} 
 

