using System;
using UnityEngine;

public class Roof : Segment
{
    private const string name = "roof";
    public Roof(Material material, BaseParams lastBaseParams,RoofParams roofParams,Transform parent, Action<Vector3[]> verticesDebugger = null)
   {
       base.verticesDebugger = verticesDebugger;
       Vector3Int baseObjSize = roofParams.baseObjSize;

       GenerateBaseCube(material, baseObjSize,name);
       obj.transform.parent = parent;
       AlterMesh(baseObjSize, lastBaseParams.finalSize, roofParams);

   }

   void AlterMesh(Vector3Int baseCubeSize,Vector3 lastFloorSize,RoofParams roofParams)
   {
       Mesh mesh = obj.GetComponent<MeshFilter>().sharedMesh;

       Vector3[] vertices = mesh.vertices;

       int ring = CalculateRingSize(baseCubeSize);
      
       AlterCubeSize(roofParams.finalSize, baseCubeSize,ref vertices);
       BendRoof(roofParams.finalSize, lastFloorSize, ring, ref vertices);

       mesh.vertices = vertices;
       mesh.RecalculateNormals();
       mesh.uv = GenerateUVs(vertices.Length, roofParams.color);
       obj.transform.localPosition = roofParams.finalPos;
   }
  
  

    void BendRoof(Vector3 goalSize, Vector3 lastBaseSize, int ring, ref Vector3[] vertices)
    {
        //pajundinti pirma zieda vertexu (apatini kairi stogo kampa)
        int tempId = 0;

        Vector3 posToAddRightSide = new Vector3( -goalSize.x, 0, 0);
        Vector3 posToAddLeftSide = new Vector3( -goalSize.x,  goalSize.x / 2, 0);

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
        float xLowerPartOfTop = -(goalSize.x + goalSize.x / 2);
        float heightenTheRoofTop = xLowerPartOfTop / -2;
        posToAddRightSide = new Vector3(lastBaseSize.x / 2 - goalSize.x, xLowerPartOfTop + heightenTheRoofTop, 0);
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

        posToAddRightSide = new Vector3(lastBaseSize.x - goalSize.x , -vertices[tempId].y, 0);
        posToAddLeftSide = new Vector3(lastBaseSize.x + goalSize.x, -vertices[tempId].y+goalSize.x/2, 0);

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
        //bot plane
        posToAddRightSide = new Vector3(-goalSize.x, 0, 0);
        posToAddLeftSide = new Vector3(-goalSize.x, goalSize.x / 2, 0);
        for (int i = 0; i < 2; i++)
        {
            AlterVertexPosition(ref vertices[tempId], posToAddLeftSide);
            tempId++;
            AlterVertexPosition(ref vertices[tempId], posToAddRightSide);
            tempId++;
        }
    }

 
}
