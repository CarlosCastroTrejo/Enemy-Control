using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class MonteCarlo : MonoBehaviour {

    // Public variables

    public NavMeshAgent agent; // NavMesh agent so enemies can have AI
    public Collider[] points; // Array of colliders that we obtain with the OverlapSphere
    public float[] totalDistances; //Array of floats of the  distances between the enemy-waypoint-player
    public GameObject player; // Gameobject player
    public GameObject nextPoint; // GameObject next point to travel
    public int radio; // Radio of the OverslapSphere
    public Color color; // Color of the debug



    // getDistances: Get the total distances from the enemy to the player passing through the specific waypoint
    // Input: Array of Colliders representing the object near the enemy
    // Output: Array of floats representing the distances

    float[] getDistances(Collider[] points) 
    {
        float[] distancias = new float[points.Length];
        for(int x = 0; x < distancias.Length; x++) 
        {
            distancias[x] = Vector3.Distance(this.transform.position, points[x].transform.position)+ Vector3.Distance(points[x].transform.position,player.transform.position);
        }
        return distancias;
    }


    // setDistance: Get the objects around the enemies, applies the monte carlo algorithm and assing the navMesh agent a destination
    // Input: 
    // Output: 

    void setDistance()
    {
        points = Physics.OverlapSphere(this.transform.position,radio,1);
        if (points.Length == 0)
        {
            radio++;
            setDistance();
        }
        else
        {
            totalDistances = getDistances(points);
            int n = totalDistances.Length;
            float aux = 0;
            float smaller = 1000000;
            int smallerIndex = 0;


            // MonteCarlo Algorithm
            for (int i = 0; i < n / 2; i++)
            {
                int x = Random.Range(0, n - 1);
                aux = totalDistances[x];
                if (aux < smaller)
                {
                    smaller = aux;
                    smallerIndex = x;
                }
            }


            Debug.Log(points.Length + " ----- " + smallerIndex);
            nextPoint = points[smallerIndex].transform.gameObject;
            this.agent.SetDestination(nextPoint.transform.position);
        }
    }


    void Start () 
    {
        agent = this.GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
        Random.InitState(System.DateTime.Now.Millisecond);
        setDistance();
		
	}
	
	// Update is called once per frame
	void Update () 
    {
        // Debug in order to see the Sphere of the OverlapSphere
        DebugExtension.DebugWireSphere(this.transform.position,color,radio);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Ganado");
        }
        else if (other.gameObject == nextPoint)
        {
            nextPoint.layer = 1;
            setDistance();
            Debug.Log("Llegado");
        }
    }

  
}
