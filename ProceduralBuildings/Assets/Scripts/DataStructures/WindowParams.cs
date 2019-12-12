using System.Collections.Generic;
using UnityEngine;

public class WindowParams : SegmentParams
{
    public ArchedOpeningParams archedOpeningParams;
    public SegmentationParams segmentationParams;
    public SquareOpeningParams squareOpeningParams;
    public PlaneParams glassParams;

    public OpeningStyle openingStyle;

    public Quaternion finalRot;
}
