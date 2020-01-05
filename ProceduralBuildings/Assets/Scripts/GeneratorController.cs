using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GeneratorController : MonoBehaviour
{
    public string path;

    public Material material;

    public const int maxFloors = 6;
    public const int minFloors = 1;

    public const float maxSizeX = 10;
    public const float maxSizeZ = 10;
    public const float minSizeX = 1;
    public const float minSizeZ = 1;

    [HideInInspector]
    public bool leftFirewall, rightFirewall, backFirewall;

    [HideInInspector]
    public bool onlySquareOpenings, onlyArchedOpenings;
    [HideInInspector]
    public bool rowSameLit;
    [HideInInspector]
    public bool generateCornerPillars;
    [HideInInspector]
    public bool sameSizeFloors;
    [HideInInspector]
    public GameObject parentObj;

    [HideInInspector]
    public bool useCustomBuildingSize;

    [HideInInspector]
    public float customBuildingSizeX, customBuildingSizeZ;

    [HideInInspector]
    public int minStoriesCount, maxStoriesCount;

    public float MaxStoriesCount
    {
        get { return maxStoriesCount; }
        set { maxStoriesCount = (int) value; }
    }

    public float MinStoriesCount
    {
        get { return minStoriesCount; }
        set { minStoriesCount = (int)value; }
    }

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
            maxStoriesCount,
            generateCornerPillars, 
            onlySquareOpenings,
            onlyArchedOpenings);

        Building building = new Building(buildingParams, material, parentObj.transform);
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
            maxStoriesCount,
            generateCornerPillars,
            onlySquareOpenings,
            onlyArchedOpenings);

        for (int i = 0; i < retryCount; i++)
        {
            startTime = EditorApplication.timeSinceStartup;

            if (parentObj != null)
                DestroyImmediate(parentObj);

            parentObj = new GameObject();
            parentObj.transform.position = Vector3.zero;
            parentObj.name = "Building";

            Building building = new Building(buildingParams, material, parentObj.transform);
            endTime = EditorApplication.timeSinceStartup;

            totalTime += (endTime - startTime);
        }

        Debug.Log("Generation test finished.Average duration: " + totalTime/ retryCount * 1000 + " ms");

    }


    public void Merge()
    {
       CombineMeshes combineMeshes = new CombineMeshes();
       combineMeshes.MergeChildren(parentObj);
       
    }
}
