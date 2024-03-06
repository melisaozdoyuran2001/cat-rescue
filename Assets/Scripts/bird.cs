using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bird : MonoBehaviour
{
    public float speed = 2f;
    public float rightBoundX = 12f;
    public float leftBoundX = -12f;
    public float upBoundY = 44f;
    public float lowBoundY = 0f;
    private bool movingRight = true;
    private bool movingUp = true;
    private float lastX;
    private float lastY;
    private Rigidbody2D rb;
    private float updateCounter;
    public bool isTriggered = false;
    private EdgeCollider2D trigger;
    public AudioClip wingFlap;
    AudioSource audioSource;

    // Start and Update methods...

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        trigger = GetComponent<EdgeCollider2D>();
        lastX = transform.position.x;
        lastY = transform.position.y;
        audioSource = GetComponent<AudioSource>();
    }

     void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Character")) 
        {
            isTriggered = true; 
        }
        trigger.enabled = false;
        audioSource.PlayOneShot(wingFlap, 1);
    }

    void Update()
    {
        if (!isTriggered) return;
        updateCounter++;
        if (lastX == transform.position.x && updateCounter > 10)
        {
            if (movingRight)
            {
                transform.position = new Vector2(rb.position.x + 5f, rb.position.y + 5f);
            }
            else
            {
                transform.position = new Vector2(rb.position.x - 5f, rb.position.y + 5f);

            }
        }
        //if(lastY == transform.position.y)
        //{
        //    rb.position += Vector2.up * 5f;
        //
        MoveBird();
        if(updateCounter > 10)
        {
            updateCounter = 0;
            lastX = transform.position.x;
            lastY = transform.position.y;
        }
    }

    void MoveBird()
    {
        float xMovement = movingRight ? speed : -speed;
        float yMovement = movingUp ? speed : -speed;
        rb.velocity = new Vector2(xMovement, yMovement);
        if (transform.position.x > rightBoundX)
        {
            movingRight = false;
        }
        if (transform.position.x < leftBoundX)
        {
            movingRight = true;
        }
        if (transform.position.y > upBoundY)
        {
            movingUp = false;
        }
        if (transform.position.y < lowBoundY)
        {
            movingUp = true;
        }
    }
}
