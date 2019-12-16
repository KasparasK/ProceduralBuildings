using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Segment
{
    private Segment openingBase;
    private Plane glass;

    public Door(
        Transform parent,
        Material material,
        DoorParams doorParams
    )
    {
        if (doorParams.openingStyle == OpeningStyle.ARCH)
            openingBase = new ArchedOpening(doorParams.finalSize, material, doorParams.archedOpeningParams);
        else
            openingBase = new SquareOpening(doorParams.finalSize, material, doorParams.squareOpeningParams);

        openingBase.obj.transform.parent = parent;
        openingBase.obj.transform.localPosition = doorParams.finalPos;
        openingBase.obj.transform.localRotation = doorParams.finalRot;


        glass = new Plane(material, openingBase.obj.transform, doorParams.finalSize, doorParams.planeParams);
        glass.obj.transform.localPosition = doorParams.planeParams.finalPos;
        glass.obj.transform.localRotation = doorParams.planeParams.finalRot;


    }
}
