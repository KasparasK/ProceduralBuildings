using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoofParams : SegmentParams
{
    public float roofThiccness;
    const float zToAdd = 0.3f;
    public Vector2Int color = ColorManager.GetRoofColor();
    public RoofParams(Vector3 atticSize,Vector3 lastBaseSize)
    {
        baseObjSize = BaseObjSizes.roofSize;
        finalSize = GetFinalSize(atticSize, zToAdd, GetThicknessOfRoof());
        finalPos = GetFinalPosition(lastBaseSize,finalSize);
        roofThiccness = GetThicknessOfRoof();

    }
    public Vector3 GetFinalPosition(Vector3 lastBaseSize, Vector3 currSize)
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

     public Vector3 GetFinalSize(Vector3 atticSize, float zToAdd, float roofThiccness)
    {
        float y = atticSize.y * 2;

        float z = atticSize.z + zToAdd;

        return new Vector3(roofThiccness, y, z);
    }
}
