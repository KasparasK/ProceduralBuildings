using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoundationParams : SegmentParams
{
    public Vector2Int color = ColorManager.GetFoundationColor();
    public readonly Vector3 minFoundationSize = new Vector3(3, 0.2f, 3.5f);
    public readonly Vector3 maxFoundationSize = new Vector3(3.5f, 0.25f, 4f);
    public const float sideDecorWidth = 0.3f;
    public const float sideDecorDepth = 0.08f;
    public float addedDecorWidth;
    public FoundationParams(BuildingParams buildingParams)
    {
        if (buildingParams.useCustomBuildingSize)
            finalSize = new Vector3(buildingParams.customBuildingSizeX, minFoundationSize.y, buildingParams.customBuildingSizeZ);
        else
            finalSize = GetFinalSize();

        finalPos = GetFinalPosition();
        if (buildingParams.generateCornerPillars)
            baseObjSize = BaseObjSizes.baseSizeWPillars;
        else
            baseObjSize = BaseObjSizes.baseSizeNoPillars;
    }

    Vector3 GetFinalSize()
    {
        Vector3 finalSize = new Vector3(
            Random.Range(minFoundationSize.x, maxFoundationSize.x),
            Random.Range(minFoundationSize.y, maxFoundationSize.y),
            Random.Range(minFoundationSize.z, maxFoundationSize.z)
        );

        return finalSize;
    }
    Vector3 GetFinalPosition()
    {
        return  Vector3.zero;
    }
}
