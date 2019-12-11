using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Segmentation : Segment
{
    private const string name = "segments";
    public Segmentation(GameObject parentObj ,Vector3 winSize, Vector3 segmentDimensions, Material material, List<Vector3> vertSegPositions, List<Vector3> horSegPositions,Vector2Int color, Action<Vector3[]> verticesDebugger = null)
    {
        base.verticesDebugger = verticesDebugger;

        GenerateSegements(parentObj, winSize, material, vertSegPositions, horSegPositions, segmentDimensions,color);
    }
    void GenerateSegements(GameObject parentObj, Vector3 winSize, Material material, List<Vector3> vertSegPositions, List<Vector3> horSegPositions,Vector3 segmentDimensions, Vector2Int color)
    {
        List<GameObject> objs = new List<GameObject>();
        Vector3Int baseObjSize = BaseObjSizes.segmentationSize;

        for (int i = 0; i < horSegPositions.Count; i++)
        {
            GenerateBaseCube(material, baseObjSize, name);
            Mesh mesh = obj.GetComponent<MeshFilter>().sharedMesh;

            Vector3[] vertices = mesh.vertices;

            Vector3 size = new Vector3(winSize.x, segmentDimensions.y, segmentDimensions.z); 

            AlterCubeSize(size, baseObjSize, ref vertices);

            mesh.vertices = vertices;
            obj.transform.parent = parentObj.transform;
            mesh.uv = GenerateUVs(vertices.Length, color);
            obj.transform.localPosition = horSegPositions[i];
            obj.transform.localRotation = Quaternion.Euler(Vector3.zero);

            objs.Add(obj);
        }

       

        for (int i = 0; i < vertSegPositions.Count; i++)
        {
            GenerateBaseCube(material, baseObjSize, name);
            Mesh mesh = obj.GetComponent<MeshFilter>().sharedMesh;

            Vector3[] vertices = mesh.vertices;

            Vector3 size = new Vector3(segmentDimensions.x, winSize.y, segmentDimensions.z);

            AlterCubeSize(size, baseObjSize, ref vertices);

            mesh.vertices = vertices;
            obj.transform.parent = parentObj.transform;
            mesh.uv = GenerateUVs(vertices.Length, color);

            obj.transform.localPosition = vertSegPositions[i];
            obj.transform.localRotation = Quaternion.Euler(Vector3.zero);

            objs.Add(obj);
        }

       // obj = new CombineMeshes().CombineOBJ(objs);
     //   obj.name = name;
    }

}
