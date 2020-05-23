using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Segment
{
    private Segment openingBase;
    private Plane planks;
    private DoorParams doorParams;
    public Door(
        Transform parent,
        Material material,
        DoorParams _doorParams
    )
    {
        doorParams = _doorParams;


        CreateDoor(parent, material);
    }

    void CreateDoor(Transform parent, Material material)
    {

        if (doorParams.openingStyle == OpeningStyle.ARCH)
            openingBase = new ArchedOpening(doorParams.finalSize, material, doorParams.archedOpeningParams);
        else
            openingBase = new SquareOpening(doorParams.finalSize, material, doorParams.squareOpeningParams);

        openingBase.obj.transform.parent = parent;
        openingBase.obj.transform.localPosition = doorParams.finalPos;
        openingBase.obj.transform.localRotation = doorParams.finalRot;


        planks = new Plane(material, openingBase.obj.transform, doorParams.finalSize, doorParams.planeParams);
        planks.obj.transform.localPosition = doorParams.planeParams.finalPos;
        planks.obj.transform.localRotation = doorParams.planeParams.finalRot;
    }
}
