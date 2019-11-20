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

    public GameObject canvas;
    public GameObject canvasPref;
    public GameObject vertexNuPref;

    private GameObject cube;

    public int x, y, z;
    public void Generate()
    {
        if(canvas != null)
            DestroyImmediate(canvas);
        canvas = Instantiate(canvasPref, Vector3.zero, Quaternion.identity);

        if(cube != null)
            DestroyImmediate(cube);

        MeshGenerator meshGenerator = new MeshGenerator();

        cube = meshGenerator.GenerateRectangle(baseMaterial, x, y, z,vertexNuPref,canvas);
    }
}
