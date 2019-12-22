using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtticParams : SegmentParams
{
    public Vector2Int color = ColorManager.GetBaseColor();
    public AtticParams(Vector3 lastFloorSize)
    {
        baseObjSize = BaseObjSizes.atticSize;
        finalSize = GetFinalSize(lastFloorSize);
        finalPos = GetFinalPosition(lastFloorSize, finalSize);
    }

    Vector3 GetFinalPosition(Vector3 lastBaseSize, Vector3 currSize)
    {

        float x = 0;
        float y = lastBaseSize.y;
        float z = (lastBaseSize.z - currSize.z) / 2;

        Vector3 finalPosition = new Vector3(x, y, z);
        return finalPosition;
    }

    Vector3 GetFinalSize(Vector3 lastFloorSize)
    {

        Vector3 finalSize = new Vector3(
            lastFloorSize.x,
            UnityEngine.Random.Range(lastFloorSize.x / 2.5f, lastFloorSize.x * 0.9f),
            lastFloorSize.z
        );

        return finalSize;

    }
}
