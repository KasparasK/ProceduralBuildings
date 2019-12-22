using System.Collections.Generic;
using UnityEngine;

public class OpeningsGenerator : MonoBehaviour
{
    private const float minDistanceBetweenWindows = 0.2f;
    private const float maxDistanceBetweenWindows = 0.5f;

    private readonly Vector3 minWinDimensions = new Vector3(0.4f, 0.5f, 0.1f);
    private readonly Vector3 maxWinDimensions = new Vector3(0.7f, 0.9f, 0.2f);

    private readonly Vector3 frameDimensions = new Vector3(0.05f, 0.05f, 0.1f);

    private readonly Vector3 segmentDimensions = new Vector3(0.04f, 0.04f, 0.04f);

    private const float minDistanceBetweenSegments = 0.1f;
    private const float maxDistanceBetweenSegments = 0.2f;
    private const float windowOffset = 0.02f; //kiek islindes

    private readonly float minDoorX = 0.5f;
    private readonly float maxDoorX = 0.9f;


    private List<Vector3> vertSegPositions;
    private List<Vector3> horSegPositions;
    private float angle;
    private Vector3[] outerArcF;
    private Vector3[] outerArcB;
    private Vector3[] innerArcF;
    private Vector3[] innerArcB;
    public void GenerateOpenings(BaseParams baseParams,ref List<WindowParams> windowParams,ref DoorParams doorParams,bool rowSameLit,Vector3 ?lastFloorWinSize = null)
    {
        List<Quaternion> rotations = new List<Quaternion>();
        List<Vector3> positions = new List<Vector3>();
        Vector3 finalSize;

        if (baseParams.floorNum < 2)
            finalSize = RandomiseWindowSize(baseParams.finalSize);
        else
            finalSize = (Vector3)lastFloorWinSize;

        GenerateWindowsPositions(baseParams.finalSize, finalSize, baseParams.leftFirewall, baseParams.rightFirewall, baseParams.backFirewall, ref positions, ref rotations);

        Vector2Int glassColor = RandomiseWindowColor();

        if (baseParams.floorNum == 0)
        {
            int r = Random.Range(0, positions.Count);

            Vector3 doorFinalSize = new Vector3(finalSize.x, finalSize.y + positions[r].y, finalSize.z);
            GenerateArcParameters(doorFinalSize);


            doorParams.finalPos = new Vector3(positions[r].x, 0, positions[r].z);
            doorParams.finalSize = doorFinalSize;
            doorParams.finalRot = rotations[r];

            doorParams.openingStyle = baseParams.doorStyle;

            if (baseParams.doorStyle == OpeningStyle.ARCH)
            {
                doorParams.archedOpeningParams = new ArchedOpeningParams(outerArcF, outerArcB, innerArcF, innerArcB, frameDimensions);
                doorParams.planeParams = new PlaneParams(baseParams.doorStyle, ColorManager.GetDoorColor(), BaseObjSizes.planeArcSize, innerArcF, doorFinalSize, windowOffset);
            }
            else
            {
                doorParams.squareOpeningParams = new SquareOpeningParams(frameDimensions);
                doorParams.planeParams = new PlaneParams(baseParams.doorStyle, ColorManager.GetDoorColor(), BaseObjSizes.planeSqSize, doorFinalSize, windowOffset);

            }

            positions.RemoveAt(r);
            rotations.RemoveAt(r);
        }

        GenerateArcParameters(finalSize);
        GenerateSegmentsPositions(finalSize);

        for (int i = 0; i < positions.Count; i++)
        {
            WindowParams windowParam = new WindowParams();

            if(!rowSameLit)
            glassColor = RandomiseWindowColor();

            if (baseParams.windowStyle == OpeningStyle.ARCH)
            {
                windowParam.archedOpeningParams = new ArchedOpeningParams(outerArcF, outerArcB, innerArcF, innerArcB, frameDimensions);
                windowParam.glassParams = new PlaneParams(baseParams.windowStyle,glassColor,BaseObjSizes.planeArcSize,innerArcF, finalSize, windowOffset);
            }
            else
            {
                windowParam.squareOpeningParams = new SquareOpeningParams(frameDimensions);
                windowParam.glassParams = new PlaneParams(baseParams.windowStyle, glassColor, BaseObjSizes.planeSqSize, finalSize, windowOffset);

            }

            windowParam.segmentationParams = new SegmentationParams(vertSegPositions, horSegPositions, segmentDimensions);
            windowParam.finalSize = finalSize;
            windowParam.finalPos = positions[i];
            windowParam.finalRot = rotations[i];
            windowParam.openingStyle = baseParams.windowStyle;
            windowParams.Add(windowParam);
        }
    }

    public void GenerateAtticOpenings(BaseParams lastBaseParams, AtticParams atticParams, ref List<WindowParams> windowParams, Vector3 lastFloorWinSize, bool rowSameLit)
    {
        if (atticParams.finalSize.y < lastBaseParams.finalSize.y)
        {
            return;
        }
        float maxGap =
            Random.Range(minDistanceBetweenWindows,
                maxDistanceBetweenWindows);

        List<Quaternion> rotations = new List<Quaternion>();
        List<Vector3> positions = new List<Vector3>();
        Vector3 winSize = lastFloorWinSize;
        float y = GetGapBetweemTopAndBottom(lastBaseParams.finalSize, winSize);
        float minX = lastBaseParams.finalSize.y / (Mathf.Tan(Mathf.Atan(atticParams.finalSize.y / (atticParams.finalSize.x / 2))));


        float spaceForWindows = atticParams.finalSize.x - minX * 2;

        float spaceForOneWin = winSize.x + maxGap * 2;

        int winCount = (int)(spaceForWindows / spaceForOneWin);
        Debug.Log(spaceForWindows);

        float SpaceForOneWindow = spaceForWindows/ winCount; //atemus tarpa nuo kampo kiek lieka vienam langui
        float step = (SpaceForOneWindow - winSize.x) / 2;

        Vector3 addBefore = new Vector3(step, 0, 0);
        Vector3 addAfter = new Vector3(winSize.x+step, 0, 0);
        Debug.Log(minX + step);
        DistributePostionsAndRotationsEvenly(
            new Vector3(minX+winSize.x, y, windowOffset),
            Quaternion.Euler(new Vector3(0, 180, 0)),
            addBefore,
            addAfter,
            winCount,
            ref positions,
            ref rotations
        );
        if (lastBaseParams.backFirewall == false)
        {
            DistributePostionsAndRotationsEvenly(
                new Vector3(minX, y, atticParams.finalSize.z - windowOffset),
                Quaternion.Euler(Vector3.zero),
                addBefore,
                addAfter,
                winCount,
                ref positions,
                ref rotations
            );
        }
        Vector2Int glassColor = RandomiseWindowColor();
        GenerateArcParameters(winSize);
        GenerateSegmentsPositions(winSize);

        for (int i = 0; i < positions.Count; i++)
        {
            WindowParams windowParam = new WindowParams();

            if (!rowSameLit)
                glassColor = RandomiseWindowColor();

            if (lastBaseParams.windowStyle == OpeningStyle.ARCH)
            {
                windowParam.archedOpeningParams = new ArchedOpeningParams(outerArcF, outerArcB, innerArcF, innerArcB, frameDimensions);
                windowParam.glassParams = new PlaneParams(lastBaseParams.windowStyle, glassColor, BaseObjSizes.planeArcSize, innerArcF, winSize, windowOffset);
            }
            else
            {
                windowParam.squareOpeningParams = new SquareOpeningParams(frameDimensions);
                windowParam.glassParams = new PlaneParams(lastBaseParams.windowStyle, glassColor, BaseObjSizes.planeSqSize, winSize, windowOffset);

            }

            windowParam.segmentationParams = new SegmentationParams(vertSegPositions, horSegPositions, segmentDimensions);
            windowParam.finalSize = winSize;
            windowParam.finalPos = positions[i];
            windowParam.finalRot = rotations[i];
            windowParam.openingStyle = lastBaseParams.windowStyle;
            windowParams.Add(windowParam);
        }
    }

    Vector2Int RandomiseWindowColor()
    {
        int r = Random.Range(0, 2);

        switch (r)
        {
            case 0:
                return ColorManager.GetBlueWindowColor();
            case 1:
                return ColorManager.GetYellowWindowColor();
            default:
                return ColorManager.GetBlueWindowColor();

        }
    }
    Vector3 RandomiseWindowSize(Vector3 baseSize)
    {
        return new Vector3(
                Random.Range(minWinDimensions.x, maxWinDimensions.x),
                Random.Range(baseSize.y / 2, baseSize.y - (baseSize.y / 2.5f)),
                Random.Range(minWinDimensions.z, maxWinDimensions.z)

            );
    }

    float GetGapBetweemTopAndBottom(Vector3 baseSize,
        Vector3 winSize)
    {
        return (baseSize.y - winSize.y) / 2;
    }
    void GenerateWindowsPositions(
        Vector3 baseSize,
        Vector3 winSize,
        bool leftFirewall,
        bool rightFirewall,
        bool backFirewall,
        ref List<Vector3> positions,
        ref List<Quaternion> rotations)
    {

        float maxGap =
            Random.Range(minDistanceBetweenWindows,
                maxDistanceBetweenWindows); // privalomas maximalus tarpas nuo namo kampo
        int xCount = (int)(baseSize.x / (winSize.x + maxGap)); // langu skaicius x asyje
        int zCount = (int)(baseSize.z / (winSize.x + maxGap)); // langu skaicius z asyje

        float y = GetGapBetweemTopAndBottom(baseSize,winSize); //atitraukimas nuo virsaus/apacios


        //x asies langai

        float SpaceForOneWindow = baseSize.x / xCount; //atemus tarpa nuo kampo kiek lieka vienam langui
        float step = (SpaceForOneWindow - winSize.x) / 2;

        Vector3 addBefore = new Vector3(step, 0, 0);
        Vector3 addAfter = new Vector3(step + winSize.x, 0, 0);

        DistributePostionsAndRotationsEvenly(
            new Vector3(winSize.x, y, windowOffset),
            Quaternion.Euler(new Vector3(0, 180, 0)),
            addBefore,
            addAfter,
            xCount,
            ref positions,
            ref rotations
            );
        if (backFirewall == false)
        {
            DistributePostionsAndRotationsEvenly(
                new Vector3(0, y, baseSize.z - windowOffset),
                Quaternion.Euler(Vector3.zero),
                addBefore,
                addAfter,
                xCount,
               ref positions,
               ref rotations
            );
        }
        //z asies langai

        SpaceForOneWindow = baseSize.z / zCount; //atemus tarpa nuo kampo kiek lieka vienam langui
        step = (SpaceForOneWindow - winSize.x) / 2;

        addBefore = new Vector3(0, 0, step);
        addAfter = new Vector3(0, 0, step + winSize.x);

        if (leftFirewall == false)
        {
            DistributePostionsAndRotationsEvenly(
                new Vector3(windowOffset, y, 0),
                Quaternion.Euler(new Vector3(0, 270, 0)),
                addBefore,
                addAfter,
                zCount,
               ref positions,
              ref rotations
            );
        }

        if (rightFirewall == false)
        {
            DistributePostionsAndRotationsEvenly(
                new Vector3(baseSize.x - windowOffset, y, winSize.x),
                Quaternion.Euler(new Vector3(0, 90, 0)),
                addBefore,
                addAfter,
                zCount,
               ref positions,
               ref rotations
            );
        }
    }

    void DistributePostionsAndRotationsEvenly(
        Vector3 startingPos,
        Quaternion rotToAdd,
        Vector3 addBefore,
        Vector3 addAfter,
        int count,
        ref List<Vector3> position,
       ref List<Quaternion> rotations)
    {
        Vector3 posToAdd = startingPos;
        for (int i = 0; i < count; i++)
        {
            posToAdd += addBefore;
            position.Add(posToAdd);
            rotations.Add(rotToAdd);
            posToAdd += addAfter;
        }
    }

    void DistributePositionsEvenly(Vector3 startingPos, Vector3 addBefore, Vector3 addAfter, int count, List<Vector3> position)
    {
        Vector3 posToAdd = startingPos;
        for (int i = 0; i < count; i++)
        {
            posToAdd += addBefore;
            position.Add(posToAdd);
            posToAdd += addAfter;
        }
    }
    void GenerateSegmentsPositions(Vector3 winSize)
    {
        vertSegPositions = new List<Vector3>();
        horSegPositions = new List<Vector3>();

        float height = winSize.y - frameDimensions.y * 2;
        float width = winSize.x - frameDimensions.x * 2;

        float verticalGap = Random.Range(minDistanceBetweenSegments, maxDistanceBetweenSegments);
        float horizontalGap = Random.Range(verticalGap, maxDistanceBetweenSegments);

        int verticalCount = (int)(width / (segmentDimensions.x + horizontalGap * 2));
        int horizontalCount = (int)(height / (segmentDimensions.x + verticalGap * 2));
        verticalCount = verticalCount == 0 ? 1 : verticalCount;
        horizontalCount = horizontalCount == 0 ? 1 : horizontalCount;

        float spaceForOneSegment = height / horizontalCount;
        float step = (spaceForOneSegment - segmentDimensions.y) / 2;

        Vector3 addBefore = new Vector3(0, step, 0);
        Vector3 addAfter = new Vector3(0, step + segmentDimensions.y, 0);

        DistributePositionsEvenly(
            Vector3.zero,
            addBefore,
            addAfter,
            horizontalCount,
            horSegPositions
        );

        spaceForOneSegment = width / verticalCount;
        step = (spaceForOneSegment - segmentDimensions.x) / 2;

        addBefore = new Vector3(step, 0, 0);
        addAfter = new Vector3(step + segmentDimensions.x, 0, 0);


        DistributePositionsEvenly(
            new Vector3(frameDimensions.x, 0, 0),
            addBefore,
            addAfter,
            verticalCount,
            vertSegPositions
        );
    }

    void GenerateArcParameters(Vector3 winSize)
    {
        angle = Random.Range(45, 60);
        ArcGenerator arcGenerator = new ArcGenerator();

        int arcPoints = BaseObjSizes.openingArcSize.y - 3;

        outerArcF = arcGenerator.GenerationZwei(angle, winSize.x, new Vector3(0, winSize.y, 0), arcPoints);
        outerArcB = arcGenerator.GenerationZwei(angle, winSize.x, new Vector3(0, winSize.y, frameDimensions.z), arcPoints);

        float addX = (winSize.x - (winSize.x - frameDimensions.x * 2)) / 2;

        innerArcF = arcGenerator.GenerationZwei(angle, winSize.x - frameDimensions.x * 2, new Vector3(addX, winSize.y, 0), arcPoints);
        innerArcB = arcGenerator.GenerationZwei(angle, winSize.x - frameDimensions.x * 2, new Vector3(addX, winSize.y, frameDimensions.z), arcPoints);

        float arcHeight = outerArcB[arcPoints / 2 - 1].y - outerArcB[0].y;

        for (int i = 0; i < outerArcF.Length; i++)
        {
            outerArcF[i].y -= arcHeight;
            outerArcB[i].y -= arcHeight;
            innerArcF[i].y -= arcHeight;
            innerArcB[i].y -= arcHeight;
        }

    }
}
