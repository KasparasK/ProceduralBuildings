using System.Collections.Generic;
using UnityEngine;

public class WindowParams : SegmentParams
{
    public List<Vector3> vertSegPositions;
    public List<Vector3> horSegPositions;

    public float angle;
    public float windowOffset;

    public Vector3[] outerArcF;
    public Vector3[] outerArcB;
    public Vector3[] innerArcF;
    public Vector3[] innerArcB;

    public OpeningStyle openingStyle;

    public Quaternion rot;

    public Vector3 segmentDimensions;
    public Vector3 winFrameDimensions;

    public Vector2Int glassColor;
    public Vector2Int frameColor = TextureColorIDs.red;
    public Vector2Int segmentsColor = TextureColorIDs.darkBrown;
}
