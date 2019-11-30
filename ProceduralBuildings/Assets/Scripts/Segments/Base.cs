using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Base : Segment
{
    private const string name = "base";

    private readonly Vector3 addToLastSize = new Vector3(0.7f,0.3f,0.7f);
    private readonly Vector3 minBaseSize = new Vector3(3, 1, 3.5f);

    public OpeningStyle windowStyle;
    public OpeningStyle doorStyle;
    public Base(Material material, Transform buildingParent, int windowStyle, int doorStyle, bool leftFirewall, bool rightFirewall, bool backFirewall,Base lastBase = null, Action<Vector3[]> verticesDebugger = null)
    {
        base.verticesDebugger = verticesDebugger;
        this.windowStyle = (OpeningStyle)windowStyle;
        this.doorStyle = (OpeningStyle)doorStyle;

        baseCubeSize = new Vector3Int(5,1,5);

        if (lastBase == null)
        {
            finalSize = GetFinalSize(minBaseSize,leftFirewall,rightFirewall,backFirewall);
            pos = Vector3.zero;
        }
        else
        {
            finalSize = GetFinalSize(lastBase.finalSize, leftFirewall, rightFirewall, backFirewall);
            pos = GetFinalPosition(lastBase, finalSize, leftFirewall, rightFirewall, backFirewall);
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

    Vector3 GetFinalPosition(Base lastBase, Vector3 currSize, bool leftFirewall, bool rightFirewall, bool backFirewall)
    {
        float x = (lastBase.finalSize.x - currSize.x) / 2;
        float y = lastBase.finalSize.y;
        float z = (lastBase.finalSize.z - currSize.z) / 2;

        if (leftFirewall)
            x = 0;
        if(rightFirewall)
            x = (lastBase.finalSize.x - currSize.x);
        if(backFirewall)
            z = (lastBase.finalSize.z - currSize.z);

        Vector3 finalPosition = new Vector3(x,y,z);
        return finalPosition;
    }
    Vector3 GetFinalSize(Vector3 lastFloorSize, bool leftFirewall, bool rightFirewall, bool backFirewall)
    {
        Vector3 tempAddToLast = addToLastSize;

    /*    float x = Random.Range(minSize.x, maxSize.x);
        float y = Random.Range(minSize.y, maxSize.y);
        float z = Random.Range(minSize.z, maxSize.z);*/


        if (leftFirewall && rightFirewall)
        {
            tempAddToLast.x = 0;
        }
        else if (leftFirewall || rightFirewall)
        {
            tempAddToLast.x /= 2;
        }

        if (backFirewall)
        {
            tempAddToLast.z /= 2;
        }

        Vector3 minSize = lastFloorSize;
        Vector3 maxSize = lastFloorSize + tempAddToLast;

        Vector3 finalSize = new Vector3(
                Random.Range(minSize.x,maxSize.x),
                Random.Range(minSize.y, maxSize.y),
                Random.Range(minSize.z, maxSize.z)
            );

        return finalSize;

    }

    public void GenerateWindows(Material windowMat, Material glassMat, Material segmentaionMat, bool leftFirewall, bool rightFirewall, bool backFirewall)
    {
        WindowsGenerator windowsGenerator = new WindowsGenerator(this, windowMat, glassMat,segmentaionMat, leftFirewall, rightFirewall, backFirewall);
    }
}
