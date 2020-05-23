using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chimney : Segment
{
    private const string name = "chimney";
    private ChimneyParams chimneyParams;
    public Chimney(ChimneyParams _chimneyParams, Material material, Transform parent)
    {
        chimneyParams = _chimneyParams;
        CreateBase(material, parent);
    }

    void CreateBase(Material material, Transform parent)
    {
        Vector3Int baseObjSize = chimneyParams.baseObjSize;

        GenerateBaseCube(material, baseObjSize, name);
        obj.transform.parent = parent;
        obj.transform.localPosition = chimneyParams.finalPos;

        Mesh mesh = obj.GetComponent<MeshFilter>().sharedMesh;

        Vector3[] vertices = mesh.vertices;

        AlterCubeSize(new Vector3(chimneyParams.finalSize.x, (chimneyParams.finalSize.y - (chimneyParams.finalSize.y / 4)) * baseObjSize.y, chimneyParams.finalSize.z), baseObjSize, ref vertices);
        vertices = AlterMesh( baseObjSize, vertices);
        mesh.uv = GenerateUVs(vertices.Length, chimneyParams.color);
        mesh.vertices = vertices;
    }
    Vector3[] AlterMesh(Vector3Int baseObjSize,Vector3[] vertices)
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

        return vertices;
    }
}
