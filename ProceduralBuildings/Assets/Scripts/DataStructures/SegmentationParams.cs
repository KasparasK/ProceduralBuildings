using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SegmentationParams : SegmentParams
{
    public List<Vector3> vertSegPositions;
    public List<Vector3> horSegPositions;
    public Vector3 segmentDimensions;
    public Vector2Int segmentsColor = ColorManager.GetSegmentColor();

    public SegmentationParams(List<Vector3> vertSegPositions, List<Vector3> horSegPositions, Vector3 segmentDimensions)
    {
        this.vertSegPositions = vertSegPositions;
        this.horSegPositions = horSegPositions;
        this.segmentDimensions = segmentDimensions;
    }
}
