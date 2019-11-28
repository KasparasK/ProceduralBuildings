using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Roof : Segment
{
    private const string name = "roof";

    readonly Vector3Int minBaseSize = new Vector3Int(1, 2, 1);
    const float zToAdd = 0.3f;
    public Roof(Material material, Base parentBase,Attic attic ,Action<Vector3[]> verticesDebugger = null)
    {
        base.verticesDebugger = verticesDebugger;
        baseCubeSize = minBaseSize;

        GenerateBaseCube(material, baseCubeSize,name);
        obj.transform.parent = parentBase.obj.transform;
        AlterMesh(baseCubeSize,parentBase.finalSize, attic.finalSize);
        
    }

    void AlterMesh(Vector3Int baseCubeSize,Vector3 lastFloorSize, Vector3 atticSize)
    {
        Mesh mesh = obj.GetComponent<MeshFilter>().sharedMesh;

        Vector3[] vertices = mesh.vertices;

        int ring = CalculateRingSize();
        finalSize = GetFinalSize(lastFloorSize, atticSize);

        AlterCubeSize(finalSize, baseCubeSize,ref vertices);
        BendRoof(finalSize, lastFloorSize, ring, ref vertices);

        mesh.vertices = vertices;
        mesh.RecalculateNormals();

        pos = obj.transform.localPosition = GetFinalPosition(lastFloorSize, finalSize);
    }
    Vector3 GetFinalPosition(Vector3 lastBaseSize, Vector3 currSize)
    {

        float x = 0;
        float y = lastBaseSize.y ;
        float z = (lastBaseSize.z - currSize.z  )/2;

        Vector3 finalPosition = new Vector3(x, y, z);
        return finalPosition;
    }
    float GetThicknessOfRoof()
    {
        return Random.Range(0.1f, 0.2f);
    }

    Vector3 GetFinalSize(Vector3 lastFloorSize, Vector3 atticSize)
    {
        float length = atticSize.z;
        float halfY = atticSize.y;

        float y = halfY * 2;

        float z = length + zToAdd;

        return  new Vector3(GetThicknessOfRoof(),y,z);
    }

    void BendRoof(Vector3 size, Vector3 lastBaseSize, int ring, ref Vector3[] vertices)
    {
       

        //pajundinti pirma zieda vertextu (apatini kairi stogo kampa)
        int tempId = 0;

        Vector3 posToAddRightSide = new Vector3( -size.x, 0, 0);
        Vector3 posToAddLeftSide = new Vector3( -size.x,  size.x / 2, 0);

        AlterVertexPosition(ref vertices[tempId], posToAddLeftSide);

       for (int i = 0; i < ring / 2; i++)
        {
            tempId++;
            AlterVertexPosition(ref vertices[tempId], posToAddRightSide);
        }
        for (int i = 0; i < (ring / 2) - 1; i++)
        {
            tempId++;
            AlterVertexPosition(ref vertices[tempId], posToAddLeftSide);

        }
        //move middle vertices to right by half width
        float xLowerPartOfTop = -(size.x + size.x / 2);
        float heightenTheRoofTop = xLowerPartOfTop / -2;
        posToAddRightSide = new Vector3(lastBaseSize.x / 2 - size.x, xLowerPartOfTop + heightenTheRoofTop, 0);
        posToAddLeftSide = new Vector3(lastBaseSize.x / 2, heightenTheRoofTop, 0);
        tempId = ring;
        AlterVertexPosition(ref vertices[tempId], posToAddLeftSide);
     
        for (int i = 0; i < ring/2; i++)
        {
            tempId++;
            AlterVertexPosition(ref vertices[tempId], posToAddRightSide);
        }
        for (int i = 0; i < (ring / 2 )-1; i++)
        {
            tempId++;
            AlterVertexPosition(ref vertices[tempId], posToAddLeftSide);

        }
        //move top most vertices to right by all width and down by all height
        tempId = ring*2;

        posToAddRightSide = new Vector3(lastBaseSize.x - size.x , -vertices[tempId].y, 0);
        posToAddLeftSide = new Vector3(lastBaseSize.x + size.x, -vertices[tempId].y+size.x/2, 0);

        AlterVertexPosition(ref vertices[tempId], posToAddLeftSide);

        for (int i = 0; i < ring/2; i++)
        {
            tempId++;
            AlterVertexPosition(ref vertices[tempId], posToAddRightSide);
        }
        for (int i = 0; i < (ring/2)-1; i++)
        {
            tempId++;
            AlterVertexPosition(ref vertices[tempId], posToAddLeftSide);

        }

        //top plane
        tempId = ring * 3;
      
      
        for (int i = 0; i < 2; i++)
        {
            AlterVertexPosition(ref vertices[tempId], posToAddLeftSide);
            tempId++;
            AlterVertexPosition(ref vertices[tempId], posToAddRightSide);
            tempId++;
        }
        posToAddRightSide = new Vector3(-size.x, 0, 0);
        posToAddLeftSide = new Vector3(-size.x, size.x / 2, 0);
        for (int i = 0; i < 2; i++)
        {
            AlterVertexPosition(ref vertices[tempId], posToAddLeftSide);
            tempId++;
            AlterVertexPosition(ref vertices[tempId], posToAddRightSide);
            tempId++;
        }
    }
}
