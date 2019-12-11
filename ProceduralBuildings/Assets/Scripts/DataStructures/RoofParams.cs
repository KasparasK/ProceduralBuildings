using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoofParams : SegmentParams
{
    public float roofThiccness;
    const float zToAdd = 0.3f;
    public Vector2Int color = TextureColorIDs.red;
    public RoofParams(Vector3 atticSize,Vector3 lastBaseSize)
    {
        baseObjSize = BaseObjSizes.roofSize;
        finalSize = GetFinalSize(atticSize);
        finalPos = GetFinalPosition(lastBaseSize,finalSize);
        roofThiccness = GetThicknessOfRoof();

    }
    Vector3 GetFinalPosition(Vector3 lastBaseSize, Vector3 currSize)
    {

        float x = 0;
        float y = lastBaseSize.y;
        float z = (lastBaseSize.z - currSize.z) / 2;

        Vector3 finalPosition = new Vector3(x, y, z);
        return finalPosition;
    }
    float GetThicknessOfRoof()
    {
        return Random.Range(0.1f, 0.2f);
    }

    Vector3 GetFinalSize(Vector3 atticSize)
    {
        float length = atticSize.z;
        float halfY = atticSize.y;

        float y = halfY * 2;

        float z = length + zToAdd;

        return new Vector3(GetThicknessOfRoof(), y, z);
    }
}
