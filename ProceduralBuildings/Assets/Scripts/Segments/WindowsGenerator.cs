using System.Collections.Generic;
using UnityEngine;

public class WindowsGenerator : MonoBehaviour
{
    private const float minDistanceBetweenWindows = 0.2f;
    private const float maxDistanceBetweenWindows = 0.5f;

    private readonly Vector3 minWinDimensions = new Vector3(0.4f, 0.5f, 0.1f);
    private readonly Vector3 maxWinDimensions = new Vector3(0.7f, 0.9f, 0.2f);

    private readonly Vector3 winFrameDimensions = new Vector3(0.05f, 0.05f, 0.1f);

    private readonly Vector3 segmentDimensions = new Vector3(0.04f, 0.04f, 0.04f);

    private const float minDistanceBetweenSegments = 0.1f;
    private const float maxDistanceBetweenSegments = 0.2f;
    private const float windowOffset = 0.02f; //kiek islindes

    private List<Vector3> vertSegPositions;
    private List<Vector3> horSegPositions;
    private float angle;
    private Vector3[] outerArcF;
    private Vector3[] outerArcB;
    private Vector3[] innerArcF;
    private Vector3[] innerArcB;
    public List<WindowParams> GenerateWindows(BaseParams baseParams)
    {
        List<Quaternion> rotations = new List<Quaternion>();
        List<Vector3> positions = new List<Vector3>();
        List<WindowParams> windowParams = new List<WindowParams>();

        Vector3 finalSize = RandomiseWindowSize(baseParams.finalSize);
        GenerateWindowsPositions(baseParams.finalSize, finalSize, baseParams.leftFirewall, baseParams.rightFirewall, baseParams.backFirewall, ref positions, ref rotations);

        if (baseParams.windowStyle == OpeningStyle.ARCH)
            GenerateArcParameters(finalSize);

        GenerateSegmentsPositions(finalSize);

        for (int i = 0; i < positions.Count; i++)
        {
            WindowParams windowParam = new WindowParams();

            windowParam.angle = angle;
            windowParam.vertSegPositions = vertSegPositions;
            windowParam.horSegPositions = horSegPositions;
            windowParam.outerArcB = outerArcB;
            windowParam.outerArcF = outerArcF;
            windowParam.innerArcB = innerArcB;
            windowParam.innerArcF = innerArcF;
            windowParam.finalSize = finalSize;
            windowParam.finalPos = positions[i];
            windowParam.rot = rotations[i];
            windowParam.segmentDimensions = segmentDimensions;
            windowParam.winFrameDimensions = winFrameDimensions;
            windowParam.windowOffset = windowOffset;
            windowParam.openingStyle = baseParams.windowStyle;
            windowParam.glassColor = RandomiseWindowColor();
            windowParams.Add(windowParam);
        }

        return windowParams;
    }

    Vector2Int RandomiseWindowColor()
    {
        int r = Random.Range(0, 2);

        switch (r)
        {
            case 0:
                return TextureColorIDs.lightBlueWindow;
            case 1:
                return TextureColorIDs.yellowWindow;
            default:
                return TextureColorIDs.lightBlueWindow;

        }
    }
    Vector3 RandomiseWindowSize(Vector3 baseSize)
    {
        return new Vector3(
                Random.Range(minWinDimensions.x, maxWinDimensions.x),
                Random.Range(baseSize.y / 2, baseSize.y - (baseSize.y / 2.5f)),//baseSize.y > maxWinDimensions.y? maxWinDimensions.y: baseSize.y
                Random.Range(minWinDimensions.z, maxWinDimensions.z)

            );
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

        float y = (baseSize.y - winSize.y) / 2; //atitraukimas nuo virsaus/apacios


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

        float height = winSize.y - winFrameDimensions.y * 2;
        float width = winSize.x - winFrameDimensions.x * 2;

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
            new Vector3(winFrameDimensions.x, 0, 0),
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
        outerArcB = arcGenerator.GenerationZwei(angle, winSize.x, new Vector3(0, winSize.y, winFrameDimensions.z), arcPoints);

        float addX = (winSize.x - (winSize.x - winFrameDimensions.x * 2)) / 2;

        innerArcF = arcGenerator.GenerationZwei(angle, winSize.x - winFrameDimensions.x * 2, new Vector3(addX, winSize.y, 0), arcPoints);
        innerArcB = arcGenerator.GenerationZwei(angle, winSize.x - winFrameDimensions.x * 2, new Vector3(addX, winSize.y, winFrameDimensions.z), arcPoints);

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
