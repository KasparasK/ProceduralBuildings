using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoundationParams : SegmentParams
{
    public Vector2Int color = TextureColorIDs.grey;
    public readonly Vector3 minFoundationSize = new Vector3(3, 0.2f, 3.5f);
    public readonly Vector3 maxFoundationSize = new Vector3(3.5f, 0.25f, 4f);

    public FoundationParams()
    {
        finalSize = GetSize();
        finalPos = GetPosition();
        baseObjSize = BaseObjSizes.baseSize;
    }

    Vector3 GetSize()
    {
        Vector3 finalSize = new Vector3(
            Random.Range(minFoundationSize.x, maxFoundationSize.x),
            Random.Range(minFoundationSize.y, maxFoundationSize.y),
            Random.Range(minFoundationSize.z, maxFoundationSize.z)
        );

        return finalSize;
    }
    Vector3 GetPosition()
    {
        return  Vector3.zero;
    }
}
