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
        baseObjSize = BaseObjSizes.planeSqSize;

        GenerateBasePlane(material, baseObjSize, name);

        obj.transform.parent = parent;

        Mesh mesh = obj.GetComponent<MeshFilter>().sharedMesh;

        Vector3[] vertices = mesh.vertices;
        AlterPlaneSize(size, baseObjSize, ref vertices);
        mesh.uv = GenerateUVs(vertices.Length);
        mesh.vertices = vertices;
      
    }
    public Plane(Material material, Transform parent, Vector3 size, Vector2Int color,Vector3[] arcPoints ,Action<Vector3[]> verticesDebugger = null)
    {
        base.verticesDebugger = verticesDebugger;
        this.color = color;
        baseObjSize = BaseObjSizes.planeArcSize;

        GenerateBasePlane(material, baseObjSize, name);

        obj.transform.parent = parent;

        Mesh mesh = obj.GetComponent<MeshFilter>().sharedMesh;

        Vector3[] vertices = mesh.vertices;
        AlterPlaneSize(size, baseObjSize, ref vertices);
        ArcThePlane(arcPoints, ref vertices);
        mesh.uv = GenerateUVs(vertices.Length);
        mesh.vertices = vertices;

        
        VisualiseVertices(vertices);
    }

    void ArcThePlane(Vector3[] arcPoints, ref Vector3[] vertices)
    {
        int tempId = baseObjSize.x+=1;
   
        for (int i = 0; i < arcPoints.Length; i++)
        {
            SetVertexPosition(ref vertices[tempId], arcPoints[i]);
            tempId++;
        }
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
