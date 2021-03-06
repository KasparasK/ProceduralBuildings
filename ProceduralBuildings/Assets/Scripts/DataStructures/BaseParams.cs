﻿using System.Collections.Generic;
using UnityEngine;

public class BaseParams : SegmentParams
{
    public float addedDecorWidth;
    public OpeningStyle windowStyle;
    public OpeningStyle doorStyle;
    public bool leftFirewall;
    public bool rightFirewall;
    public bool backFirewall;

    public readonly Vector3 addToLastSize = new Vector3(0.7f, 0.3f, 0.7f);
    public readonly Vector3 minBaseSize = new Vector3(3, 1, 3.5f);

    public readonly  Vector3 addToFoundationSize = new Vector3(-0.1f,1,-0.1f);

    public readonly float sideDecorWidth = 0.2f;
    public readonly float sideDecorDepth = 0.07f;

    public Vector2Int wallsColor;
    public Vector2Int pillarsColor;

    public int floorNum;

    public List<WindowParams> windowParams;
    public DoorParams doorParams;
    public BaseParams(Vector3 foundationFinalSize,BuildingParams buildingParams, OpeningStyle windowStyle, OpeningStyle doorStyle)
    {
        if (buildingParams.generateCornerPillars)
            baseObjSize = BaseObjSizes.baseSizeWPillars;
        else
            baseObjSize = BaseObjSizes.baseSizeNoPillars;

        floorNum = 0;
        this.windowStyle = windowStyle;
        this.doorStyle = doorStyle;
        
        this.leftFirewall = buildingParams.leftFirewall;
        this.rightFirewall = buildingParams.rightFirewall;
        this.backFirewall = buildingParams.backFirewall;

        SetColors();

        finalSize = GetGroundFloorFinalSize(foundationFinalSize, minBaseSize, addToFoundationSize);
        finalPos = GetGroundFloorFinalPosition(foundationFinalSize);
    }
    public BaseParams(Vector3 lastFloorFinalSize, BuildingParams buildingParams, int floorNum, OpeningStyle windowStyle)
    {
        if (buildingParams.generateCornerPillars)
            baseObjSize = BaseObjSizes.baseSizeWPillars;
        else
            baseObjSize = BaseObjSizes.baseSizeNoPillars;

        this.floorNum = floorNum;
        this.windowStyle = windowStyle;

        this.leftFirewall = buildingParams.leftFirewall;
        this.rightFirewall = buildingParams.rightFirewall;
        this.backFirewall = buildingParams.backFirewall;

        SetColors();

        if(buildingParams.sameSizeFloors)
            addToLastSize = Vector3.zero;

        finalSize = GetFinalSize(lastFloorFinalSize, addToLastSize);
        finalPos = GetFinalPosition(lastFloorFinalSize);
    }

    void SetColors()
    {
        wallsColor = ColorManager.GetBaseColor();
        pillarsColor = ColorManager.GetPillarsColor();

    }
    public Vector3 GetGroundFloorFinalPosition(Vector3 lastFloorFinalSize)
    {
        float x = (lastFloorFinalSize.x - finalSize.x) / 2;
        float y = lastFloorFinalSize.y;
        float z = (lastFloorFinalSize.z - finalSize.z) / 2;

        Vector3 finalPosition = new Vector3(x, y, z);
        return finalPosition;
    }

    public Vector3 GetFinalPosition(Vector3 lastFloorFinalSize)
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

    public Vector3 GetGroundFloorFinalSize(Vector3 foundationSize, Vector3 minBaseSize, Vector3 addToLastSize)
    {
        Vector3 maxSize = foundationSize + addToLastSize;
        Vector3 finalSize = new Vector3(
            maxSize.x,
            Random.Range(minBaseSize.y, maxSize.y),
            maxSize.z

        );

        return finalSize;
    }
    public Vector3 GetFinalSize(Vector3 lastFloorSize,Vector3 addToLastSize)
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

    public void GenerateWindowsParams(BuildingParams buildingParams, Vector3? lastFloorWinSize = null)
    {
        windowParams = new List<WindowParams>();
        OpeningsGenerator openingsGenerator = new OpeningsGenerator();
        openingsGenerator.GenerateOpenings(this, ref windowParams, ref doorParams, buildingParams.rowSameLit, lastFloorWinSize);
    }

    public void GenerateWindowsAndDoorParams(BuildingParams buildingParams, Vector3? lastFloorWinSize = null)
    {
        windowParams = new List<WindowParams>();
        doorParams = new DoorParams();

        OpeningsGenerator openingsGenerator = new OpeningsGenerator();
        openingsGenerator.GenerateOpenings(this, ref windowParams, ref doorParams, buildingParams.rowSameLit, lastFloorWinSize);
    }
}
