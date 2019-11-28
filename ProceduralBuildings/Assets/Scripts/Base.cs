using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Base : Segment
{
    private const string name = "base";

    private readonly Vector3 addToLastSize = new Vector3(0.7f,0.3f,0.7f);
    private readonly Vector3 minBaseSize = new Vector3(3, 1, 3.5f);

    public Base(Material material, Transform buildingParent, Base lastBase = null, Action<Vector3[]> verticesDebugger = null)
    {
        base.verticesDebugger = verticesDebugger;

        baseCubeSize = new Vector3Int(1,1,1);

        if (lastBase == null)
        {
            finalSize = GetFinalSize(minBaseSize);
            pos = Vector3.zero;
        }
        else
        {
            finalSize = GetFinalSize(lastBase.finalSize);
            pos = GetFinalPosition(lastBase, finalSize);
        }

        GenerateBaseCube(material, baseCubeSize, name);

        if (lastBase == null)
            obj.transform.parent = buildingParent;
        else
            obj.transform.parent = lastBase.obj.transform;

        Mesh mesh = obj.GetComponent<MeshFilter>().sharedMesh;

        Vector3[] vertices = mesh.vertices;
        AlterCubeSize(finalSize, baseCubeSize, ref vertices);

        mesh.vertices = vertices;
        obj.transform.localPosition = pos;

        int removeFrom = CalculateRingSize() * (baseCubeSize.y + 1);
        int removeTo = removeFrom + ((baseCubeSize.x + 1) * (baseCubeSize.z + 1)) - 1;
        RemoveVerticesAndTriangles(removeFrom, removeTo);

    }

    Vector3 GetFinalPosition(Base lastBase, Vector3 currSize)
    {
        float x = (lastBase.finalSize.x - currSize.x) / 2;
        float y = lastBase.finalSize.y;
        float z = (lastBase.finalSize.z - currSize.z) / 2;

        Vector3 finalPosition = new Vector3(x,y,z);
        return finalPosition;
    }
    Vector3 GetFinalSize(Vector3 lastFloorSize)
    {
        Vector3 minSize = lastFloorSize;
        Vector3 maxSize = lastFloorSize + addToLastSize;

        Vector3 finalSize = new Vector3(
                Random.Range(minSize.x,maxSize.x),
                Random.Range(minSize.y, maxSize.y),
                Random.Range(minSize.z, maxSize.z)
            );

        return finalSize;

    }
}
