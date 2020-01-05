using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plane : Segment
{
    private const string name = "plane";
    public Plane(Material material, Transform parent, Vector3 size, PlaneParams planeParams)
    {
        Vector3Int baseObjSize = planeParams.baseObjSize;

        GenerateBasePlane(material, baseObjSize, name);

        obj.transform.parent = parent;

        Mesh mesh = obj.GetComponent<MeshFilter>().sharedMesh;

        Vector3[] vertices = mesh.vertices;
        AlterPlaneSize(size, baseObjSize, ref vertices);
        if(planeParams.openingStyle == OpeningStyle.ARCH)
            ArcThePlane(planeParams.arcPoints, ref vertices, baseObjSize);
        mesh.uv = GenerateUVs(vertices.Length, planeParams.color);
        mesh.vertices = vertices;
      
    }

    void ArcThePlane(Vector3[] arcPoints, ref Vector3[] vertices, Vector3Int baseObjSize)
    {
        int tempId = baseObjSize.x+=1;
   
        for (int i = 0; i < arcPoints.Length; i++)
        {
            SetVertexPosition(ref vertices[tempId], arcPoints[i]);
            tempId++;
        }
    }
}
