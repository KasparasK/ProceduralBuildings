using System;   
using UnityEngine;

public class Foundation : Segment
{
    private const string name = "foundation";

   
    private readonly FoundationParams foundationParams;

    public Foundation(Material material, Transform parent, FoundationParams _foundationParams,
        BuildingParams buildingParams)
    {
        foundationParams = _foundationParams;

        CreateBase(material, parent, buildingParams);
    }

    void CreateBase(Material material, Transform parent, BuildingParams buildingParams)
    {
        Vector3Int baseObjSize = foundationParams.baseObjSize;

        GenerateBaseCube(material, baseObjSize, name);
        obj.transform.parent = parent;

        AlterMesh(baseObjSize, buildingParams.generateCornerPillars);
    }

    void AlterMesh(Vector3Int baseObjSize, bool generateCorner)
    {
        Mesh mesh = obj.GetComponent<MeshFilter>().sharedMesh;

        Vector3[] vertices = mesh.vertices;
        AlterCubeSize(foundationParams.finalSize, baseObjSize, ref vertices);
        obj.transform.localPosition = foundationParams.finalPos;

        if (generateCorner)
        {
            AddSidePilars(
                ref vertices,
                false,
                false,
                false,
                FoundationParams.sideDecorWidth,
                FoundationParams.sideDecorDepth,
                ref foundationParams.addedDecorWidth,
                foundationParams.finalSize,
                baseObjSize, 
                null);
        }

        mesh.vertices = vertices;

        int removeFrom = CalculateRingSize(baseObjSize) * (baseObjSize.y + 1) +
                         ((baseObjSize.x + 1) * (baseObjSize.z + 1));
        int removeTo = removeFrom + ((baseObjSize.x + 1) * (baseObjSize.z + 1)) - 1;
        RemoveVerticesAndTriangles(removeFrom, removeTo);

        mesh = obj.GetComponent<MeshFilter>().sharedMesh;
        vertices = mesh.vertices;
        mesh.uv = GenerateUVs(vertices.Length, foundationParams.color);
    }


}
