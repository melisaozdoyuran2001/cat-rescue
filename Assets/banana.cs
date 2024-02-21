using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class banana : MonoBehaviour
{
    public Rigidbody2D rb;
    public double cameraMinX = -9.59;
    public double cameraMaxX = 9.59;
    public Vector2 velocity;
    public Vector2 direction;
    void Start()
    {
        //monkey sets direction and velocity
        transform.right = direction;
        rb.velocity = velocity;
    }

    void Update()
    {
        if (rb.position.x < cameraMinX || rb.position.x > cameraMaxX || rb.position.y < 16)
        {
            //self delete
            Destroy(gameObject);
        }
        
    }
}
