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
    public float jump_buffer = 0f;
    private bool isGrappling = false;
    private Vector2 grapplePoint;
    private bool isJumping = false;
    private bool isBoostActive = false;
    private bool isBoost2= false;
    public Sprite normalSprite;
    public Sprite clingingSprite;
    private SpriteRenderer spriteRenderer;


    void Start()
    {
        usingController = isControllerConnected();
        RigidBody = GetComponent<Rigidbody2D>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = normalSprite;
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

        // Stop grappling if the character reaches the grapple point
        if ((Vector2)transform.position == grapplePoint)
        {
            isGrappling = false;
        }
        checkMovementInput();
        jump_buffer -= Time.deltaTime;
        isJumping = false;
    }

 
public void DetachHook()
{   
    if (curHook != null)
    {   
     float tolerance = 2.7f; // Adjust the tolerance value as needed
  if (Vector2.Distance(transform.position, grapplePoint) <= tolerance)
{
    isBoost2 = false;
}
else
{
    isBoost2 = true;
}
        
  
        // Get the current position of the hook for direction calculation
        Vector2 hookPosition = curHook.transform.position;
        // Calculate the unit direction from the character to the hook
        Vector2 toHookDirection = (hookPosition - (Vector2)transform.position).normalized;

        // Reference to the Hook script to access any required data before destruction
        Hook hookScript = curHook.GetComponent<Hook>();

        // Cleanup: Destroy the hook and remove visuals
        hookScript.DestroyRope();
        curHook = null;

        // Destroy the hinge joint to fully detach the character from the grapple
        if (swingJoint != null)
        {
            Destroy(swingJoint);
            swingJoint = null;
        }

       if (isGrappling)
{   
    isGrappling = false;
    Rigidbody2D rb = GetComponent<Rigidbody2D>();
    // Determine the boost magnitude
    float boostMagnitude = 7.45f; // Adjust based on gameplay needs
    // Apply a boost in the direction of the hook
    rb.AddForce(toHookDirection * boostMagnitude, ForceMode2D.Impulse);
     
   
    
    
}

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
        bool isSwinging = curHook != null; // Assuming this means the character is currently swinging
        Vector2 movementForce = Vector2.zero;
        //essentially prevents flying-like movement
        
        

        if (Input.GetAxis("Horizontal") == 0 && !isBoostActive && !isSwinging && !isBoost2)
        {
            RigidBody.velocity = new Vector2(0, RigidBody.velocity.y);
        }
        else if (Input.GetAxis("Horizontal") > 0 && !isBoostActive && !isSwinging && !isBoost2)
        {
            if (RigidBody.position.x < cameraMaxX)
            {
                transform.right = Vector2.right;
                RigidBody.velocity = new Vector2(transform.right.x * 4, RigidBody.velocity.y);
            }
        }
        else if (Input.GetAxis("Horizontal") < 0 && !isBoostActive && !isSwinging && !isBoost2)
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
        if(isSwinging)
        {
            isBoost2 = false;
            float desiredSpeed = Input.GetAxis("Horizontal") * 25;

    // Calculate the difference from the current speed
           float speedDifference = desiredSpeed - RigidBody.velocity.x;

    // Apply force based on the speed difference and Rigidbody's mass
    // Ensure there's a minimum force applied for movement
           float force = Mathf.Clamp(speedDifference * RigidBody.mass, -50, 50);

    // Apply the force
           RigidBody.AddForce(new Vector2(force, 0), ForceMode2D.Force);
          
    

        }

     
        // Check for grapple input
        if (!endGame && Input.GetMouseButtonDown(0))
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction = (mousePosition - (Vector2)transform.position).normalized;
            // Offset the ray start position slightly in the direction of the ray to avoid hitting character's collider
            float colliderAvoidanceOffset = 1.4f; // Adjust this value as needed
            Vector2 rayStart = (Vector2)transform.position + direction * colliderAvoidanceOffset;
            Debug.DrawRay(rayStart, direction * 3000f, Color.red, 5f);
            RaycastHit2D hit = Physics2D.Raycast(rayStart, direction, 9f);


            if (hit.collider != null && (hit.collider.gameObject.CompareTag("Branch") || (hit.collider.gameObject.CompareTag("Leaf"))))
            {
                Swing(hit.point);
            }

        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!isJumping)
            {
                isJumping = true;
                Jump();
                DetachHook(); // Call to detach the hook when jumping
            }
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
                //isGrappling = false;
                //DetachHook();
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
        if(isGrappling)
        {
        DetachHook();    
        isGrappling = false;
        

        }
        else if(!isGrappling && curHook != null)
        {
        DetachHook();
        }
        else
        {

         if (touchingPlatform() || jump_buffer > 0)
        {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        float jumpAngle = 80f;
        
        if(transform.right == Vector3.left)
        {
            jumpAngle = 100f;

        }
        float jumpRadians = jumpAngle * Mathf.Deg2Rad;
        Vector3 jumpDirection = new Vector2(Mathf.Cos(jumpRadians), Mathf.Sin(jumpRadians));
        //rb.AddForce(jumpDirection.normalized * jumpHeight, ForceMode2D.Impulse);
        rb.velocity = new Vector3(rb.velocity.x, 10f);
        
        }

        }


        if (swingJoint != null)
    {
        Destroy(swingJoint);
        swingJoint = null;
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
        isJumping = false;
        spriteRenderer.sprite = normalSprite;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag != "Hook")
        {
            Debug.Log("collision for move");
            isBoostActive = false;
            isBoost2 = false;
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
        }

        if (collision.contacts[0].normal.x != 0 && capsuleCollider.bounds.min.y < collision.GetContact(0).point.y)
        {
            float halfwayY = collision.collider.bounds.center.y + collision.collider.bounds.extents.y;
            if (transform.position.y < (halfwayY * 1.05))
            {
                spriteRenderer.sprite = clingingSprite;
            }
        }
    }

} 
 

