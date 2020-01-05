using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Segmentation : Segment
{
    private const string name = "segments";
    public Segmentation(Transform parent,Vector3 winSize, Material material, SegmentationParams segmentationParams)
    {

        GenerateSegements(parent, winSize, material, segmentationParams);
    }
    void GenerateSegements(Transform parent, Vector3 winSize, Material material, SegmentationParams segmentationParams)
    {
        List<GameObject> objs = new List<GameObject>();
        Vector3Int baseObjSize = BaseObjSizes.segmentationSize;

        for (int i = 0; i < segmentationParams.horSegPositions.Count; i++)
        {
            GenerateBaseCube(material, baseObjSize, name);
            Mesh mesh = obj.GetComponent<MeshFilter>().sharedMesh;

            Vector3[] vertices = mesh.vertices;

            Vector3 size = new Vector3(winSize.x, segmentationParams.segmentDimensions.y, segmentationParams.segmentDimensions.z); 

            AlterCubeSize(size, baseObjSize, ref vertices);

            mesh.vertices = vertices;
            obj.transform.parent = parent;
            mesh.uv = GenerateUVs(vertices.Length, segmentationParams.segmentsColor);
            obj.transform.localPosition = segmentationParams.horSegPositions[i];
            obj.transform.localRotation = Quaternion.Euler(Vector3.zero);

            objs.Add(obj);
        }

       

        for (int i = 0; i < segmentationParams.vertSegPositions.Count; i++)
        {
            GenerateBaseCube(material, baseObjSize, name);
            Mesh mesh = obj.GetComponent<MeshFilter>().sharedMesh;

            Vector3[] vertices = mesh.vertices;

            Vector3 size = new Vector3(segmentationParams.segmentDimensions.x, winSize.y, segmentationParams.segmentDimensions.z);

            AlterCubeSize(size, baseObjSize, ref vertices);

            mesh.vertices = vertices;
            obj.transform.parent = parent;
            mesh.uv = GenerateUVs(vertices.Length, segmentationParams.segmentsColor);

            obj.transform.localPosition = segmentationParams.vertSegPositions[i];
            obj.transform.localRotation = Quaternion.Euler(Vector3.zero);

            objs.Add(obj);
        }

       // obj = new CombineMeshes().CombineOBJ(objs);
     //   obj.name = name;
    }

}
