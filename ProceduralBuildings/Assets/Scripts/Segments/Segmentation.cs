using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Segmentation : Segment
{
    private const string name = "segments";
    public Segmentation(GameObject parentObj ,Vector3 winSize, Vector3 segmentDimensions, Material material, List<Vector3> vertSegPositions, List<Vector3> horSegPositions, Action<Vector3[]> verticesDebugger = null)
    {
        base.verticesDebugger = verticesDebugger;
        baseCubeSize = new Vector3Int(1, 1, 1);

        GenerateSegements(parentObj, winSize, material, vertSegPositions, horSegPositions, segmentDimensions);
    }
    protected override Vector2[] GenerateUVs(int verticesLength)
    {
        Vector2[] uvs = new Vector2[verticesLength];
        Vector2 color = GetColorPosition(TextureColorIDs.darkBrown);
        for (int i = 0; i < verticesLength; i++)
        {
            uvs[i] = color;
        }

        return uvs;
    }
    void GenerateSegements(GameObject parentObj, Vector3 winSize, Material material, List<Vector3> vertSegPositions, List<Vector3> horSegPositions,Vector3 segmentDimensions)
    {
        List<GameObject> objs = new List<GameObject>();

        for (int i = 0; i < horSegPositions.Count; i++)
        {
            GenerateBaseCube(material, baseCubeSize, name);
            Mesh mesh = obj.GetComponent<MeshFilter>().sharedMesh;

            Vector3[] vertices = mesh.vertices;

            Vector3 size = new Vector3(winSize.x, segmentDimensions.y, segmentDimensions.z); 

            AlterCubeSize(size, baseCubeSize, ref vertices);

            mesh.vertices = vertices;
            obj.transform.parent = parentObj.transform;
            mesh.uv = GenerateUVs(vertices.Length);
            obj.transform.localPosition = horSegPositions[i];
            obj.transform.localRotation = Quaternion.Euler(Vector3.zero);

            objs.Add(obj);
        }

       

        for (int i = 0; i < vertSegPositions.Count; i++)
        {
            GenerateBaseCube(material, baseCubeSize, name);
            Mesh mesh = obj.GetComponent<MeshFilter>().sharedMesh;

            Vector3[] vertices = mesh.vertices;

            Vector3 size = new Vector3(segmentDimensions.x, winSize.y, segmentDimensions.z);

            AlterCubeSize(size, baseCubeSize, ref vertices);

            mesh.vertices = vertices;
            obj.transform.parent = parentObj.transform;
            mesh.uv = GenerateUVs(vertices.Length);

            obj.transform.localPosition = vertSegPositions[i];
            obj.transform.localRotation = Quaternion.Euler(Vector3.zero);

            objs.Add(obj);
        }

       // obj = new CombineMeshes().CombineOBJ(objs);
     //   obj.name = name;
    }

}
