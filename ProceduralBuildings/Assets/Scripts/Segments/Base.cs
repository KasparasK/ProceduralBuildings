﻿using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
public class Base : Segment
{
    private const string name = "base";
    public List<Window> windows;
    public Door door;
    public Base(ref BaseParams baseParams, Transform parent, Material material,BuildingParams buildingParams, BaseParams lastBaseParams = null)
    {
        Vector3Int baseObjSize = baseParams.baseObjSize;

        GenerateBaseCube(material, baseParams.baseObjSize, name);

        obj.transform.parent = parent;

        AlterMesh(ref baseParams, baseObjSize, buildingParams, lastBaseParams);
    }

    void AlterMesh(ref BaseParams baseParams, Vector3Int baseObjSize, BuildingParams buildingParams, BaseParams lastBaseParams = null)
    {
        Mesh mesh = obj.GetComponent<MeshFilter>().sharedMesh;
        Vector3[] vertices = mesh.vertices;

        AlterCubeSize(baseParams.finalSize, baseParams.baseObjSize, ref vertices);

        if (buildingParams.generateCornerPillars)
        {
            AddSidePilars(
                ref vertices,
                baseParams.backFirewall,
                baseParams.leftFirewall,
                baseParams.rightFirewall,
                baseParams.sideDecorWidth,
                baseParams.sideDecorDepth,
                ref baseParams.addedDecorWidth,
                baseParams.finalSize,
                baseObjSize,
                lastBaseParams);
        }

        mesh.vertices = vertices;

        if (baseParams.floorNum == 0)
        {
            int removeFrom = CalculateRingSize(baseObjSize) * (baseObjSize.y + 1) +
                             ((baseObjSize.x + 1) * (baseObjSize.z + 1));
            int removeTo = removeFrom + ((baseObjSize.x + 1) * (baseObjSize.z + 1)) - 1;
            RemoveVerticesAndTriangles(removeFrom, removeTo);
        }

        mesh = obj.GetComponent<MeshFilter>().sharedMesh;
        vertices = mesh.vertices;

        if (buildingParams.generateCornerPillars)
            obj.GetComponent<MeshFilter>().sharedMesh.uv = GenerateUVsWPillars(vertices.Length, baseObjSize,
                baseParams.wallsColor,
                baseParams.pillarsColor);
        else
            obj.GetComponent<MeshFilter>().sharedMesh.uv = GenerateUVs(vertices.Length, baseParams.wallsColor);

        obj.transform.localPosition = baseParams.finalPos;
    }

    protected Vector2[] GenerateUVsWPillars(int verticesLength, Vector3Int baseObjSize, Vector2Int _wallsColor,
        Vector2Int _pillarsColor)
    {
        Vector2 wallsColor = GetColorPosition(_wallsColor);
        Vector2 pillarsColor = GetColorPosition(_pillarsColor);
        bool pillarColor = false;
        Vector2[] uvs = new Vector2[verticesLength];
        uvs[0] = pillarsColor;
        uvs[1] = pillarsColor;
        int increment = 2;
        int i = 2;
        int ring = CalculateRingSize(baseObjSize);
        while (i < ring * (baseObjSize.y + 1))
        {
            for (int j = 0; j < increment; j++)
            {
                // Debug.Log(i+j);
                if (i + j >= verticesLength)
                    break;

                uvs[i + j] = pillarColor ? pillarsColor : wallsColor;
            }

            i += increment;
            increment = !pillarColor ? 4 : 2;
            pillarColor = !pillarColor;
        }

        return uvs;
    }

    public void GenerateWindows(BaseParams baseParams, Material material)
    {
        windows = new List<Window>();

        for (int j = 0; j < baseParams.windowParams.Count; j++)
        {
            windows.Add(new Window(obj.transform, material, baseParams.windowParams[j]));
        }
    }

    public void GenerateDoor(DoorParams doorParams, Material material)
    {
        door = new Door(obj.transform, material, doorParams);
    }
}
