using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoundationParams : SegmentParams
{
    public Vector2Int color = TextureColorIDs.grey;
    public FoundationParams(BaseParams firstFloor)
    {
        finalSize = GetSize(firstFloor.finalSize);
        finalPos = GetPosition(firstFloor,finalSize);
        baseObjSize = BaseObjSizes.baseSize;
    }

    Vector3 GetSize(Vector3 lastFloorSize)
    {
        return new Vector3(lastFloorSize.x + 0.1f, lastFloorSize.y / 4, lastFloorSize.z + 0.1f);
    }
    Vector3 GetPosition(BaseParams firstFloor, Vector3 currSize)
    {
        float x = (firstFloor.finalSize.x - currSize.x) / 2;
        float y = 0;
        float z = (firstFloor.finalSize.z - currSize.z) / 2;

        if (firstFloor.leftFirewall)
            x = 0;
        if (firstFloor.rightFirewall)
            x = (firstFloor.finalSize.x - currSize.x);
        if (firstFloor.backFirewall)
            z = (firstFloor.finalSize.z - currSize.z);

        Vector3 finalPosition = new Vector3(x, y, z);
        return finalPosition;
    }
}
