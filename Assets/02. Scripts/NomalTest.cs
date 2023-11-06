using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NomalTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        MeshRenderer myMaterial = GetComponent<MeshRenderer>();
        myMaterial.material.SetFloat("_BumpScale", -1f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
