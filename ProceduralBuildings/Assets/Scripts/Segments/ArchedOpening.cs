using System;
using UnityEngine;

public class ArchedOpening : Segment
{
    private const string name = "archedOpening";


    public ArchedOpening(Vector3 goalSize, Material material, ArchedOpeningParams archedOpeningParams , Action<Vector3[]> verticesDebugger = null)
    {
        base.verticesDebugger = verticesDebugger;
        Vector3Int baseObjSize = BaseObjSizes.openingArcSize;
        GenerateBaseCube(material, baseObjSize, name);
        Mesh mesh = obj.GetComponent<MeshFilter>().sharedMesh;
        Vector3[] vertices = mesh.vertices;

        AlterCubeSize(new Vector3(archedOpeningParams.frameDimensions.x, goalSize.y * baseObjSize.y, archedOpeningParams.frameDimensions.z), baseObjSize, ref vertices);
        AlterMesh(goalSize, archedOpeningParams.frameDimensions, archedOpeningParams, ref vertices, baseObjSize);

        mesh.vertices = vertices;

        RemoveVerticesAndTriangles(CalculateRingSize(baseObjSize) * (baseObjSize.y + 1), vertices.Length - 1);
        mesh = obj.GetComponent<MeshFilter>().sharedMesh;
        mesh.vertices = vertices;
        mesh.uv = GenerateUVs(vertices.Length, archedOpeningParams.frameColor);

    }

    void AlterMesh(Vector3 goalSize, Vector3 frameDimensions, ArchedOpeningParams archedOpeningParams, ref Vector3[] vertices,Vector3Int baseObjSize)
    {
        int arcPoints = baseObjSize.y - 3;

        int ring = CalculateRingSize(baseObjSize);
        int tempId = baseObjSize.x;

        Vector3 posToAdd = new Vector3(0, frameDimensions.x, 0);

        for (int i = 0; i < ring / 2; i++)
        {
            AlterVertexPosition(ref vertices[tempId], posToAdd);
            tempId++;
        }
        //---------------------------------
        tempId = ring;
       for (int i = 0; i <= arcPoints; i++)
        {
            SetVertexPosition(ref vertices[tempId], archedOpeningParams.outerArcF[i]);
            tempId++;
            SetVertexPosition(ref vertices[tempId], archedOpeningParams.innerArcF[i]);
            tempId++;
            SetVertexPosition(ref vertices[tempId], archedOpeningParams.innerArcF[i]);
            tempId++;
            SetVertexPosition(ref vertices[tempId], archedOpeningParams.innerArcB[i]);
            tempId++;
            SetVertexPosition(ref vertices[tempId], archedOpeningParams.innerArcB[i]);
            tempId++;
            SetVertexPosition(ref vertices[tempId], archedOpeningParams.outerArcB[i]);
            tempId++;
            SetVertexPosition(ref vertices[tempId], archedOpeningParams.outerArcB[i]);
            tempId++;
            SetVertexPosition(ref vertices[tempId], archedOpeningParams.outerArcF[i]);
            tempId++;

        }
        //---------------------------------
        tempId = ring * (baseObjSize.y-1);
        posToAdd = new Vector3(goalSize.x, goalSize.y * -(baseObjSize.y - 1), 0);

        AlterVertexPosition(ref vertices[tempId], posToAdd);
        tempId++;
        posToAdd = new Vector3(goalSize.x - frameDimensions.x * 2, goalSize.y * -(baseObjSize.y - 1) + frameDimensions.x, 0);

        for (int i = 0; i < ring / 2; i++)
        {
            AlterVertexPosition(ref vertices[tempId], posToAdd);
            tempId++;
        }
        posToAdd = new Vector3(goalSize.x, goalSize.y * -(baseObjSize.y - 1), 0);

        for (int i = 0; i < (ring / 2) - 1; i++)
        {
            AlterVertexPosition(ref vertices[tempId], posToAdd);
            tempId++;
        }
        //---------------------------------

        tempId = ring * baseObjSize.y;
        posToAdd = new Vector3(0, goalSize.y * -baseObjSize.y, 0);

        AlterVertexPosition(ref vertices[tempId], posToAdd);
        tempId++;
        posToAdd = new Vector3(0, goalSize.y * -baseObjSize.y + frameDimensions.x, 0);

        for (int i = 0; i < ring / 2; i++)
        {
            AlterVertexPosition(ref vertices[tempId], posToAdd);
            tempId++;
        }
        posToAdd = new Vector3(0, goalSize.y * -baseObjSize.y, 0);

        for (int i = 0; i < (ring / 2) - 1; i++)
        {
            AlterVertexPosition(ref vertices[tempId], posToAdd);
            tempId++;
        }
        //---------------------------------

    }
}
