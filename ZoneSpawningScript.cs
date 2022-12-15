using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System; 
using UnityEngine.AI; 

public class ZoneSpawningScript : MonoBehaviour
{
    // Start is called before the first frame update

    // for known location spawns
    public Transform[] orcSpawnPoints;


    // for random spawns
    public Transform playerLocation;

    public GameObject orcPrefab;

    List<GameObject> orcs;

    public Transform spawnPlane;
    public GameObject spawnObjectIndicator;
    public LayerMask spawnRayMask;

    List<Vector3> orcDests = new List<Vector3>();

    int nOrcs = 25; 

    void Start()
    {
        GenerateSpawnPoints();

        
        orcs = new List<GameObject>();
        for (int i = 0; i < nOrcs; i++)
        {
            GameObject newOrc = Instantiate(orcPrefab);

            // raycast to ground and see if the orc is close enough

            newOrc.name = "orc_" + i; 
            newOrc.GetComponent<NavMeshAgent>().Warp(orcDests[i]); 

            int randInd = (int)UnityEngine.Random.Range(0f, nOrcs-1);

            Debug.Log("orc = " + i);

            NavMeshAgent agent = newOrc.GetComponent<NavMeshAgent>(); 


            //if(agent.isOnNavMesh)
            //    newOrc.GetComponent<NavMeshAgent>().destination = orcDests[nOrcs - i - 1];            

            orcs.Add(newOrc);
        }

      //  ClusterOrcs(); 
    }


    void ClusterOrcs()
    {
        for (int i = 0; i < orcs.Count; i++)
            for (int j = 0; j < orcs.Count; j++)
            {
                if (i != j)
                {
                    if ((orcs[i].transform.position - orcs[j].transform.position).magnitude < 50)
                    {
                        orcs[i].GetComponentInChildren<EnemyScript>().AddLinkedMob(
                            orcs[j].GetComponentInChildren<EnemyScript>());
                    
                    }                     
                }           
            }  
    }


    void GenerateSpawnPoints()
    {

        /*
        // for whole-map spawning
        float xScale = spawnPlane.transform.localScale.x;
        float zScale = spawnPlane.transform.localScale.z;
        float xOffset = spawnPlane.transform.position.x;
        float zOffset = spawnPlane.transform.position.z;
        */

        // for local spawning (close to player only)
        float xScale = 50;
        float zScale = 50;
        float xOffset = playerLocation.position.x;
        float zOffset = playerLocation.position.z;


        float yLoc = spawnPlane.transform.position.y; 

        for (int i = 0; i < nOrcs; i++)
        {
            float randX = UnityEngine.Random.Range(-xScale*5, xScale*5);
            float randZ = UnityEngine.Random.Range(-zScale*5, zScale*5);

            Vector3 randomSpawnPlanePosition = new Vector3(randX, yLoc, randZ) + new Vector3(xOffset, 0, zOffset);
            RaycastHit hitInfo;

            Ray ray = new Ray(randomSpawnPlanePosition, Vector3.down);

            Physics.Raycast(ray, out hitInfo, Mathf.Infinity, spawnRayMask);

            orcDests.Add(hitInfo.point);

        } 


    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
