using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class MonteCarlo : MonoBehaviour {

    public NavMeshAgent agent;
    public Collider[] points;
    public float[] distanciasTotales;
    public GameObject player;
    public GameObject nextDistance;
    public int radio;
    public Color color;



    // Use this for initialization

    float[] getDistances(Collider[] points) 
    {
        float[] distancias = new float[points.Length];
        for(int x = 0; x < distancias.Length; x++) 
        {
            distancias[x] = Vector3.Distance(this.transform.position, points[x].transform.position)+ Vector3.Distance(points[x].transform.position,player.transform.position);
        }
        return distancias;
    }

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
            distanciasTotales = getDistances(points);
            int n = distanciasTotales.Length;
            float aux = 0;
            float menor = 1000000;
            int indiceMenor = 0;

            for (int i = 0; i < n / 2; i++)
            {
                int x = Random.Range(0, n - 1);
                aux = distanciasTotales[x];
                if (aux < menor)
                {
                    menor = aux;
                    indiceMenor = x;
                }
            }


            Debug.Log(points.Length + " ----- " + indiceMenor);
            nextDistance = points[indiceMenor].transform.gameObject;
            this.agent.SetDestination(nextDistance.transform.position);
        }
    }


    void Start () 
    {
        agent = this.GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
        setDistance();
		
	}
	
	// Update is called once per frame
	void Update () 
    {
        DebugExtension.DebugWireSphere(this.transform.position,color,radio);


    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Ganado");
        }
        else if (other.gameObject == nextDistance)
        {
            nextDistance.layer = 1;
            setDistance();
            Debug.Log("Llegado");
        }
    }

  
}
