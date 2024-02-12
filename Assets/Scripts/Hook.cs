using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hook : MonoBehaviour
{  public float velocity = 1f;
   public Vector2 dest;
   public float distance_node = 2;
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
        transform.position = Vector2.MoveTowards(transform.position,dest, velocity);
        // check if the hook is traveling or not
        if((Vector2)transform.position != dest ) // if the position of the hook is different than the destination, then it is traveling
        { // instantiate nodes to control the rope. Set a specified distance

        if(Vector2.Distance(player.transform.position,finalNode.transform.position) > distance_node && !t)
        {
            DoNode();
        }
        else if (t == false)
        {
            t = true;
            finalNode.GetComponent<HingeJoint2D>().connectedBody = player.GetComponent<Rigidbody2D>();

        }
        UpdateRopeVisual();

    

        }

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
        void UpdateRopeVisual()
    {
        lineRenderer.positionCount = nodes.Count + 2; // +2 for the hook and player positions
        lineRenderer.SetPosition(0, transform.position); // Set the hook's position as the first point

        for (int i = 0; i < nodes.Count; i++)
        {
            lineRenderer.SetPosition(i + 1, nodes[i].transform.position); // Set each node's position
        }

        lineRenderer.SetPosition(nodes.Count + 1, player.transform.position); // Set the player's position as the last point
    }
    void ConfigureLineRenderer() {
        // Basic LineRenderer setup
        lineRenderer.startWidth = 0.05f;
        lineRenderer.endWidth = 0.05f;
        // Add more configuration as needed (materials, colors, etc.)
    }

}

