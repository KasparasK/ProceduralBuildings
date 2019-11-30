using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratorController : MonoBehaviour
{
    public string path;

    public Material baseMaterial;
    public Material windowMaterial;
    public Material glassMaterial;
    public Material doorMaterial;
    public Material roofMaterial;
    public Material segmentsMaterial;

    public bool leftFirewall, rightFirewall, backFirewall;

    public GameObject parentObj;

    public VertexVisualiser VertexVisualiser;

    public void Generate()
    {
       

        if(parentObj != null)
            DestroyImmediate(parentObj);

        parentObj = new GameObject();
        parentObj.transform.position= Vector3.zero;
        parentObj.name = "Building";

        Building building = new Building(5,this, VertexVisualiser);

    }



}
