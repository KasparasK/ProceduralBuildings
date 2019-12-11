using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class Base : Segment
{
    private const string name = "base";
    public List<Window> windows;
    public Base(ref BaseParams baseParams,Transform parent, Material material, BaseParams lastBaseParams = null, Action<Vector3[]> verticesDebugger = null)
    {
        base.verticesDebugger = verticesDebugger;
        Vector3Int baseObjSize = baseParams.baseObjSize;

        GenerateBaseCube(material, baseParams.baseObjSize, name);

        obj.transform.parent = parent;
        Mesh mesh = obj.GetComponent<MeshFilter>().sharedMesh;
        Vector3[] vertices = mesh.vertices;

        AlterCubeSize(baseParams.finalSize, baseParams.baseObjSize, ref vertices);
        AddSidePilars(
            ref vertices,
            baseParams.backFirewall, 
            baseParams.leftFirewall,
            baseParams.rightFirewall,
            baseParams.sideDecorWidth,
            baseParams.sideDecorDepth,
            ref baseParams.addedDecorWidth,
            baseParams.finalSize,
            baseObjSize,
            lastBaseParams);
        mesh.vertices = vertices;

        if (baseParams.groundFloor)
        {
            int removeFrom = CalculateRingSize(baseObjSize) * (baseObjSize.y + 1) + ((baseObjSize.x + 1) * (baseObjSize.z + 1));
            int removeTo = removeFrom + ((baseObjSize.x + 1) * (baseObjSize.z + 1)) - 1;
            RemoveVerticesAndTriangles(removeFrom, removeTo);
        }

        mesh = obj.GetComponent<MeshFilter>().sharedMesh;
        vertices = mesh.vertices;

        obj.GetComponent<MeshFilter>().sharedMesh.uv = GenerateUVs(vertices.Length, baseObjSize,baseParams.wallsColor,baseParams.pillarsColor);
        obj.transform.localPosition = baseParams.finalPos;
    }


    protected Vector2[] GenerateUVs(int verticesLength,Vector3Int baseObjSize,Vector2Int _wallsColor, Vector2Int _pillarsColor)
    {
        Vector2 wallsColor = GetColorPosition(_wallsColor);
        Vector2 pillarsColor = GetColorPosition(_pillarsColor);
        bool pillarColor = false;
        Vector2[] uvs = new Vector2[verticesLength];
        uvs[0] = pillarsColor;
        uvs[1] = pillarsColor;
        int increment = 2;
        int i = 2;
        int ring = CalculateRingSize(baseObjSize);
        while (i < ring*(baseObjSize.y+1))
        {
            for (int j = 0; j < increment; j++)
            {
               // Debug.Log(i+j);
                if(i+j >= verticesLength)
                    break;
                
                uvs[i+j] = pillarColor ? pillarsColor : wallsColor;
            }

            i += increment;
            increment = !pillarColor? 4 : 2;
            pillarColor = !pillarColor;
        }

        return uvs;
    }
}
