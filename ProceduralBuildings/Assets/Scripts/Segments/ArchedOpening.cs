using System;
using UnityEngine;

public class ArchedOpening : Segment
{
    private const string name = "archedOpening";
    private Vector2Int color;


    public ArchedOpening(Vector3 goalSize, Material material, Vector3 winFrameDimensions, Vector2Int color, WindowParams winParams, Action<Vector3[]> verticesDebugger = null)
    {
        base.verticesDebugger = verticesDebugger;
        baseObjSize = BaseObjSizes.openingArcSize;
        GenerateBaseCube(material, baseObjSize, name);
        Mesh mesh = obj.GetComponent<MeshFilter>().sharedMesh;
        this.color = color;
        Vector3[] vertices = mesh.vertices;

        AlterCubeSize(new Vector3(winFrameDimensions.x, goalSize.y * baseObjSize.y, winFrameDimensions.z), baseObjSize, ref vertices);
        AlterMesh(goalSize, winFrameDimensions, winParams, ref vertices);
       // SquareUp(goalSize, ref vertices, winFrameDimensions.x);

        mesh.vertices = vertices;
        mesh.uv = GenerateUVs(vertices.Length);

        RemoveVerticesAndTriangles(CalculateRingSize() * (baseObjSize.y + 1), vertices.Length - 1);

    }

    void AlterMesh(Vector3 goalSize, Vector3 winFrameDimensions, WindowParams winParams, ref Vector3[] vertices)
    {
        int arcPoints = baseObjSize.y - 3;

        int ring = CalculateRingSize();
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

    protected override Vector2[] GenerateUVs(int verticesLength)
    {
        Vector2[] uvs = new Vector2[verticesLength];
        Vector2 color = GetColorPosition( this.color);
        for (int i = 0; i < verticesLength; i++)
        {
            uvs[i] = color;
        }

        return uvs;
    }
}
