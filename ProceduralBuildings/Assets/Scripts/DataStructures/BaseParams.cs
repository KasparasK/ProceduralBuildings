using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseParams : SegmentParams
{
    public float addedDecorWidth;
    public bool groundFloor;
    public OpeningStyle windowStyle;
    public OpeningStyle doorStyle;
    public bool leftFirewall;
    public bool rightFirewall;
    public bool backFirewall;

    public readonly Vector3 addToLastSize = new Vector3(0.7f, 0.3f, 0.7f);
    public readonly Vector3 minBaseSize = new Vector3(3, 1, 3.5f);

    public readonly float sideDecorWidth = 0.2f;
    public readonly float sideDecorDepth = 0.07f;

    public Vector2Int wallsColor;
    public Vector2Int pillarsColor;


    public BaseParams(bool leftFirewall, bool rightFirewall, bool backFirewall, OpeningStyle windowStyle, OpeningStyle doorStyle)
    {
        groundFloor = true;
        baseObjSize = BaseObjSizes.baseSize;

        this.windowStyle = windowStyle;
        this.doorStyle = doorStyle;
        this.leftFirewall = leftFirewall;
        this.rightFirewall = rightFirewall;
        this.backFirewall = backFirewall;

        SetColors();

        finalSize = GetFinalSize(minBaseSize);
        finalPos = Vector3.zero;
    }
    public BaseParams(Vector3 lastFloorFinalSize,bool leftFirewall, bool rightFirewall, bool backFirewall, OpeningStyle windowStyle)
    {
        groundFloor = false;
        baseObjSize = BaseObjSizes.baseSize;

        this.windowStyle = windowStyle;
        this.leftFirewall = leftFirewall;
        this.rightFirewall = rightFirewall;
        this.backFirewall = backFirewall;

        SetColors();

        finalSize = GetFinalSize(lastFloorFinalSize);
        finalPos = GetFinalPosition(lastFloorFinalSize);
    }

    void SetColors()
    {
        wallsColor = TextureColorIDs.orange;
        pillarsColor = TextureColorIDs.black;

    }

    Vector3 GetFinalPosition(Vector3 lastFloorFinalSize)
    {
        float x = (lastFloorFinalSize.x - finalSize.x) / 2;
        float y =  lastFloorFinalSize.y;
        float z = (lastFloorFinalSize.z - finalSize.z) / 2;

        if (leftFirewall)
            x = 0;
        if (rightFirewall)
            x = (lastFloorFinalSize.x - finalSize.x);
        if (backFirewall)
            z = (lastFloorFinalSize.z - finalSize.z);

        Vector3 finalPosition = new Vector3(x, y, z);
        return finalPosition;
    }
    Vector3 GetFinalSize(Vector3 lastFloorSize)
    {
        Vector3 tempAddToLast = addToLastSize;

        if (leftFirewall && rightFirewall)
        {
            tempAddToLast.x = 0;
        }
        else if (leftFirewall || rightFirewall)
        {
            tempAddToLast.x /= 2;
        }

        if (backFirewall)
        {
            tempAddToLast.z /= 2;
        }

        Vector3 minSize = lastFloorSize;
        Vector3 maxSize = lastFloorSize + tempAddToLast;

        Vector3 finalSize = new Vector3(
            Random.Range(minSize.x, maxSize.x),
            Random.Range(minSize.y, maxSize.y),
            Random.Range(minSize.z, maxSize.z)
        );

        return finalSize;

    }

}
