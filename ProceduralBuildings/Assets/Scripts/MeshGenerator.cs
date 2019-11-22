using Boo.Lang;
using UnityEngine;
using TMPro;
public class Vertex
{
    public Vector3 pos;
    public Vector3[] normals;

    public Vertex(Vector3 pos, int normalsCount)
    {
        this.pos = pos;
        normals = new Vector3[normalsCount];
    }
}

enum RactangleParts
{
    BOT_LEFT_DOWN,
    BOT_RIGHT_DOWN,
    BOT_LEFT_UP,
    BOT_RIGHT_UP,

    TOP_LEFT_DOWN,
    TOP_RIGHT_DOWN,
    TOP_LEFT_UP,
    TOP_RIGHT_UP,

    LEFT_DOWN_SIDE,
    LEFT_UP_SIDE,
    RIGHT_DOWN_SIDE,
    RIGHT_UP_SIDE,
}

public class MeshGenerator {

    int xSize, ySize, zSize;

    private Mesh mesh;
    private Vertex[] vertices;
    int[] triangles;
    int vertex;
 //  private Vector3[] normals;
    private int verticesCount;

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

        int cornerVertices = 8*3;
        int edgeVertices = (xSize + ySize + zSize - 3) * 8;
        int faceVertices = (
            (xSize - 1) * (ySize - 1) +
            (xSize - 1) * (zSize - 1) +
            (ySize - 1) * (zSize - 1)) * 2;

        verticesCount = cornerVertices + edgeVertices + faceVertices;
        Debug.Log(faceVertices);
        vertices = new Vertex[verticesCount];
      //  normals = new Vector3[vertices.Length];

        CreateMesh(vertextNumPref,canvas);
        //Debug.Log("edgeVertices: " + cornerVertices + " edgeVertices: " + edgeVertices + " faceVertices: " + faceVertices);

        return newMesh;
    }

    void CreateMesh(GameObject vertextNumPref, GameObject canvas)
    {
        GenerateVertices();
      
        Vector3[] finalVertices = new Vector3[verticesCount];
        List<Vector3> finalNormals = new List<Vector3>();
        for (int i = 0; i < vertices.Length; i++)
        {
            finalVertices[i] = vertices[i].pos;
            finalNormals.AddRange(vertices[i].normals);
        }
        VisualiseVertices(finalVertices,vertextNumPref, canvas);

        mesh.vertices = finalVertices;
        CreateTriangles();
        mesh.triangles = triangles;
        Debug.Log(mesh.normals.Length);
        mesh.normals = finalNormals.ToArray();

    //    mesh.RecalculateNormals();
    }


    void VisualiseVertices(Vector3[] finalVertices, GameObject vertextNumPref, GameObject canvas)
    {
        for (int i = 0; i < finalVertices.Length; i++)
        {
          GameObject vert =  GameObject.Instantiate(vertextNumPref,canvas.transform);
          vert.GetComponent<TMP_Text>().SetText(i.ToString());
          vert.transform.position = finalVertices[i];
        }
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
                switch (x)
                {
                    case 0: //kampas 
                        SetVertex(v, x, y, 0,new List<Vector3>
                        {
                            Vector3.one
                            //new Vector3(1,0,0), new Vector3(0, 1, 0), new Vector3(0, 0, 1)
                        });
                        break;
                    default:
                        SetVertex(v, x, y, 0, new List<Vector3> { Vector3.one });
                        break;

                }
                //   Debug.Log("[1] "+v);

            }
            for (int z = 0; z <= zSize; z++, v++)
            {
              //  Debug.Log("[2] " + v);

                SetVertex(v, xSize, y, z, new List<Vector3> { Vector3.one });
            }
            for (int x = xSize; x >= 0; x--, v++)
            {
            //    Debug.Log("[3] " + v);

                SetVertex(v, x, y, zSize, new List<Vector3> { Vector3.one });
            }
            for (int z = zSize; z >= 0; z--, v++)
            {
             //   Debug.Log("[4] " + v);

                SetVertex(v, 0, y, z, new List<Vector3> { Vector3.one });
            }
        }

        //dugnas ir virsus
        for (int y = ySize; y >= 0; y -= ySize)
        {
            for (int z = 0; z <= zSize; z++)
            {
                for (int x = 0; x <= xSize; x++, v++)
                {
                 //   Debug.Log("[5]+ " + v);

                    SetVertex(v, x, y, z, new List<Vector3> { Vector3.one });
                }
            }

        }
        


    }
    private void SetVertex(int i, int x, int y, int z,List<Vector3> normalVectors)
    {

            vertices[i]=  new Vertex(new Vector3(x, y, z), normalVectors.Count);
        //   Vector3 inner =vertices[i].pos
        for (int j = 0; j < normalVectors.Count; j++)
        {
            vertices[i].normals[j] = (vertices[i].pos - normalVectors[j]).normalized;
        }
        //   normals[i] = 

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

}

