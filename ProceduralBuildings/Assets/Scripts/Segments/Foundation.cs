using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Foundation : Segment
{
    private const string name = "foundation";

    private const float sideDecorWidth = 0.3f;
    private const float sideDecorDepth = 0.08f;
    public float addedDecorWidth;


    public Foundation(Material material, Transform buildingParent, Base firstBase, Action<Vector3[]> verticesDebugger = null)
    {
        base.verticesDebugger = verticesDebugger;

        baseObjSize = BaseObjSizes.baseSize;


        finalSize = GetSize(firstBase.finalSize);
        pos = GetPosition(firstBase, finalSize, false, false, false);

        GenerateBaseCube(material, baseObjSize, name);

        obj.transform.parent = firstBase.obj.transform;

        Mesh mesh = obj.GetComponent<MeshFilter>().sharedMesh;

        Vector3[] vertices = mesh.vertices;
        AlterCubeSize(finalSize, baseObjSize, ref vertices);
        obj.transform.localPosition = pos;

        AddSidePilars(ref vertices, false, false, false,sideDecorWidth,sideDecorDepth,ref addedDecorWidth);
        mesh.vertices = vertices;
        mesh.uv = GenerateUVs(vertices.Length);

        /*  int removeFrom = CalculateRingSize() * (baseObjSize.y + 1);
          int removeTo = removeFrom + ((baseObjSize.x + 1) * (baseObjSize.z + 1)) - 1;
          RemoveVerticesAndTriangles(removeFrom, removeTo);
          */
    }

    Vector3 GetSize(Vector3 lastFloorSize)
    {
        return new Vector3(lastFloorSize.x + 0.1f, lastFloorSize.y / 4, lastFloorSize.z +0.1f );
    }
    Vector3 GetPosition(Base lastBase, Vector3 currSize, bool leftFirewall, bool rightFirewall, bool backFirewall)
    {
        float x = (lastBase.finalSize.x - currSize.x) / 2;
        float y = 0;
        float z = (lastBase.finalSize.z - currSize.z) / 2;

        if (leftFirewall)
            x = 0;
        if (rightFirewall)
            x = (lastBase.finalSize.x - currSize.x);
        if (backFirewall)
            z = (lastBase.finalSize.z - currSize.z);

        Vector3 finalPosition = new Vector3(x, y, z);
        return finalPosition;
    }
    protected override Vector2[] GenerateUVs(int verticesLength)
    {
        Vector2 color = GetColorPosition(TextureColorIDs.grey);
        Vector2[] uvs = new Vector2[verticesLength];
        for (int i = 0; i < verticesLength; i++)
        {
            uvs[i] = color;
        }

        return uvs;
    }
}
