using UnityEngine;

public class PlaneParams : SegmentParams
{
    public Vector2Int color;
    public Vector3[] arcPoints;
    public OpeningStyle openingStyle;
    public Quaternion finalRot;
    public PlaneParams(OpeningStyle openingStyle, Vector2Int color, Vector3Int baseObjSize, Vector3[] arcPoints,
        Vector3 frameFinalSize, float depthOffset)
    {
        this.openingStyle = openingStyle;
        this.color = color;
        this.baseObjSize = baseObjSize;
        this.arcPoints = arcPoints;
        finalPos = GetFinalPos(frameFinalSize, depthOffset);
        finalRot = GetFinalRot();
    }
    public PlaneParams(OpeningStyle openingStyle, Vector2Int color, Vector3Int baseObjSize, Vector3 frameFinalSize,
        float depthOffset)
    {
        this.openingStyle = openingStyle;
        this.color = color;
        this.baseObjSize = baseObjSize;
        finalPos = GetFinalPos(frameFinalSize, depthOffset);
        finalRot = GetFinalRot();
    }

    private Vector3 GetFinalPos(Vector3 frameFinalSize,float depthOffset)
    {
        return new Vector3(frameFinalSize.x, 0, depthOffset + 0.01f);
    }

    private Quaternion GetFinalRot()
    {
        return Quaternion.Euler(new Vector3(0, 180, 0));
    }

}
