using System;   
using UnityEngine;

public class Foundation : Segment
{
    private const string name = "foundation";

    private const float sideDecorWidth = 0.3f;
    private const float sideDecorDepth = 0.08f;
    public float addedDecorWidth;


    public Foundation(Material material, Transform parent, FoundationParams foundationParams,BuildingParams buildingParams, Action<Vector3[]> verticesDebugger = null)
    {
        base.verticesDebugger = verticesDebugger;

        Vector3Int baseObjSize = foundationParams.baseObjSize;

        GenerateBaseCube(material, baseObjSize, name);

        obj.transform.parent = parent;

        Mesh mesh = obj.GetComponent<MeshFilter>().sharedMesh;

        Vector3[] vertices = mesh.vertices;
        AlterCubeSize(foundationParams.finalSize, baseObjSize, ref vertices);
        obj.transform.localPosition = foundationParams.finalPos;

        if (buildingParams.generateCornerPillars)
        {
            AddSidePilars(ref vertices, false, false, false, sideDecorWidth, sideDecorDepth, ref addedDecorWidth,
                foundationParams.finalSize, baseObjSize, null);
        }

        mesh.vertices = vertices;

        int removeFrom = CalculateRingSize(baseObjSize) * (baseObjSize.y + 1)+((baseObjSize.x + 1) * (baseObjSize.z + 1));
        int removeTo = removeFrom + ((baseObjSize.x + 1) * (baseObjSize.z + 1)) - 1;
        RemoveVerticesAndTriangles(removeFrom, removeTo);

        mesh = obj.GetComponent<MeshFilter>().sharedMesh;
        vertices = mesh.vertices;
        mesh.uv = GenerateUVs(vertices.Length, foundationParams.color);

    }

   
}
