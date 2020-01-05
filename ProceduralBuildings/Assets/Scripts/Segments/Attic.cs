﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attic : Segment
{
    private const string name = "attic";
    public List<Window> windows;

    public Attic(Material material, AtticParams atticParams,Transform parent)
    {
        Vector3Int baseObjSize = BaseObjSizes.atticSize;

        GenerateBaseCube(material, baseObjSize,name);
        obj.transform.parent = parent;
        AlterMesh(atticParams);
    }

    void AlterMesh(AtticParams atticParams)
    {
        Mesh mesh = obj.GetComponent<MeshFilter>().sharedMesh;
        Vector3[] vertices = mesh.vertices;

        Vector3Int baseObjSize = atticParams.baseObjSize;

        AlterCubeSize(atticParams.finalSize, baseObjSize, ref vertices);
        FormAttic(atticParams.finalSize,baseObjSize, ref vertices);
        mesh.vertices = vertices;

        int removeFrom = CalculateRingSize(baseObjSize) * (baseObjSize.y + 1);
        int removeTo = removeFrom + ((baseObjSize.x + 1) * (baseObjSize.z + 1)) - 1;
        RemoveVerticesAndTriangles(removeFrom, removeTo);
        mesh = obj.GetComponent<MeshFilter>().sharedMesh;
        vertices = mesh.vertices;
        mesh.uv = GenerateUVs(vertices.Length,atticParams.color);

        //  VisualiseVertices(mesh.vertices);
        obj.transform.localPosition = atticParams.finalPos;
    }

    void FormAttic(Vector3 goalSize,Vector3Int baseObjSize, ref Vector3[] vertices)
    {

        int ring = CalculateRingSize(baseObjSize);
        goalSize.x /= -2;

        Vector3 sizeToAdd = Vector3.zero;

        int tempId = 1;
        sizeToAdd.x = goalSize.x;
        for (int i = 0; i < ring/2; i++)
        {
            AlterVertexPosition(ref vertices[tempId], sizeToAdd);
            tempId++;
        }

        sizeToAdd.x *= -1;
        tempId += (ring / 2) - 1;
        AlterVertexPosition(ref vertices[tempId], sizeToAdd);
        tempId++;

        sizeToAdd.x = 0;
        sizeToAdd.y = -goalSize.y;
        AlterVertexPosition(ref vertices[tempId], sizeToAdd);
        tempId++;
        AlterVertexPosition(ref vertices[tempId], sizeToAdd);
        tempId++;
        AlterVertexPosition(ref vertices[tempId], sizeToAdd);
        tempId++;
        AlterVertexPosition(ref vertices[tempId], sizeToAdd);
        tempId++;

        sizeToAdd.y = 0;
        sizeToAdd.x = -goalSize.x;
        AlterVertexPosition(ref vertices[tempId], sizeToAdd);
        tempId++;
        AlterVertexPosition(ref vertices[tempId], sizeToAdd);
        tempId++;
        AlterVertexPosition(ref vertices[tempId], sizeToAdd);
        
        
    }

    public void GenerateWindows(AtticParams atticParams, Material material)
    {
        windows = new List<Window>();

        for (int j = 0; j < atticParams.windowParams.Count; j++)
        {
            windows.Add(new Window(obj.transform, material, atticParams.windowParams[j]));
        }
    }


}
