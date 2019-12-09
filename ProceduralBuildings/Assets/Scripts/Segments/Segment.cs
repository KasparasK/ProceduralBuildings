using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Segment
{
    public Vector3 pos;
    public Vector3 finalSize;
    public Quaternion rot;
    public GameObject obj;
    public List<Segment> attchedSegments;
    
    public Action<Vector3[]> verticesDebugger;
    protected Vector3Int baseObjSize;

    private MeshGenerator meshGenerator;

    protected const float textureImageSize = 512;
    protected const float textureColorSize = 32;
    protected const float textureOffset = textureColorSize / 2;

    protected int CalculateRingSize()
    {
        return ((baseObjSize.x + 1) * 2) + ((baseObjSize.z + 1) * 2);
    }
    protected void AlterVertexPosition(ref Vector3 vertex, Vector3 posToAdd)
    {
        vertex += posToAdd;
    }
    protected void SetVertexPosition(ref Vector3 vertex, Vector3 newPos)
    {
        vertex = newPos;
    }

    protected void GenerateBaseCube(Material material, Vector3Int size,string name)
    {
        meshGenerator = new MeshGenerator();
        obj = meshGenerator.GenerateBaseRectangle(material, size, name);
    }
    protected void GenerateBasePlane(Material material, Vector3Int size, string name)
    {
        meshGenerator = new MeshGenerator();
        obj = meshGenerator.GenerateBasePlane(material, size, name);
    }

    protected Vector2 GetColorPosition(Vector2Int pos)
    {
        return new Vector2(((textureColorSize * pos.x) - textureOffset) / textureImageSize, ((textureColorSize * pos.y) - textureOffset) / textureImageSize);
    }
    /*.      x--------x
     *.     /|       /|
     *.    / |      / |
     *.   /  x-----/--x
     *.  /  /     /  /       
     *  x--------x  /
     *  | /      | /
     *  |/       |/
     *  x--------x
     */
    //used just to evenly change cube dimensions
    protected void AlterCubeSize(Vector3 goalSize, Vector3Int baseCubeSize, ref Vector3[] vertices)
    {
        int ring = CalculateRingSize();

        goalSize -= baseCubeSize;

        float xStep = goalSize.x / baseCubeSize.x;
        float yStep = goalSize.y / baseCubeSize.y;
        float zStep = goalSize.z / baseCubeSize.z;

        Vector3 sizeToAdd = Vector3.zero;

        int tempId = 0;

        //sides
        for (int i = 0; i <= baseCubeSize.y; i++)
        {
            tempId = i * ring;

            sizeToAdd = Vector3.zero;
            sizeToAdd.y = i * yStep;
            
            AlterVertexPosition(ref vertices[tempId], sizeToAdd);
            tempId++;
            for (int j = 1; j <= baseCubeSize.x; j++)
            {
                sizeToAdd.x = j * xStep;
                AlterVertexPosition(ref vertices[tempId], sizeToAdd);
                tempId++;
            }
            AlterVertexPosition(ref vertices[tempId], sizeToAdd);
            tempId++;

            for (int j = 1; j <= baseCubeSize.z; j++)
            {
                sizeToAdd.z = j * zStep;
                AlterVertexPosition(ref vertices[tempId], sizeToAdd);
                tempId++;
            }
            AlterVertexPosition(ref vertices[tempId], sizeToAdd);
            tempId++;

            for (int j = baseCubeSize.x-1; j >= 0 ; j--)
            {
                sizeToAdd.x = j * xStep;
                AlterVertexPosition(ref vertices[tempId], sizeToAdd);
                tempId++;
            }
            AlterVertexPosition(ref vertices[tempId], sizeToAdd);
            tempId++;

            for (int j = baseCubeSize.z-1; j >= 0; j--)
            {
                sizeToAdd.z = j * zStep;
                AlterVertexPosition(ref vertices[tempId], sizeToAdd);
                tempId++;
            }

        }
        //top and bottom
        for (int i = 0; i < 2; i++)
        {
            sizeToAdd.y = (i == 0 ? yStep * baseCubeSize.y : 0);
            for (int z = 0; z < baseCubeSize.z+1; z++)
            {
                for (int x = 0; x < baseCubeSize.x+1; x++)
                {
                    sizeToAdd.x = sizeToAdd.x = x * xStep;
                    sizeToAdd.z = sizeToAdd.z = z * zStep;
                    AlterVertexPosition(ref vertices[tempId], sizeToAdd);
                    tempId++;
                }

            }
            
        }
    }
    protected void AlterPlaneSize(Vector3 goalSize, Vector3Int baseObjSize, ref Vector3[] vertices)
    {
        int ring = baseObjSize.x+1;

        goalSize -= baseObjSize;

        float xStep = goalSize.x / baseObjSize.x;
        float yStep = goalSize.y / baseObjSize.y;

        Vector3 sizeToAdd = Vector3.zero;

        int tempId;

        //sides
        for (int i = 0; i <= baseObjSize.y; i++)
        {
            tempId = i * ring;

            sizeToAdd = Vector3.zero;
            sizeToAdd.y = i * yStep;

            AlterVertexPosition(ref vertices[tempId], sizeToAdd);
            tempId++;
            for (int j = 1; j <= baseObjSize.x; j++)
            {
                sizeToAdd.x = j * xStep;
                AlterVertexPosition(ref vertices[tempId], sizeToAdd);
                tempId++;
            }

        }
    }

    protected void AddSidePilars(ref Vector3[] vertices, bool backFirewall, bool leftFirewall, bool rightFirewall,float sideDecorWidth ,float sideDecorDepth,ref float addedDecorWidth, Base lastBase = null)
    {
        if(baseObjSize.x != 5 && baseObjSize.z !=5)
            return;

        int ring = CalculateRingSize();
        int tempId = 0;
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

        //front down

        for (int j = 0; j <= baseObjSize.y; j++)
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
            tempId = (ring * j) + baseObjSize.x - 2;
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
            tempId = (ring * j) + baseObjSize.x + baseObjSize.z - 1;
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
            tempId = (ring * j) + baseObjSize.x - 2;
            pos = new Vector3(finalSize.x - (sideDecorWidth - sideDecorDepth), vertices[tempId].y, vertices[tempId].z);
            SetVertexPosition(ref vertices[tempId], pos);
            tempId++;
            pos = new Vector3(finalSize.x - (sideDecorWidth - sideDecorDepth), vertices[tempId].y,
                vertices[tempId].z - sideDecorDepth);
            SetVertexPosition(ref vertices[tempId], pos);
            tempId = (ring * j) + baseObjSize.x * 2 + baseObjSize.z;
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
            tempId++; // = vertices.Length+1 - (baseObjSize.x + 1) * (baseObjSize.z + 1);
            pos = new Vector3(-sideDecorDepth, 0, -sideDecorDepth);
            AlterVertexPosition(ref vertices[tempId], pos);
            tempId++;
            pos = new Vector3(sideDecorWidth - sideDecorDepth, vertices[tempId].y, -sideDecorDepth);
            SetVertexPosition(ref vertices[tempId], pos);
            tempId++;
            pos = new Vector3(sideDecorWidth - sideDecorDepth, vertices[tempId].y, vertices[tempId].z);
            SetVertexPosition(ref vertices[tempId], pos);
            tempId += baseObjSize.x - 4;
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
            tempId += baseObjSize.x - 4;
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
            tempId += baseObjSize.x - 2;
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
            tempId += baseObjSize.x - 2;
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

            tempId += baseObjSize.x - 4;
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
            tempId += baseObjSize.x - 4;
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


    protected abstract Vector2[] GenerateUVs(int verticesLength);

    protected void RemoveVerticesAndTriangles(int removeFrom, int removeTo)
    {
        obj = meshGenerator.RemoveVerticesAndTriangles(obj, removeFrom, removeTo);
    }
    protected void VisualiseVertices(Vector3[] vertices)
    {
        if (verticesDebugger != null)
            verticesDebugger(vertices);
        else
            Debug.LogWarning("Vertices Debugger not connected");
    }

}
