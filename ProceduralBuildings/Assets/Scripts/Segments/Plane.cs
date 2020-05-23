using UnityEngine;

public class Plane : Segment
{
    private const string name = "plane";
    private readonly PlaneParams planeParams;
    public Plane(Material material, Transform parent, Vector3 size, PlaneParams _planeParams)
    {
        this.planeParams = _planeParams;

        CreateBase(material, parent, size);
    }

    void CreateBase(Material material, Transform parent, Vector3 size)
    {
        Vector3Int baseObjSize = planeParams.baseObjSize;

        GenerateBasePlane(material, baseObjSize, name);

        obj.transform.parent = parent;

        Mesh mesh = obj.GetComponent<MeshFilter>().sharedMesh;

        Vector3[] vertices = mesh.vertices;
        AlterPlaneSize(size, baseObjSize, ref vertices);
        if (planeParams.openingStyle == OpeningStyle.ARCH)
            vertices = ArcThePlane(planeParams.arcPoints, vertices, baseObjSize);
        mesh.uv = GenerateUVs(vertices.Length, planeParams.color);
        mesh.vertices = vertices;
    }

    Vector3[] ArcThePlane(Vector3[] arcPoints, Vector3[] vertices, Vector3Int baseObjSize)
    {
        int tempId = baseObjSize.x+=1;
   
        for (int i = 0; i < arcPoints.Length; i++)
        {
            SetVertexPosition(ref vertices[tempId], arcPoints[i]);
            tempId++;
        }

        return vertices;
    }
}
