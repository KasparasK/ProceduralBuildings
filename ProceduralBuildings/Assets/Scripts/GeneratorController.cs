using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GeneratorController : MonoBehaviour
{
    public string path;

    public Material mainMaterial;

    public bool leftFirewall, rightFirewall, backFirewall;

    public GameObject parentObj;

    public VertexVisualiser VertexVisualiser;

    

    public int minStoriesCount, maxStoriesCount;
    private double startTime;
    private double endTime;
    public void Generate()
    {
       

        if(parentObj != null)
            DestroyImmediate(parentObj);

        parentObj = new GameObject();
        parentObj.transform.position= Vector3.zero;
        parentObj.name = "Building";

        Building building = new Building(minStoriesCount, maxStoriesCount, this, VertexVisualiser);

    }


    public void GenerationTest()
    {
        double totalTime = 0;
        int retryCount = 200;
        for (int i = 0; i < retryCount; i++)
        {
            startTime = EditorApplication.timeSinceStartup;

            if (parentObj != null)
                DestroyImmediate(parentObj);

            parentObj = new GameObject();
            parentObj.transform.position = Vector3.zero;
            parentObj.name = "Building";

            Building building = new Building(3, 3, this, VertexVisualiser);
            endTime = EditorApplication.timeSinceStartup;

            totalTime += (endTime - startTime);
        }

        Debug.Log("Generation test finished.Average duration: " + totalTime/ retryCount * 1000 + " ms");

    }



}
