using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrcMaterials : MonoBehaviour
{

    public Texture cb_orcch0001;
    public Texture cb_orcua0001;
    public Texture cb_orclg0001;
    public Texture cb_orcfa0001;
    public Texture cb_orcft0001;
    public Texture cb_orcft0002;
    public Texture cb_orchn0001;
    public Texture cb_orchn0002;
    public Texture Tm_orclg0002;
    public Texture cb_orche0101;

    public GameObject head; 


    // Start is called before the first frame update
    void Start()
    {
        Renderer rend = GetComponent<Renderer>();
        rend.materials[0].mainTexture = cb_orcch0001;
        rend.materials[1].mainTexture = cb_orcua0001;
        rend.materials[2].mainTexture = cb_orclg0001;
        rend.materials[3].mainTexture = cb_orcfa0001;
        rend.materials[4].mainTexture = cb_orcch0001;
        rend.materials[5].mainTexture = cb_orcft0002;
        rend.materials[6].mainTexture = cb_orchn0001;
        rend.materials[7].mainTexture = cb_orchn0002;
        rend.materials[8].mainTexture = Tm_orclg0002;

        Renderer headRend = head.GetComponent<Renderer>();
        headRend.material.mainTexture = cb_orche0101; 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
