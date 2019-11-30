using System.Collections.Generic;
using UnityEngine;

public class CombineMeshes {
    
    public GameObject CombineOBJ(List<GameObject> objs)
    {
        MeshFilter[] meshFilters = new MeshFilter[objs.Count];
        for (int j = 0; j < objs.Count; j++)
        {
            meshFilters[j] = objs[j].GetComponent<MeshFilter>();
        }
        CombineInstance[] combine = new CombineInstance[meshFilters.Length];

        int i = 0;
        while (i < meshFilters.Length)
        {
            combine[i].mesh = meshFilters[i].sharedMesh;
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
            meshFilters[i].gameObject.SetActive(false);

            i++;
        }

        objs[0].GetComponent<MeshFilter>().mesh = new Mesh();
        objs[0].GetComponent<MeshFilter>().sharedMesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        objs[0].GetComponent<MeshFilter>().sharedMesh.CombineMeshes(combine);
        objs[0].gameObject.SetActive(true);

        for (i = 1; i < objs.Count; i++)
        {
            Object.DestroyImmediate(objs[i]);
        }

        return objs[0];
    }
}
