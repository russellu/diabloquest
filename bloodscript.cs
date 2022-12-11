using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bloodscript : MonoBehaviour
{
    float totalTime = 0f; 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        totalTime += Time.deltaTime;
        if (totalTime > 1)
            Destroy(this); 
        
    }
}
