using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMaterial : MonoBehaviour
{

    public Texture alt; 

    public Material D_elmmch;

    // Start is called before the first frame update
    void Start()
    {
        Renderer rend = GetComponent<Renderer>();
        // rend.materials[1].EnableKeyword("_MAINTEX");

        //rend.materials[1].SetTexture("_MainTex", alt);
        rend.materials[1].mainTexture = alt; 
        Debug.Log(rend.materials[1].name); 

        //Debug.Log("changing chest"); 
        //D_elmmch.SetTexture("_MainTex",alt); 
            
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
