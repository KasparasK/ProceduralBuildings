using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Window : Segment
{
    private Segment openingBase;
    private Segmentation segmentation;
    private Plane glass;

    public Window(
        Base baseToAttach,
        Material windowMat,
        Material glassMat,
        Material segmentaionMat,
        Vector3 winSize,
        Vector3 pos,
        Quaternion rot,
        List<Vector3> vertSegPositions,
        List<Vector3> horSegPositions,
        Vector3 segmentDimensions,
        Vector3 winFrameDimensions,
        float winPosOffset

    )
    {
        openingBase = new SquareOpening(winSize,windowMat, winFrameDimensions);
        openingBase.obj.transform.parent = baseToAttach.obj.transform;
        openingBase.obj.transform.localPosition = pos;
        openingBase.obj.transform.localRotation = rot;

        segmentation = new Segmentation(openingBase.obj,winSize, segmentDimensions, segmentaionMat,vertSegPositions,horSegPositions);
        glass = new Plane(glassMat,openingBase.obj.transform,winSize);
        glass.obj.transform.localPosition = new Vector3(0, 0, winPosOffset+0.01f);
        glass.obj.transform.localRotation = Quaternion.Euler(Vector3.zero);


    }
}
