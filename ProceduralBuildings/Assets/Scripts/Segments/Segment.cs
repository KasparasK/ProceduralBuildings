using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Segment
{
    public Vector3 pos;
    public Vector3 finalSize;
    public Quaternion rot;
    public GameObject obj;
    public List<Segment> attchedSegments;
    
    public Action<Vector3[]> verticesDebugger;
    protected Vector3Int baseCubeSize;

    private MeshGenerator meshGenerator;

    protected int CalculateRingSize()
    {
        return ((baseCubeSize.x + 1) * 2) + ((baseCubeSize.z + 1) * 2);
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
    protected void GenerateBasePlane(Material material, Vector2Int size, string name)
    {
        meshGenerator = new MeshGenerator();
        obj = meshGenerator.GenerateBasePlane(material, size, name);
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
        int ringCount = 0;

        //sides
        for (int i = 0; i <= baseCubeSize.y; i++,ringCount++)
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
