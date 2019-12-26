using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GeneratorController : MonoBehaviour
{
    public string path;

    public Material material;

    public bool leftFirewall, rightFirewall, backFirewall;
    public bool rowSameLit;

    public bool sameSizeFloors;
    public GameObject parentObj;

    public VertexVisualiser VertexVisualiser;

    public bool useCustomBuildingSize;
        [Range(2,10)]
    public float customBuildingSizeX;
    [Range(2, 10)]

    public float customBuildingSizeZ;

    public int minStoriesCount, maxStoriesCount;

    private double startTime;
    private double endTime;
    public void Generate()
    {
        startTime = EditorApplication.timeSinceStartup;


        if (parentObj != null)
            DestroyImmediate(parentObj);

        parentObj = new GameObject();
        parentObj.transform.position= Vector3.zero;
        parentObj.name = "Building";

        BuildingParams buildingParams = new BuildingParams(
            leftFirewall,
            rightFirewall, 
            backFirewall,
            useCustomBuildingSize,
            customBuildingSizeX,
            customBuildingSizeZ,
            rowSameLit,
            sameSizeFloors,
            minStoriesCount,
            maxStoriesCount);

        Building building = new Building(buildingParams, material, parentObj.transform, VertexVisualiser);
        endTime = EditorApplication.timeSinceStartup;
        Debug.Log("Generation finished. Duration: " + (endTime - startTime) * 1000 + " ms");

    }


    public void GenerationTest()
    {
        double totalTime = 0;
        int retryCount = 200;

        BuildingParams buildingParams = new BuildingParams(
            leftFirewall,
            rightFirewall,
            backFirewall,
            useCustomBuildingSize,
            customBuildingSizeX,
            customBuildingSizeZ,
            rowSameLit,
            sameSizeFloors,
            minStoriesCount,
            maxStoriesCount);

        for (int i = 0; i < retryCount; i++)
        {
            startTime = EditorApplication.timeSinceStartup;

            if (parentObj != null)
                DestroyImmediate(parentObj);

            parentObj = new GameObject();
            parentObj.transform.position = Vector3.zero;
            parentObj.name = "Building";

            Building building = new Building(buildingParams, material, parentObj.transform, VertexVisualiser);
            endTime = EditorApplication.timeSinceStartup;

            totalTime += (endTime - startTime);
        }

        Debug.Log("Generation test finished.Average duration: " + totalTime/ retryCount * 1000 + " ms");

    }



}
