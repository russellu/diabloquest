using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowScript : MonoBehaviour
{
    float elapsed = 0;
    bool released;
    Vector3 releaseForward;
    float arrowSpeed = 150f; 

    // Start is called before the first frame update
    void Start()
    {


    }

    public void InitiateFlight(Vector3 startPosition, Vector3 endPosition)
    {
        released = true;
        releaseForward = -transform.up;
        transform.parent = null; 
    }


    void Update()
    {
  

        if (released)
        {
            elapsed += Time.deltaTime;
            transform.position = transform.position + Time.deltaTime * releaseForward * arrowSpeed; 
            
        }
        if (elapsed > 4)
            Destroy(gameObject); 
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "orc")
        {
            Debug.Log("hit orc");
            other.gameObject.GetComponent<EnemyScript>().TakeDamage(5, transform.position);

        }

    }

}
