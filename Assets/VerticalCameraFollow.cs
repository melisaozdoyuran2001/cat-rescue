using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class VerticalCameraFollow : MonoBehaviour
{
    public Transform player; 
    public float yOffset = 0f; 

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            transform.position = new Vector3(transform.position.x, player.position.y + yOffset, transform.position.z);
        }
    }
}