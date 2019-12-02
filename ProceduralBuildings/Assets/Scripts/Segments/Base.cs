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
        obj.transform.localPosition = pos;

        AddSideDecor(ref vertices,backFirewall,leftFirewall,rightFirewall,lastBase);
        mesh.vertices = vertices;
        mesh.uv = GenerateUVs(vertices.Length);

        /*  int removeFrom = CalculateRingSize() * (baseCubeSize.y + 1);
          int removeTo = removeFrom + ((baseCubeSize.x + 1) * (baseCubeSize.z + 1)) - 1;
          RemoveVerticesAndTriangles(removeFrom, removeTo);
          */
    }

    protected override Vector2[] GenerateUVs(int verticesLength)
    {
        Vector2 wallsColor = GetColorPosition(TextureColorIDs.lightBrown);
        Vector2 pillarsColor = GetColorPosition(TextureColorIDs.black);
        bool pillarColor = false;
        Vector2[] uvs = new Vector2[verticesLength];
        uvs[0] = pillarsColor;
        uvs[1] = pillarsColor;
        int increment = 2;
        int i = 2;
        while (i < verticesLength - ((baseCubeSize.x+1)* (baseCubeSize.z + 1)*2))
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

    void AddSideDecor(ref Vector3[] vertices, bool backFirewall, bool leftFirewall, bool rightFirewall,
        Base lastBase = null)
    {
        if (lastBase != null)
        {
            float diff = finalSize.z - lastBase.finalSize.z;
            if (backFirewall)
            {
                addedDecorWidth = sideDecorWidth > diff ? 0 : (diff);
            }
            else
            {
                addedDecorWidth = sideDecorWidth > diff ? 0 : (diff) / 2;
            }

            if (leftFirewall && rightFirewall)
                addedDecorWidth += lastBase.addedDecorWidth;
            else
                addedDecorWidth = 0;

        }
        else
            addedDecorWidth = 0;

        int ring = CalculateRingSize();
        int tempId = 0;
        //front down

        for (int j = 0; j <= baseCubeSize.y; j++)
        {
            tempId = ring * j;

            Vector3 pos = new Vector3(-sideDecorDepth, 0, -sideDecorDepth);
            AlterVertexPosition(ref vertices[tempId], pos);
            tempId++;
            pos = new Vector3(sideDecorWidth - sideDecorDepth, vertices[tempId].y, -sideDecorDepth);
            SetVertexPosition(ref vertices[tempId], pos);
            tempId++;
            pos = new Vector3(sideDecorWidth - sideDecorDepth, vertices[tempId].y, vertices[tempId].z);
            SetVertexPosition(ref vertices[tempId], pos);
            tempId = (ring * j) + baseCubeSize.x - 2;
            pos = new Vector3(finalSize.x - (sideDecorWidth - sideDecorDepth), vertices[tempId].y, vertices[tempId].z);
            SetVertexPosition(ref vertices[tempId], pos);
            tempId++;
            pos = new Vector3(finalSize.x - (sideDecorWidth - sideDecorDepth), vertices[tempId].y,
                vertices[tempId].z - sideDecorDepth);
            // SetVertexPosition(ref vertices[tempId], pos);
            tempId++;
            //first corner
            pos = new Vector3(+sideDecorDepth, 0, -sideDecorDepth);

            for (int i = 0; i < 2; i++)
            {
                AlterVertexPosition(ref vertices[tempId], pos);
                tempId++;
            }

            //right down
            pos = new Vector3(vertices[tempId].x + sideDecorDepth, vertices[tempId].y,
                sideDecorWidth + addedDecorWidth - sideDecorDepth);
            SetVertexPosition(ref vertices[tempId], pos);
            tempId++;
            pos = new Vector3(vertices[tempId].x, vertices[tempId].y,
                sideDecorWidth + addedDecorWidth - sideDecorDepth);
            SetVertexPosition(ref vertices[tempId], pos);
            tempId = (ring * j) + baseCubeSize.x + baseCubeSize.z - 1;
            pos = new Vector3(vertices[tempId].x, vertices[tempId].y,
                finalSize.z - (sideDecorWidth + addedDecorWidth - sideDecorDepth));
            SetVertexPosition(ref vertices[tempId], pos);
            tempId++;
            pos = new Vector3(vertices[tempId].x + sideDecorDepth, vertices[tempId].y,
                finalSize.z - (sideDecorWidth + addedDecorWidth - sideDecorDepth));
            SetVertexPosition(ref vertices[tempId], pos);
            tempId++;
            pos = new Vector3(+sideDecorDepth, 0, +sideDecorDepth);
            //second corner
            for (int i = 0; i < 2; i++)
            {
                AlterVertexPosition(ref vertices[tempId], pos);
                tempId++;
            }

            //back down
            pos = new Vector3(finalSize.x - (sideDecorWidth - sideDecorDepth), vertices[tempId].y,
                vertices[tempId].z + sideDecorDepth);
            SetVertexPosition(ref vertices[tempId], pos);
            tempId++;
            pos = new Vector3(finalSize.x - (sideDecorWidth - sideDecorDepth), vertices[tempId].y, vertices[tempId].z);
            SetVertexPosition(ref vertices[tempId], pos);
            tempId = (ring * j) + baseCubeSize.x - 2;
            pos = new Vector3(finalSize.x - (sideDecorWidth - sideDecorDepth), vertices[tempId].y, vertices[tempId].z);
            SetVertexPosition(ref vertices[tempId], pos);
            tempId++;
            pos = new Vector3(finalSize.x - (sideDecorWidth - sideDecorDepth), vertices[tempId].y,
                vertices[tempId].z - sideDecorDepth);
            SetVertexPosition(ref vertices[tempId], pos);
            tempId = (ring * j) + baseCubeSize.x * 2 + baseCubeSize.z;
            pos = new Vector3(0 + (sideDecorWidth - sideDecorDepth), vertices[tempId].y, vertices[tempId].z);
            SetVertexPosition(ref vertices[tempId], pos);
            tempId++;
            pos = new Vector3(0 + (sideDecorWidth - sideDecorDepth), vertices[tempId].y,
                vertices[tempId].z + sideDecorDepth);
            SetVertexPosition(ref vertices[tempId], pos);
            tempId++;
            //third corner
            pos = new Vector3(-sideDecorDepth, 0, +sideDecorDepth);

            for (int i = 0; i < 2; i++)
            {
                AlterVertexPosition(ref vertices[tempId], pos);
                tempId++;
            }

            //left down
            pos = new Vector3(-sideDecorDepth, vertices[tempId].y,
                finalSize.z - (sideDecorWidth + addedDecorWidth - sideDecorDepth));
            SetVertexPosition(ref vertices[tempId], pos);
            tempId++;
            pos = new Vector3(0, vertices[tempId].y, finalSize.z - (sideDecorWidth + addedDecorWidth - sideDecorDepth));
            SetVertexPosition(ref vertices[tempId], pos);
            tempId = (ring * j) + ring - 3;
            pos = new Vector3(0, vertices[tempId].y, (sideDecorWidth + addedDecorWidth - sideDecorDepth));
            SetVertexPosition(ref vertices[tempId], pos);
            tempId++;
            pos = new Vector3(-sideDecorDepth, vertices[tempId].y, (sideDecorWidth + addedDecorWidth - sideDecorDepth));

            SetVertexPosition(ref vertices[tempId], pos);
            tempId++;
            pos = new Vector3(-sideDecorDepth, 0, -sideDecorDepth);

            //forth corner
            AlterVertexPosition(ref vertices[tempId], pos);
        }
        for (int j = 0; j < 2; j++)
        {
            //pirma eile
            tempId++; // = vertices.Length+1 - (baseCubeSize.x + 1) * (baseCubeSize.z + 1);
            pos = new Vector3(-sideDecorDepth, 0, -sideDecorDepth);
            AlterVertexPosition(ref vertices[tempId], pos);
            tempId++;
            pos = new Vector3(sideDecorWidth - sideDecorDepth, vertices[tempId].y, -sideDecorDepth);
            SetVertexPosition(ref vertices[tempId], pos);
            tempId++;
            pos = new Vector3(sideDecorWidth - sideDecorDepth, vertices[tempId].y, vertices[tempId].z);
            SetVertexPosition(ref vertices[tempId], pos);
            tempId += baseCubeSize.x - 4;
            pos = new Vector3(finalSize.x - (sideDecorWidth - sideDecorDepth), vertices[tempId].y, vertices[tempId].z);
            SetVertexPosition(ref vertices[tempId], pos);
            tempId++;
            pos = new Vector3(finalSize.x - (sideDecorWidth - sideDecorDepth), vertices[tempId].y,
                vertices[tempId].z - sideDecorDepth);
            SetVertexPosition(ref vertices[tempId], pos);
            tempId++;
            pos = new Vector3(+sideDecorDepth, 0, -sideDecorDepth);
            AlterVertexPosition(ref vertices[tempId], pos);
            //amtra eile
            tempId++;
            pos = new Vector3(-sideDecorDepth, vertices[tempId].y, (sideDecorWidth + addedDecorWidth - sideDecorDepth));
            SetVertexPosition(ref vertices[tempId], pos);
            tempId++;
            pos = new Vector3((sideDecorWidth - sideDecorDepth), vertices[tempId].y, sideDecorWidth - sideDecorDepth);
            SetVertexPosition(ref vertices[tempId], pos);
            tempId++;
            SetVertexPosition(ref vertices[tempId], pos);
            tempId += baseCubeSize.x - 4;
            pos = new Vector3(finalSize.x - (sideDecorWidth - sideDecorDepth), vertices[tempId].y,
                sideDecorWidth - sideDecorDepth);
            SetVertexPosition(ref vertices[tempId], pos);
            tempId++;
            SetVertexPosition(ref vertices[tempId], pos);
            tempId++;
            pos = new Vector3(vertices[tempId].x + sideDecorDepth, vertices[tempId].y,
                sideDecorWidth + addedDecorWidth - sideDecorDepth);
            SetVertexPosition(ref vertices[tempId], pos);
            //trecia eile
            tempId++;
            pos = new Vector3(0, vertices[tempId].y, (sideDecorWidth + addedDecorWidth - sideDecorDepth));
            SetVertexPosition(ref vertices[tempId], pos);
            tempId++;
            SetVertexPosition(ref vertices[tempId], pos);
            tempId += baseCubeSize.x - 2;
            pos = new Vector3(finalSize.x - (sideDecorWidth - sideDecorDepth), vertices[tempId].y,
                sideDecorWidth - sideDecorDepth);
            SetVertexPosition(ref vertices[tempId], pos);
            tempId++;
            pos = new Vector3(vertices[tempId].x, vertices[tempId].y, sideDecorWidth + addedDecorWidth - sideDecorDepth);
            SetVertexPosition(ref vertices[tempId], pos);
            //trecia nuo galo (ketvirta) eile TOLESNIS KODAS NEPRITAIKYTAS KITOKIAM NEI 5x5 KUBUI
            tempId++;
            pos = new Vector3(0, vertices[tempId].y, finalSize.z - (sideDecorWidth + addedDecorWidth - sideDecorDepth));
            SetVertexPosition(ref vertices[tempId], pos);
            tempId++;
            pos = new Vector3((sideDecorWidth - sideDecorDepth), vertices[tempId].y,
                finalSize.z - (sideDecorWidth + addedDecorWidth - sideDecorDepth));
            SetVertexPosition(ref vertices[tempId], pos);
            tempId += baseCubeSize.x - 2;
            pos = new Vector3(finalSize.x - (sideDecorWidth - sideDecorDepth), vertices[tempId].y,
                finalSize.z - (sideDecorWidth + addedDecorWidth - sideDecorDepth));
            SetVertexPosition(ref vertices[tempId], pos);
            tempId++;
            pos = new Vector3(vertices[tempId].x, vertices[tempId].y,
                finalSize.z - (sideDecorWidth + addedDecorWidth - sideDecorDepth));
            SetVertexPosition(ref vertices[tempId], pos);
            //penkta eile
            tempId++;
            pos = new Vector3(-sideDecorDepth, vertices[tempId].y,
                finalSize.z - (sideDecorWidth + addedDecorWidth - sideDecorDepth));
            SetVertexPosition(ref vertices[tempId], pos);
            tempId++;
            pos = new Vector3((sideDecorWidth - sideDecorDepth), vertices[tempId].y,
                finalSize.z - (sideDecorWidth + addedDecorWidth - sideDecorDepth));
            SetVertexPosition(ref vertices[tempId], pos);
            tempId++;
            SetVertexPosition(ref vertices[tempId], pos);

            tempId += baseCubeSize.x - 4;
            pos = new Vector3(finalSize.x - (sideDecorWidth - sideDecorDepth), vertices[tempId].y,
                finalSize.z - (sideDecorWidth + addedDecorWidth - sideDecorDepth));
            SetVertexPosition(ref vertices[tempId], pos);
            tempId++;
            SetVertexPosition(ref vertices[tempId], pos);
            tempId++;

            pos = new Vector3(vertices[tempId].x + sideDecorDepth, vertices[tempId].y,
                finalSize.z - (sideDecorWidth + addedDecorWidth - sideDecorDepth));
            SetVertexPosition(ref vertices[tempId], pos);
            //paskutine eile
            tempId++;
            pos = new Vector3(-sideDecorDepth, 0, +sideDecorDepth);
            AlterVertexPosition(ref vertices[tempId], pos);
            tempId++;
            pos = new Vector3(0 + (sideDecorWidth - sideDecorDepth), vertices[tempId].y,
                vertices[tempId].z + sideDecorDepth);
            SetVertexPosition(ref vertices[tempId], pos);
            tempId++;
            pos = new Vector3(0 + (sideDecorWidth - sideDecorDepth), vertices[tempId].y, vertices[tempId].z);
            SetVertexPosition(ref vertices[tempId], pos);
            tempId += baseCubeSize.x - 4;
            pos = new Vector3(finalSize.x - (sideDecorWidth - sideDecorDepth), vertices[tempId].y, vertices[tempId].z);
            SetVertexPosition(ref vertices[tempId], pos);
            tempId++;
            pos = new Vector3(finalSize.x - (sideDecorWidth - sideDecorDepth), vertices[tempId].y,
                vertices[tempId].z + sideDecorDepth);
            SetVertexPosition(ref vertices[tempId], pos);
            tempId++;
            pos = new Vector3(sideDecorDepth, 0, +sideDecorDepth);
            AlterVertexPosition(ref vertices[tempId], pos);
            
        }
       

    }

    public void GenerateWindows(Material material, bool leftFirewall, bool rightFirewall, bool backFirewall)
    {
        WindowsGenerator windowsGenerator = new WindowsGenerator(this, material, leftFirewall, rightFirewall, backFirewall);
    }
}
