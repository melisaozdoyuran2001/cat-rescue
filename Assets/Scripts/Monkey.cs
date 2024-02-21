using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monkey : MonoBehaviour
{
    public GameObject character;
    public GameObject bananaPrefab;
    public double minY = 20;
    public double maxY = 25;
    public int updateInterval = 20;
    void Start()
    {
        
    }
    void Update()
    {
        if (Time.frameCount % updateInterval == 0)
        {
            Vector3 characterPosition = character.transform.position;
            //if character is in banana throwing range
            if (characterPosition.y > minY && characterPosition.y < maxY)
            {
                GameObject newBanana = Instantiate(bananaPrefab) as GameObject;

                // Set the direction and velocity based on the character's position
                banana bananaScript = newBanana.GetComponent<banana>();
                // flesh this out to make targeting more precise
                Vector2 direction = (characterPosition.x > transform.position.x) ? Vector2.right : Vector2.left;

                bananaScript.direction = direction;
                bananaScript.velocity = new Vector2(direction.x * 20, 0); 
            }
        }

    }
}
