using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareOpeningParams : SegmentParams
{
    public Vector3 frameDimensions;
    public Vector2Int frameColor = ColorManager.GetOpeningColor();

    public SquareOpeningParams(Vector3 frameDimensions)
    {
        this.frameDimensions = frameDimensions;
    }
}
