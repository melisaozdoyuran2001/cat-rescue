using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hook : MonoBehaviour
{  public float velocity = 1f;
   public Vector2 dest;
   public float distance_node = 0.5f;
   public GameObject nodePrefab;
   public GameObject player;
   public GameObject finalNode;
   bool t = false;
   public LineRenderer lineRenderer; // Reference to the LineRenderer
   private List<GameObject> nodes = new List<GameObject>(); // To keep track of all nodes

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Character");
        finalNode = transform.gameObject; // the last node we created will be connected to the game object!
        lineRenderer = GetComponent<LineRenderer>();
                
        if (lineRenderer == null) { // If there wasn't a LineRenderer already, add one
            lineRenderer = gameObject.AddComponent<LineRenderer>();
        }
      ConfigureLineRenderer();  
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, dest, velocity);

    // If the hook has not reached the destination, continue creating nodes
    if ((Vector2)transform.position != dest)
    {
        if (Vector2.Distance(player.transform.position, finalNode.transform.position) > distance_node && !t)
        {
            DoNode();
        }
    }
    else if (!t)
    {
        // If the hook reached the destination and is not attached, attach it
        t = true;
        AttachHookToPlayer();
    }

    if(t)
    {
       UpdateRopeVisual(); 
    }
    
}

void AttachHookToPlayer()
{
    finalNode.GetComponent<HingeJoint2D>().connectedBody = player.GetComponent<Rigidbody2D>();
}
        
public void DestroyRope()
{
    // Destroy all nodes
    foreach (GameObject node in nodes)
    {
        Destroy(node);
    }
    nodes.Clear(); // Clear the list after destroying the nodes

    // Finally, destroy the hook itself
    Destroy(gameObject);
}

        
    

    void DoNode()
    {
        Vector2 created_position = player.transform.position - finalNode.transform.position;
        created_position.Normalize(); // points towards the player
        created_position = created_position * distance_node; // to achieve node seperation, essentially we are constructing the points that will make the nodes
        created_position = (Vector2)created_position + (Vector2)finalNode.transform.position; // essentially a subtraction
        // now instantiate the Node prefab at the position we computed(created_position)
        GameObject g = (GameObject)Instantiate(nodePrefab, created_position, Quaternion.identity);
        
        // set node actions
        // first connect the last node to the newly created node
        finalNode.GetComponent<HingeJoint2D>().connectedBody = g.GetComponent<Rigidbody2D>();

        finalNode = g;
        nodes.Add(g);




    }
    public void UpdateRopeVisual()
{
    if (t) // Check if the hook is active
    {   Debug.Log("Updating Rope Visual");
        lineRenderer.positionCount = 2; // Only two points needed for a simple line

        // Set the first point of the line at the ray start position
        Vector2 ropeStartPoint = (Vector2)player.transform.position;
        lineRenderer.SetPosition(0, ropeStartPoint);

        // Set the second point of the line at the hook's current position
        lineRenderer.SetPosition(1, transform.position);
    }
    else
    {
        lineRenderer.positionCount = 0; // No points to render when the hook is inactive
    }
}

    
   void ConfigureLineRenderer() {
    // Basic LineRenderer setup
    lineRenderer.startWidth = 0.1f;
    lineRenderer.endWidth = 0.1f;

    // Create a new Material with a shader for the LineRenderer that is visible on both Scene and Game view
    Material lineMaterial = new Material(Shader.Find("Sprites/Default"));

    // Set the color of the LineRenderer to white and fully opaque
    lineMaterial.color = Color.white;
    lineRenderer.material = lineMaterial;

    // Optional: Set sorting layer and order for 2D game
    // lineRenderer.sortingLayerName = "Foreground";
    // lineRenderer.sortingOrder = 5;
}


}

