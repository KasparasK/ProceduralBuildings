using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chimney : Segment
{
    private const string name = "chimney";

    public Chimney(ChimneyParams chimneyParams, Material material, Transform parent)
    {
        Vector3Int baseObjSize = chimneyParams.baseObjSize;

        GenerateBaseCube(material, baseObjSize, name);
        obj.transform.parent = parent;
        obj.transform.localPosition = chimneyParams.finalPos;

        Mesh mesh = obj.GetComponent<MeshFilter>().sharedMesh;

        Vector3[] vertices = mesh.vertices;

        AlterCubeSize(new Vector3(chimneyParams.finalSize.x, (chimneyParams.finalSize.y-(chimneyParams.finalSize.y/4)) * baseObjSize.y, chimneyParams.finalSize.z), baseObjSize, ref vertices);
        AlterMesh(chimneyParams, baseObjSize, ref vertices);
        mesh.uv = GenerateUVs(vertices.Length, chimneyParams.color);
        mesh.vertices = vertices;
    }

    void AlterMesh(ChimneyParams chimneyParams,Vector3Int baseObjSize,ref Vector3[] vertices)
    {
        int ring = CalculateRingSize(baseObjSize);

        float capExtrusionSize = (chimneyParams.finalSize.y / 4);
        int tempID = ring*2;

        //--------------------------------
        Vector3 posToAdd = new Vector3(-chimneyParams.capExtrusionSizeXZ,-vertices[ring].y, -chimneyParams.capExtrusionSizeXZ);
        for (int i = 0; i < 2; i++)
        {
            AlterVertexPosition(ref vertices[tempID], posToAdd);
            tempID++;
            posToAdd.x = chimneyParams.capExtrusionSizeXZ;
            AlterVertexPosition(ref vertices[tempID], posToAdd);
            tempID++;
            AlterVertexPosition(ref vertices[tempID], posToAdd);
            tempID++;
            posToAdd.z = chimneyParams.capExtrusionSizeXZ;
            AlterVertexPosition(ref vertices[tempID], posToAdd);
            tempID++;
            AlterVertexPosition(ref vertices[tempID], posToAdd);
            tempID++;
            posToAdd.x = -chimneyParams.capExtrusionSizeXZ;
            AlterVertexPosition(ref vertices[tempID], posToAdd);
            tempID++;
            AlterVertexPosition(ref vertices[tempID], posToAdd);
            tempID++;
            posToAdd.z = -chimneyParams.capExtrusionSizeXZ;
            AlterVertexPosition(ref vertices[tempID], posToAdd);
            tempID++;
            posToAdd.y += capExtrusionSize - vertices[ring].y;

        }

        posToAdd.y =  capExtrusionSize + vertices[ring].y*-2;
        posToAdd.x = -chimneyParams.capExtrusionSizeXZ;
        AlterVertexPosition(ref vertices[tempID], posToAdd);
        tempID++;
        posToAdd.x = chimneyParams.capExtrusionSizeXZ;
        AlterVertexPosition(ref vertices[tempID], posToAdd);
        tempID++;
        posToAdd.z = chimneyParams.capExtrusionSizeXZ;
        posToAdd.x = -chimneyParams.capExtrusionSizeXZ;

        AlterVertexPosition(ref vertices[tempID], posToAdd);
        tempID++;
        posToAdd.x = chimneyParams.capExtrusionSizeXZ;

        AlterVertexPosition(ref vertices[tempID], posToAdd);
        //--------------------------------

    }
}
