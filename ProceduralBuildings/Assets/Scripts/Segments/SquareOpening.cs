using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareOpening : Segment
{
    private const string name = "squareOpening";
    private SquareOpeningParams squareOpeningParams;
    public SquareOpening(Vector3 size, Material material,SquareOpeningParams _squareOpeningParams)
    {
        squareOpeningParams = _squareOpeningParams;

        CreateBase(size, material);
    }

    void CreateBase(Vector3 size, Material material)
    {
        Vector3Int baseObjSize = BaseObjSizes.openingSqSize;
        GenerateBaseCube(material, baseObjSize, name);
        Mesh mesh = obj.GetComponent<MeshFilter>().sharedMesh;

        Vector3[] vertices = mesh.vertices;

        AlterCubeSize(new Vector3(squareOpeningParams.frameDimensions.x, size.y * baseObjSize.y, squareOpeningParams.frameDimensions.z), baseObjSize, ref vertices);
        vertices = SquareUp(size, vertices, squareOpeningParams.frameDimensions.x, baseObjSize);

        mesh.vertices = vertices;

        RemoveVerticesAndTriangles(CalculateRingSize(baseObjSize) * (baseObjSize.y + 1), vertices.Length - 1);
        mesh = obj.GetComponent<MeshFilter>().sharedMesh;
        mesh.vertices = vertices;
        mesh.uv = GenerateUVs(vertices.Length, squareOpeningParams.frameColor);
    }
    Vector3[] SquareUp(Vector3 goalSize, Vector3[] vertices,float xSize,Vector3Int baseObjSize)
    {
        int ring = CalculateRingSize(baseObjSize);
        int tempId = baseObjSize.x;

        Vector3 posToAdd = new Vector3(0,xSize,0);

        for (int i = 0; i < ring/2; i++)
        {
            AlterVertexPosition(ref vertices[tempId], posToAdd);
            tempId++;
        }
        //---------------------------------
        posToAdd = new Vector3(0, -xSize, 0);
        tempId = ring + baseObjSize.x;
        for (int i = 0; i < ring / 2; i++)
        {
            AlterVertexPosition(ref vertices[tempId], posToAdd);
            tempId++;
        }
        //---------------------------------

        tempId = ring * 2;
        posToAdd = new Vector3(goalSize.x, goalSize.y*-1, 0);

        AlterVertexPosition(ref vertices[tempId], posToAdd);
        tempId++;
        posToAdd = new Vector3(goalSize.x-xSize*2, goalSize.y*-1-xSize, 0);

        for (int i = 0; i < ring / 2; i++)
        {
            AlterVertexPosition(ref vertices[tempId], posToAdd);
            tempId++;
        }
        posToAdd = new Vector3(goalSize.x, goalSize.y*-1, 0);

        for (int i = 0; i < (ring / 2)-1; i++)
        {
            AlterVertexPosition(ref vertices[tempId], posToAdd);
            tempId++;
        }
        //---------------------------------
        tempId = ring * 3;
        posToAdd = new Vector3(goalSize.x, goalSize.y * -3, 0);

        AlterVertexPosition(ref vertices[tempId], posToAdd);
        tempId++;
        posToAdd = new Vector3(goalSize.x - xSize*2, goalSize.y * -3 + xSize, 0);

        for (int i = 0; i < ring / 2; i++)
        {
            AlterVertexPosition(ref vertices[tempId], posToAdd);
            tempId++;
        }
        posToAdd = new Vector3(goalSize.x, goalSize.y * -3, 0);

        for (int i = 0; i < (ring / 2) - 1; i++)
        {
            AlterVertexPosition(ref vertices[tempId], posToAdd);
            tempId++;
        }
        //---------------------------------

        tempId = ring * 4;
        posToAdd = new Vector3(0, goalSize.y * -4, 0);

        AlterVertexPosition(ref vertices[tempId], posToAdd);
        tempId++;
        posToAdd = new Vector3(0, goalSize.y * -4 + xSize, 0);

        for (int i = 0; i < ring / 2; i++)
        {
            AlterVertexPosition(ref vertices[tempId], posToAdd);
            tempId++;
        }
        posToAdd = new Vector3(0, goalSize.y * -4, 0);

        for (int i = 0; i < (ring / 2) - 1; i++)
        {
            AlterVertexPosition(ref vertices[tempId], posToAdd);
            tempId++;
        }
        //---------------------------------
        return vertices;
    }

   
}
