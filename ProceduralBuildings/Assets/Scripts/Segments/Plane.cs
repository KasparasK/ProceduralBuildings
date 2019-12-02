using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plane : Segment
{
    private const string name = "plane";

    private Vector2Int color;
    public Plane(Material material, Transform parent, Vector3 size,Vector2Int color ,Action<Vector3[]> verticesDebugger = null)
    {
        base.verticesDebugger = verticesDebugger;
        this.color = color;
        baseCubeSize = new Vector3Int(1, 1, 0);

        GenerateBaseCube(material, baseCubeSize, name);

        obj.transform.parent = parent;

        Mesh mesh = obj.GetComponent<MeshFilter>().sharedMesh;

        Vector3[] vertices = mesh.vertices;
        AlterCubeSize(size, baseCubeSize, ref vertices);
        mesh.uv = GenerateUVs(vertices.Length);
        mesh.vertices = vertices;

        

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
}
