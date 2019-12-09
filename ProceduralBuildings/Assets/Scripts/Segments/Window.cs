using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Window : Segment
{
    private Segment openingBase;
    private Segmentation segmentation;
    private Plane glass;

    public Window(
        Base baseToAttach,
        Material material,
        Vector3 winSize,
        Vector3 pos,
        Quaternion rot,
        WindowParams winParams,
        Vector3 segmentDimensions,
        Vector3 winFrameDimensions,
        float winPosOffset

    )
    {
        if(winParams.openingStyle == OpeningStyle.ARCH)
            openingBase = new ArchedOpening(winSize,material,winFrameDimensions,TextureColorIDs.red, winParams); 
        else
            openingBase = new SquareOpening(winSize, material, winFrameDimensions, TextureColorIDs.grey);

        openingBase.obj.transform.parent = baseToAttach.obj.transform;
        openingBase.obj.transform.localPosition = pos;
        openingBase.obj.transform.localRotation = rot;

        segmentation = new Segmentation(openingBase.obj,winSize, segmentDimensions, material, winParams.vertSegPositions, winParams.horSegPositions);
        if (winParams.openingStyle == OpeningStyle.ARCH)
            glass = new Plane(material, openingBase.obj.transform,winSize, TextureColorIDs.lightBlue, winParams.innerArcF);
        else
            glass = new Plane(material, openingBase.obj.transform, winSize, TextureColorIDs.lightBlue); //new Plane(material, openingBase.obj.transform, winSize,openingBase.innerArcF, TextureColorIDs.lightBlue);

        glass.obj.transform.localPosition = new Vector3(winSize.x, 0, winPosOffset+0.01f);
        glass.obj.transform.localRotation = Quaternion.Euler(new Vector3(0,180,0));


    }


    protected override Vector2[] GenerateUVs(int verticesLength)
    {
        throw new System.NotImplementedException();
    }
}
