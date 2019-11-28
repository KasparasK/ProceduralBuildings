using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attic : Segment
{
    private const string name = "attic";

    private const float minRoofHeight = 1;
    private const float maxRoofHeight = 2.5f;

    public Attic(Material material, Base parentBase, Action<Vector3[]> verticesDebugger = null)
    {
        base.verticesDebugger = verticesDebugger;
        baseCubeSize = new Vector3Int(1,1,1);

        GenerateBaseCube(material, baseCubeSize,name);
        obj.transform.parent = parentBase.obj.transform;
        AlterMesh(parentBase.finalSize);
    }

    void AlterMesh(Vector3 lastFloorSize)
    {
        Mesh mesh = obj.GetComponent<MeshFilter>().sharedMesh;
        Vector3[] vertices = mesh.vertices;

        finalSize = GetFinalSize(lastFloorSize);

        AlterCubeSize(finalSize, baseCubeSize, ref vertices);
        FormAttic(finalSize, lastFloorSize, ref vertices);
        mesh.vertices = vertices;

        int removeFrom = CalculateRingSize() * (baseCubeSize.y + 1);
        int removeTo = removeFrom + ((baseCubeSize.x + 1) * (baseCubeSize.z + 1)) - 1;
        RemoveVerticesAndTriangles(removeFrom, removeTo);


      //  VisualiseVertices(mesh.vertices);
        pos = obj.transform.localPosition = GetFinalPosition(lastFloorSize, finalSize);
    }
    Vector3 GetFinalPosition(Vector3 lastBaseSize, Vector3 currSize)
    {

        float x = 0;
        float y = lastBaseSize.y;
        float z = (lastBaseSize.z - currSize.z) / 2;

        Vector3 finalPosition = new Vector3(x, y, z);
        return finalPosition;
    }

    Vector3 GetFinalSize(Vector3 lastFloorSize)
    {

        Vector3 finalSize = new Vector3(
            lastFloorSize.x,
            UnityEngine.Random.Range(minRoofHeight, maxRoofHeight),
            lastFloorSize.z
        );

        return finalSize;

    }


    void FormAttic(Vector3 goalSize, Vector3 lastBaseSize, ref Vector3[] vertices)
    {
        int ring = CalculateRingSize();
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
}
