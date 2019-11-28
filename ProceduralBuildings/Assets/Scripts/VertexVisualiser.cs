using System.Collections;
using System.Collections.Generic;
using TMPro;
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
