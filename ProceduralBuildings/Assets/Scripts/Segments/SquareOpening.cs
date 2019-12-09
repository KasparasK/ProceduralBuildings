using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareOpening : Segment
{
    private const string name = "squareOpening";
    private Vector2Int color;
    public SquareOpening(Vector3 goalSize, Material material,Vector3 winFrameDimensions, Vector2Int color, Action<Vector3[]> verticesDebugger = null)
    {
        base.verticesDebugger = verticesDebugger;
        baseObjSize = BaseObjSizes.openingSqSize;
        GenerateBaseCube(material, baseObjSize, name);
        Mesh mesh = obj.GetComponent<MeshFilter>().sharedMesh;
        this.color = color;
        Vector3[] vertices = mesh.vertices;

        AlterCubeSize(new Vector3(winFrameDimensions.x, goalSize.y*baseObjSize.y, winFrameDimensions.z), baseObjSize,ref vertices);
        SquareUp(goalSize,ref vertices, winFrameDimensions.x);

        mesh.vertices = vertices;
        mesh.uv = GenerateUVs(vertices.Length);

        RemoveVerticesAndTriangles(CalculateRingSize()*(baseObjSize.y+1),vertices.Length-1);

    }

    protected override Vector2[] GenerateUVs(int verticesLength)
    {
        Vector2[] uvs = new Vector2[verticesLength];
        Vector2 wallsColor = GetColorPosition(color);
        for (int i = 0; i < verticesLength; i++)
        {
            uvs[i] = wallsColor;
        }

        return uvs;
    }

    void SquareUp(Vector3 goalSize, ref Vector3[] vertices,float xSize)
    {
        int ring = CalculateRingSize();
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

    }

   
}
