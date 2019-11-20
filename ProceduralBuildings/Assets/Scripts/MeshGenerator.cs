using UnityEngine;
using TMPro;

public class MeshGenerator {

    int xSize, ySize, zSize;

    private Mesh mesh;
    private Vector3[] vertices;
    int[] triangles;
    int vertex;


    public GameObject GenerateRectangle(Material material,int _xSize, int _ySize, int _zSize, GameObject vertextNumPref,GameObject canvas)
    {
        xSize = _xSize;
        ySize = _ySize;
        zSize = _zSize;

        GameObject newMesh = new GameObject();
        newMesh.AddComponent<MeshFilter>().mesh = mesh = new Mesh();
        newMesh.AddComponent<MeshRenderer>().sharedMaterial= material;

        mesh.name = "ProceduralRectangle";
        newMesh.name = "ProceduralRectangle";

        int cornerVertices = 8;
        int edgeVertices = (xSize + ySize + zSize - 3) * 4;
        int faceVertices = (
            (xSize - 1) * (ySize - 1) +
            (xSize - 1) * (zSize - 1) +
            (ySize - 1) * (zSize - 1)) * 2;
        vertices = new Vector3[cornerVertices + edgeVertices + faceVertices];

      
        CreateMesh(vertextNumPref,canvas);
        //Debug.Log("edgeVertices: " + cornerVertices + " edgeVertices: " + edgeVertices + " faceVertices: " + faceVertices);

        return newMesh;
    }

    void CreateMesh(GameObject vertextNumPref, GameObject canvas)
    {
        GenerateVertices();
        VisualiseVertices(vertextNumPref, canvas);
       mesh.vertices = vertices;
        CreateTriangles();
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }

    void VisualiseVertices(GameObject vertextNumPref, GameObject canvas)
    {
        for (int i = 0; i < vertices.Length; i++)
        {
          GameObject vert =  GameObject.Instantiate(vertextNumPref,canvas.transform);
          vert.GetComponent<TMP_Text>().SetText(i.ToString());
          vert.transform.position = vertices[i];
        }
    }

    void GenerateVertices()
    {
        int v = 0;

        //ziedu sluoksniai
        for (int y = 0; y <= ySize; y++)
        {
            //sukuriamas spirale pirmas ziedas
            for (int x = 0; x <= xSize; x++)
            {
                vertices[v++] = new Vector3(x, y, 0);
                
            }
            for (int z = 1; z <= zSize; z++)
            {
                vertices[v++] = new Vector3(xSize, y, z);
            }
            for (int x = xSize - 1; x >= 0; x--)
            {
                vertices[v++] = new Vector3(x, y, zSize);
            }
            for (int z = zSize - 1; z > 0; z--)
            {
                vertices[v++] = new Vector3(0, y, z);
            }
        }

        //dugnas ir virsus
        for (int y = ySize; y >= 0; y -= ySize)
        {
            for (int z = 1; z < zSize; z++)
            {
                for (int x = 1; x < xSize; x++)
                {
                    vertices[v] = new Vector3(x, y, z);
                  //  Debug.Log(v + " y: " + y + " x: " + x + " z: " + z);
                    v++;
                }
            }

        }


    }

    void CreateTriangles()
    {
        vertex = 0;
        int ring = xSize * 2 + zSize * 2;
        int t = 0;
        t = GenerateSideTriangles(ring, t);
        t = GenerateTopAndBottomTriangles(ring, t);
        t = TopPlaneRing(ring, t);
        t = BotPlaneRing(ring, t);

    }

    int GenerateSideTriangles(int ring,int t)
    {
        triangles = new int[(xSize * ySize + xSize * zSize + ySize * zSize) * 12];
        
        for (int y = 0; y < ySize; y++, vertex++)
        {
            for (int x = 0; x <= ring - 2; x++, vertex++)
            {
        
            SplitQuad(ref t, 
                    vertex, 
                    ring + vertex, 
                    (vertex + 1 ), 
                    ring + vertex + 1 );
            }
            //paskuti quad reik jungti su pradiniais vertexais
            SplitQuad( ref t,
                vertex,
                ring + vertex,
                vertex + 1 - ring,
                vertex + 1 );
        }

        return t;
    }

    int GenerateTopAndBottomTriangles(int ring, int t)
    {

        int botVertStart = vertex = ((ySize + 1) * 2 * xSize) + ((ySize + 1) * 2 * zSize);

        //virsus (flipinta viskas, palyginus su apacia)
        for (int z = 0; z < (zSize - 2); z++)
        {
            for (int x = 0; x < (xSize - 2); x++)
            {
               // Debug.Log("0: " + (botVertStart + xSize - 1) + " 1: " + botVertStart + " 2: " + (botVertStart + xSize) + " 3: " + (botVertStart + 1));
                SplitQuad(ref t, vertex + xSize, vertex + 1, vertex + xSize - 1,vertex);
                vertex++;
            }
            vertex++;
        }
        vertex += ((zSize - 1)*(xSize-1)) - (vertex - botVertStart);
        
        //apacia
        for (int z = 0; z < (zSize - 2); z++)
        {
            for (int x = 0; x < (xSize - 2); x++)
            {
                // Debug.Log("0: " + (botVertStart + xSize - 1) + " 1: " + botVertStart + " 2: " + (botVertStart + xSize) + " 3: " + (botVertStart + 1));
                SplitQuad(ref t, vertex + xSize - 1, vertex, vertex + xSize, vertex + 1);
                vertex++;
            }
            vertex++;
        }


        //-----------------------------------------


        return t;

    }

    int TopPlaneRing(int ring, int t)
    {
        int v = ring * ySize;

          for (int x = 0; x < xSize - 1; x++, v++)
          {
              SplitQuad(ref t, v, v + ring - 1,v + 1, v + ring);
   
          }

          SplitQuad(ref t, v, v + ring - 1,v + 1, v + 2);
     
          //-----------------------------------------

          int vMin = ring * (ySize + 1) - 1;
          int vMid = vMin + 1;
          int vMax = v + 2;

          for (int z = 1; z < zSize - 1; z++, vMin--, vMid++, vMax++)
          {
              SplitQuad(ref t, vMin, vMin-1, vMid, vMid + xSize - 1);
              

              vMid += xSize - 2;
             // Debug.Log(" vMin: " + vMin + " vMid: " + vMid + " vMax: " + vMax);

              SplitQuad(ref t, vMid , vMid + xSize - 1, vMax , vMax +1);
          
          }
          //-----------------------------------------
          int vTop = vMin - 2;
          // Debug.Log("vTop: " + vTop + " vMin: " + vMin + " vMid: " + vMid + " vMax: " + vMax);

          SplitQuad(ref t, vMin, vMin - 1, vMid, vTop);
        

          for (int x = 1; x < xSize - 1; x++, vTop--, vMid++)
          {
              SplitQuad(ref t, vMid, vTop, vMid + 1, vTop - 1);
        

          }
          SplitQuad(ref t, vMid, vTop, vTop - 2, vTop - 1);
    

          //-----------------------------------------

          return t;
    }

    int BotPlaneRing(int ring, int t)
    {
        int v = 1;
        int vMid = vertices.Length - (xSize - 1) * (zSize - 1);
        // virsutine eile
        SplitQuad(ref t, ring - 1, 0, vMid, 1);
        for (int x = 1; x < xSize - 1; x++, v++, vMid++)
        {
            SplitQuad(ref t, vMid, v, vMid+1, v + 1);
        }
        SplitQuad(ref t, vMid, v , v+2, v + 1);
        //-----------------------------------------

        int vMin = ring - 2;
        vMid -= xSize - 2;
        int vMax = v + 2;
        for (int z = 1; z < zSize - 1; z++, vMin--, vMid++, vMax++)
        {
            SplitQuad(ref t, vMin, vMin + 1, vMid+xSize-1, vMid);
            vMid += (xSize - 2);
            SplitQuad(ref t, vMid + xSize - 1, vMid, vMax+1, vMax);
        }
        //-----------------------------------------

        int vTop = vMin - 1;

        SplitQuad(ref t, vMin, vMin+1, vMin-1, vMid);
        for (int x = 1; x < xSize - 1; x++, vTop--, vMid++)
        {
            SplitQuad(ref t, vTop, vMid, vTop-1, vMid + 1);
        }
       // Debug.Log("vMin: " + vMin + " vMid: " + vMid + " vMax: " + vMax + " vTop: " + vTop);

        SplitQuad(ref t, vTop, vMid, vTop - 1, vTop - 2);
        //-----------------------------------------

        return t;
    }

    void SplitQuad(ref int t, int v00, int v01, int v10, int v11 )
    {
        triangles[t] = v00;
        triangles[t + 1] = triangles[t + 4] = v01;
        triangles[t + 2] = triangles[t + 3] = v10;
        triangles[t + 5] = v11;

        t += 6; 
    }
/*
    private void OnDrawGizmos()
    {
        if (vertices == null)
        {
            return;
        }
        for (int i = 0; i < vertices.Length; i++)
        {
            Gizmos.color = Color.black;
            Gizmos.DrawSphere(vertices[i], 0.1f);
            Gizmos.color = Color.yellow;
            Gizmos.DrawRay(vertices[i], normals[i]);
    }
    }*/
}

