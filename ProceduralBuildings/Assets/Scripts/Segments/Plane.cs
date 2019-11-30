using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plane : Segment
{
    private const string name = "plane";

    public Plane(Material material, Transform parent, Vector3 size, Action<Vector3[]> verticesDebugger = null)
    {
        base.verticesDebugger = verticesDebugger;

        baseCubeSize = new Vector3Int(1, 1, 0);

        GenerateBaseCube(material, baseCubeSize, name);

        obj.transform.parent = parent;

        Mesh mesh = obj.GetComponent<MeshFilter>().sharedMesh;

        Vector3[] vertices = mesh.vertices;
        AlterCubeSize(size, baseCubeSize, ref vertices);

        mesh.vertices = vertices;

        

    }
}
