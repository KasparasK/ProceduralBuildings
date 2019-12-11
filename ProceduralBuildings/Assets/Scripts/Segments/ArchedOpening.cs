using System;
using UnityEngine;

public class ArchedOpening : Segment
{
    private const string name = "archedOpening";


    public ArchedOpening(Vector3 goalSize, Material material, Vector3 winFrameDimensions, Vector2Int color, WindowParams winParams, Action<Vector3[]> verticesDebugger = null)
    {
        base.verticesDebugger = verticesDebugger;
        Vector3Int baseObjSize = BaseObjSizes.openingArcSize;
        GenerateBaseCube(material, baseObjSize, name);
        Mesh mesh = obj.GetComponent<MeshFilter>().sharedMesh;
        Vector3[] vertices = mesh.vertices;

        AlterCubeSize(new Vector3(winFrameDimensions.x, goalSize.y * baseObjSize.y, winFrameDimensions.z), baseObjSize, ref vertices);
        AlterMesh(goalSize, winFrameDimensions, winParams, ref vertices, baseObjSize);
       // SquareUp(goalSize, ref vertices, winFrameDimensions.x);

        mesh.vertices = vertices;

        RemoveVerticesAndTriangles(CalculateRingSize(baseObjSize) * (baseObjSize.y + 1), vertices.Length - 1);
        mesh = obj.GetComponent<MeshFilter>().sharedMesh;
        mesh.vertices = vertices;
        mesh.uv = GenerateUVs(vertices.Length, color);

    }

    void AlterMesh(Vector3 goalSize, Vector3 winFrameDimensions, WindowParams winParams, ref Vector3[] vertices,Vector3Int baseObjSize)
    {
        int arcPoints = baseObjSize.y - 3;

        int ring = CalculateRingSize(baseObjSize);
        int tempId = baseObjSize.x;

        Vector3 posToAdd = new Vector3(0, winFrameDimensions.x, 0);

        for (int i = 0; i < ring / 2; i++)
        {
            AlterVertexPosition(ref vertices[tempId], posToAdd);
            tempId++;
        }
        //---------------------------------
        tempId = ring;
        for (int i = 0; i <= arcPoints; i++)
        {
            SetVertexPosition(ref vertices[tempId], winParams.outerArcF[i]);
            tempId++;
            SetVertexPosition(ref vertices[tempId], winParams.innerArcF[i]);
            tempId++;
            SetVertexPosition(ref vertices[tempId], winParams.innerArcF[i]);
            tempId++;
            SetVertexPosition(ref vertices[tempId], winParams.innerArcB[i]);
            tempId++;
            SetVertexPosition(ref vertices[tempId], winParams.innerArcB[i]);
            tempId++;
            SetVertexPosition(ref vertices[tempId], winParams.outerArcB[i]);
            tempId++;
            SetVertexPosition(ref vertices[tempId], winParams.outerArcB[i]);
            tempId++;
            SetVertexPosition(ref vertices[tempId], winParams.outerArcF[i]);
            tempId++;

        }
        //---------------------------------
        tempId = ring * (baseObjSize.y-1);
        posToAdd = new Vector3(goalSize.x, goalSize.y * -(baseObjSize.y - 1), 0);

        AlterVertexPosition(ref vertices[tempId], posToAdd);
        tempId++;
        posToAdd = new Vector3(goalSize.x - winFrameDimensions.x * 2, goalSize.y * -(baseObjSize.y - 1) + winFrameDimensions.x, 0);

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
        posToAdd = new Vector3(0, goalSize.y * -baseObjSize.y + winFrameDimensions.x, 0);

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
