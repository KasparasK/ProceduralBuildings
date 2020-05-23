using System;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using Boo.Lang;
using UnityEngine;
using TMPro;
using UnityEditor;

public class MeshGenerator {

    int xSize, ySize, zSize;

    private Mesh mesh;
    private Vector3[] vertices;
    int[] triangles;
    int vertex;
    private int verticesCount;

    public GameObject GenerateBaseRectangle(Material material,Vector3Int size,string name)
    {
        xSize = size.x;
        ySize = size.y;
        zSize = size.z;

        GameObject baseRectangle = new GameObject();
        baseRectangle.AddComponent<MeshFilter>().mesh = mesh = new Mesh();
        baseRectangle.AddComponent<MeshRenderer>().sharedMaterial= material;

        mesh.name = name;
        baseRectangle.name = name;

        int cornerVertices = 8*3;
        int edgeVertices = (xSize + ySize + zSize - 3) * 8;
        int faceVertices = (
            (xSize - 1) * (ySize - 1) +
            (xSize - 1) * (zSize - 1) +
            (ySize - 1) * (zSize - 1)) * 2;

        verticesCount = cornerVertices + edgeVertices + faceVertices;
        vertices = new Vector3[verticesCount];

        CreateMesh();
        baseRectangle.isStatic = true;
        return baseRectangle;
    }
    public GameObject GenerateBasePlane(Material material, Vector3Int size, string name)
    {
        xSize = size.x;
        ySize = size.y;
        zSize = 0;

        GameObject basePlane = new GameObject();
        basePlane.AddComponent<MeshFilter>().mesh = mesh = new Mesh();
        basePlane.AddComponent<MeshRenderer>().sharedMaterial = material;

        mesh.name = name;
        basePlane.name = name;

        verticesCount = (xSize + 1) * (ySize + 1);
        vertices = new Vector3[verticesCount];

        CreateMesh();

        return basePlane;
    }
    void CreateMesh()
    {
        if(zSize > 0)
            GenerateVertices();
        else
            GenerateVerticesPlane();

        mesh.vertices = vertices;
        CreateTriangles();

        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }
    void GenerateVertices()
    {
        int v = 0;
        //ziedu sluoksniai
        for (int y = 0; y <= ySize; y++)
        {
            //sukuriamas spirale pirmas ziedas
            for (int x = 0; x <= xSize; x++, v++)
            {
                SetVertex(v, x, y, 0);
            }
            for (int z = 0; z <= zSize; z++, v++)
            {
                SetVertex(v, xSize, y, z);
            }
            for (int x = xSize; x >= 0; x--, v++)
            {
                SetVertex(v, x, y, zSize);
            }
            for (int z = zSize; z >= 0; z--, v++)
            {
                SetVertex(v, 0, y, z);
            }
        }
        //dugnas ir virsus
        for (int y = ySize; y >= 0; y -= ySize)
        {
            for (int z = 0; z <= zSize; z++)
            {
                for (int x = 0; x <= xSize; x++, v++)
                {

                    SetVertex(v, x, y, z);
                }
            }
        }
    }
    void GenerateVerticesPlane()
    {
        int v = 0;

        //ziedu sluoksniai
        for (int y = 0; y <= ySize; y++)
        {
            //sukuriamas spirale pirmas ziedas
            for (int x = 0; x <= xSize; x++, v++)
            {
                SetVertex(v, x, y, 0);
            }
        }
    }
    private void SetVertex(int i, int x, int y, int z)
    {

            vertices[i]=  new Vector3(x, y, z);

    }
    void CreateTriangles()
    {
        vertex = 0;
     
        int t = 0;
        if (zSize > 0)
        {
            int ring = ((xSize + 1) * 2) + ((zSize + 1) * 2);
            t = GenerateSideTriangles(ring, t);
            GenerateTopAndBottomTriangles(ring, t);
        }
        else
        {
            int ring = xSize + 1;
            GeneratePlaneTriangles(ring, t);
        }

    }
    int GenerateSideTriangles(int ring,int t)
    {
        triangles = new int[
          (zSize*ySize* 12) + (xSize * ySize * 12) + (xSize * zSize * 12)
        ];
        for (int y = 0; y < ySize; y++)
        {
            for (int i = 0; i < 2; i++)
            {
                for (int x = 0; x < xSize; x++, vertex++)
                {
                    SplitQuad(ref t,
                        vertex,
                        ring + vertex,
                        (vertex + 1),
                        ring + vertex + 1);
                }

                vertex++;

                for (int z = 0; z < zSize; z++, vertex++)
                {
                    SplitQuad(ref t,
                        vertex,
                        ring + vertex,
                        (vertex + 1),
                        ring + vertex + 1);
                }
                vertex++;

            }

        }

        return t;
    }
    int GeneratePlaneTriangles(int ring, int t)
    {
        triangles = new int[xSize * ySize * 6];
        for (int y = 0; y < ySize; y++)
        {
            for (int x = 0; x < xSize; x++, vertex++)
            {
                //verteksu prisikirimas poligonui
                SplitQuad(ref t,
                    vertex,
                    ring + vertex,
                    (vertex + 1),
                    ring + vertex + 1);
            }

            vertex++;
        }

        return t;
    }
    int GenerateTopAndBottomTriangles(int ring, int t)
    {
        vertex = ring * (ySize+1);

        //virsus (flipinta viskas, palyginus su apacia)
        for (int z = 0; z < (zSize); z++)
        {
            for (int x = 0; x < (xSize); x++)
            {
                    SplitQuad(ref t, vertex, vertex + xSize + 1, vertex + 1, vertex + xSize + 2);

                vertex++;
            }
            vertex++;
        }

        vertex += xSize + 1;
        //apacia
        for (int z = 0; z < (zSize ); z++)
        {
            for (int x = 0; x < (xSize); x++)
            {
                 SplitQuad(ref t, vertex + xSize +1, vertex, vertex + xSize +2, vertex + 1);

                 vertex++;
            }
            vertex++;
        }

        //-----------------------------------------

        return t;
    }

    void SplitQuad(ref int t, int v00, int v01, int v10, int v11,bool debug = false )
    {
        if(debug)
            Debug.Log("0: " + v00 + " 1: " + v01 + " 2: " + v10 + " 3: " + v11);

        triangles[t] = v00;
        triangles[t + 1] = triangles[t + 4] = v01;
        triangles[t + 2] = triangles[t + 3] = v10;
        triangles[t + 5] = v11;

        t += 6; 
    }

    public GameObject RemoveVerticesAndTriangles(GameObject obj, int removeFrom, int removeTo)
    {

        MeshFilter meshFilter = obj.GetComponent<MeshFilter>();
        Mesh newMesh = new Mesh();
        Mesh oldMesh = meshFilter.sharedMesh;

        List<Vector3> vertices = new List<Vector3>(oldMesh.vertices);
        List<int> tris = new List<int>(oldMesh.triangles);
    

        //pradžioj ištrinti nereikalingas vertexu nuorodas taip panaikinant trikampius
        for (int i = removeFrom; i <= removeTo; i++)
        {
            tris.RemoveAll(x => x == i);
        }
        //sumažinti vertexu masyvo nuorodas, kadangi trikampių masyvas sumažejo
        for (int i = 0; i < tris.Count; i++)
        {
            if (tris[i] > removeTo)
                tris[i] -= removeTo - removeFrom + 1;
        }
        //ištrinti nebenaudojamus vertexus
        for (int i = removeTo; i >= removeFrom; i--)
        {
            vertices.RemoveAt(i);
        }

        newMesh.vertices = vertices.ToArray();
        newMesh.triangles = tris.ToArray();
        newMesh.RecalculateNormals();
        meshFilter.sharedMesh = newMesh;

        return obj;
    }
}


