using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChimneyParams : SegmentParams
{
    private readonly Vector3 minChimneySize = new Vector3(0.4f,1,0.4f);
    private readonly Vector3 maxChimneySize = new Vector3(0.5f,2f,0.5f);

    public readonly float capExtrusionSizeXZ = 0.05f;

    public Vector2Int color = ColorManager.GetChimneyColor();
    public ChimneyParams(RoofParams roofParams, BaseParams lastBaseParams)
    {
        finalSize = GetFinalSize();
        finalPos = GetFinalPos(roofParams, lastBaseParams.finalSize);
        baseObjSize = BaseObjSizes.chimneySize;

    }

    Vector3 GetFinalPos(RoofParams roofParams, Vector3 baseFinalSize )
    {
        float x = baseFinalSize.x/4;
        float y = roofParams.finalSize.y / 4 + roofParams.roofThiccness;
        float z = roofParams.finalSize.z / 2;

        return  new Vector3(x,y,z);
    }

    Vector3 GetFinalSize()
    {
        return  new Vector3(
            Random.Range(minChimneySize.x,maxChimneySize.x),
            Random.Range(minChimneySize.y, maxChimneySize.y),
            Random.Range(minChimneySize.z, maxChimneySize.z)

            );
    }
}
