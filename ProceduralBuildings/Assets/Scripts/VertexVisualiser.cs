using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public class VertexVisualiser : MonoBehaviour
{
    public GameObject canvas;
    public GameObject canvasPref;
    public GameObject vertexNuPref;

    public void VisualiseVertices(Vector3[] finalVertices)
    {
        if (canvas != null)
            DestroyImmediate(canvas);

        canvas = Instantiate(canvasPref, Vector3.zero, Quaternion.identity);


        for (int i = 0; i < finalVertices.Length; i++)
        {
          GameObject vert =  Object.Instantiate(vertexNuPref, canvas.transform);
          vert.GetComponent<TMP_Text>().SetText(i.ToString());
          vert.transform.localPosition = finalVertices[i];
        }
    }



}
/*
[CustomEditor(typeof(MeshFilter))]
public class VertexVisualiser : Editor
{

       private Mesh mesh;

        void OnEnable()
        {
            MeshFilter mf = target as MeshFilter;
            if (mf != null)
            {
                mesh = mf.sharedMesh;
            }
        }

        void OnSceneGUI()
        {
            if (mesh == null)
            {
                return;
            }

            for (int i = 0; i < mesh.vertexCount; i++)
            {
                Handles.matrix = (target as MeshFilter).transform.localToWorldMatrix;
                Handles.color = Color.orange;
                Handles.DrawLine(
                    mesh.vertices[i],
                    mesh.vertices[i] + mesh.normals[i]);
            }
        }
        

}
*/