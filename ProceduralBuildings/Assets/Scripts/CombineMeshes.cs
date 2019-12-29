using UnityEngine;

public class CombineMeshes {
    public void MergeChildren(GameObject gameObject)
    {
        Quaternion oldRot = gameObject.transform.rotation;
        Vector3 oldPos = gameObject.transform.position;

        GameObject child = gameObject.transform.GetChild(0).gameObject;

        Material mat = child.GetComponent<Renderer>().sharedMaterial;

        MeshFilter[] meshFilters = gameObject.GetComponentsInChildren<MeshFilter>();
        CombineInstance[] combine = new CombineInstance[meshFilters.Length];
        int i = 0;
        while (i < meshFilters.Length)
        {
            combine[i].mesh = meshFilters[i].sharedMesh;
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
            meshFilters[i].gameObject.SetActive(false);
            i++;
        }
        if (!gameObject.GetComponent<MeshFilter>())
            gameObject.AddComponent<MeshFilter>();
        if (!gameObject.GetComponent<MeshRenderer>())
            gameObject.AddComponent<MeshRenderer>();
        if (!gameObject.GetComponent<MeshCollider>())
            gameObject.AddComponent<MeshCollider>();

        Mesh finalMesh = new Mesh();
        finalMesh.CombineMeshes(combine);

        gameObject.transform.GetComponent<MeshCollider>().sharedMesh = finalMesh;
        gameObject.transform.GetComponent<MeshFilter>().sharedMesh = finalMesh;

        gameObject.transform.rotation = oldRot;
        gameObject.transform.position = oldPos;
        gameObject.GetComponent<Renderer>().sharedMaterial = mat;
        gameObject.transform.gameObject.SetActive(true);

    }

}
