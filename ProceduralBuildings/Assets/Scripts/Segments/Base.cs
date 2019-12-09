using System;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class Base : Segment
{
    private const string name = "base";

    private readonly Vector3 addToLastSize = new Vector3(0.7f,0.3f,0.7f);
    private readonly Vector3 minBaseSize = new Vector3(3, 1, 3.5f);

    private const float sideDecorWidth = 0.2f;
    private const float sideDecorDepth = 0.07f;

    public float addedDecorWidth;

    public OpeningStyle windowStyle;
    public OpeningStyle doorStyle;
    public Base(Material material, Transform buildingParent, int windowStyle, int doorStyle, bool leftFirewall, bool rightFirewall, bool backFirewall,Base lastBase = null, Action<Vector3[]> verticesDebugger = null)
    {
        base.verticesDebugger = verticesDebugger;
        this.windowStyle = (OpeningStyle)windowStyle;
        this.doorStyle = (OpeningStyle)doorStyle;

        baseObjSize = BaseObjSizes.baseSize;

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

        GenerateBaseCube(material, baseObjSize, name);

        if (lastBase == null)
            obj.transform.parent = buildingParent;
        else
            obj.transform.parent = lastBase.obj.transform;

        Mesh mesh = obj.GetComponent<MeshFilter>().sharedMesh;

        Vector3[] vertices = mesh.vertices;
        AlterCubeSize(finalSize, baseObjSize, ref vertices);
        obj.transform.localPosition = pos;

        AddSidePilars(ref vertices,backFirewall,leftFirewall,rightFirewall,sideDecorWidth, sideDecorDepth, ref addedDecorWidth,lastBase);
        mesh.vertices = vertices;
        mesh.uv = GenerateUVs(vertices.Length);

        /*  int removeFrom = CalculateRingSize() * (baseObjSize.y + 1);
          int removeTo = removeFrom + ((baseObjSize.x + 1) * (baseObjSize.z + 1)) - 1;
          RemoveVerticesAndTriangles(removeFrom, removeTo);
          */
    }


    protected override Vector2[] GenerateUVs(int verticesLength)
    {
        Vector2 wallsColor = GetColorPosition(TextureColorIDs.yellow);
        Vector2 pillarsColor = GetColorPosition(TextureColorIDs.black);
        bool pillarColor = false;
        Vector2[] uvs = new Vector2[verticesLength];
        uvs[0] = pillarsColor;
        uvs[1] = pillarsColor;
        int increment = 2;
        int i = 2;
        while (i < verticesLength - ((baseObjSize.x+1)* (baseObjSize.z + 1)*2))
        {
            for (int j = 0; j < increment; j++)
            {
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
  

    
    public void GenerateWindows(Material material, bool leftFirewall, bool rightFirewall, bool backFirewall)
    {
        WindowsGenerator windowsGenerator = new WindowsGenerator(this, material, leftFirewall, rightFirewall, backFirewall,windowStyle);
    }
}
