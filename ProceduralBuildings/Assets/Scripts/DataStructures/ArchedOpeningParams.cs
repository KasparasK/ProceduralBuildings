using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArchedOpeningParams : SegmentParams
{
    public Vector3[] outerArcF;
    public Vector3[] outerArcB;
    public Vector3[] innerArcF;
    public Vector3[] innerArcB;
    public Vector3 frameDimensions;

    public Vector2Int frameColor = TextureColorIDs.lightBrown;

    public ArchedOpeningParams(Vector3[] outerArcF, Vector3[] outerArcB, Vector3[] innerArcF, Vector3[] innerArcB, Vector3 frameDimensions)
    {
        this.outerArcF = outerArcF;
        this.outerArcB = outerArcB;
        this.innerArcF = innerArcF;
        this.innerArcB = innerArcB;
        this.frameDimensions = frameDimensions;
    }
}
