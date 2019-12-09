using System.Collections.Generic;
using UnityEngine;

public struct WindowParams
{
    public List<Vector3> vertSegPositions;
    public List<Vector3> horSegPositions;
    public float angle;
    public Vector3[] outerArcF;
    public Vector3[] outerArcB;
    public Vector3[] innerArcF;
    public Vector3[] innerArcB;
    public OpeningStyle openingStyle;
}
public class WindowsGenerator
{
    private const float minDistanceBetweenWindows=0.2f;
    private const float maxDistanceBetweenWindows=0.5f;

    private readonly Vector3 minWinDimensions = new Vector3(0.4f,0.5f,0.1f);
    private readonly Vector3 maxWinDimensions = new Vector3(0.7f, 0.9f, 0.2f);

    private readonly Vector3 winFrameDimensions = new Vector3(0.05f,0.05f,0.1f);

    private readonly Vector3 segmentDimensions = new Vector3(0.04f, 0.04f, 0.04f);

    private const float minDistanceBetweenSegments = 0.1f;
    private const float maxDistanceBetweenSegments = 0.2f;
    private const float windowOffset = 0.02f; //kiek islindes

    public WindowsGenerator(Base baseObj, Material material,  bool leftFirewall, bool rightFirewall, bool backFirewall, OpeningStyle openingStyle)
    {

        GenerateWindows(baseObj, material,  leftFirewall, rightFirewall, backFirewall, openingStyle);
    }

    void GenerateWindows(Base baseObj, Material material, bool leftFirewall, bool rightFirewall, bool backFirewall, OpeningStyle openingStyle)
    {
        List<Quaternion> rotations = new List<Quaternion>();
        List<Vector3> positions = new List<Vector3>();
        WindowParams windowParams = new WindowParams();
        windowParams.openingStyle = openingStyle;
        Vector3 winSize = RandomiseWindowSize(baseObj.finalSize);
        GenerateWindowsPositions(baseObj.finalSize, winSize, leftFirewall, rightFirewall, backFirewall,ref positions, ref rotations);

        if (openingStyle == OpeningStyle.ARCH)
            windowParams = GenerateArcParameters(windowParams, winSize);

        windowParams = GenerateSegmentsPositions(windowParams, winSize);
        for (int i = 0; i < positions.Count; i++)
        {
            new Window(
                baseObj,
                material,
                winSize, 
                positions[i],
                rotations[i],
                windowParams,
                segmentDimensions,
                winFrameDimensions,
                windowOffset
                );
        }
    }
    Vector3 RandomiseWindowSize(Vector3 baseSize)
    {
        return  new Vector3(
                Random.Range(minWinDimensions.x, maxWinDimensions.x),
                Random.Range(minWinDimensions.y, baseSize.y > maxWinDimensions.y? maxWinDimensions.y: baseSize.y),
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
        int xCount = (int) (baseSize.x / (winSize.x + maxGap)); // langu skaicius x asyje
        int zCount = (int) (baseSize.z / (winSize.x + maxGap)); // langu skaicius z asyje

        float y = (baseSize.y - winSize.y) / 2; //atitraukimas nuo virsaus/apacios


        //x asies langai
    
        float SpaceForOneWindow = baseSize.x / xCount; //atemus tarpa nuo kampo kiek lieka vienam langui
        float step = (SpaceForOneWindow - winSize.x) / 2;

        Vector3 addBefore = new Vector3(step,0,0);
        Vector3 addAfter = new Vector3( step+winSize.x, 0,0);

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
        int count ,
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

    void DistributePositionsEvenly(Vector3 startingPos, Vector3 addBefore, Vector3 addAfter, int count, ref List<Vector3> position)
    {
        Vector3 posToAdd = startingPos;
        for (int i = 0; i < count; i++)
        {
            posToAdd += addBefore;
            position.Add(posToAdd);
            posToAdd += addAfter;
        }
    }
    WindowParams GenerateSegmentsPositions(WindowParams wParams, Vector3 winSize)
    {
        wParams.vertSegPositions = new List<Vector3>();
        wParams.horSegPositions = new List<Vector3>();

        float height = winSize.y - winFrameDimensions.y * 2;
        float width = winSize.x - winFrameDimensions.x * 2;

        float verticalGap = Random.Range(minDistanceBetweenSegments, maxDistanceBetweenSegments);
        float horizontalGap = Random.Range(verticalGap, maxDistanceBetweenSegments);

        int verticalCount = (int)(width / (segmentDimensions.x + horizontalGap * 2));
        int horizontalCount = (int)(height / (segmentDimensions.x + verticalGap * 2));
        verticalCount = verticalCount== 0 ? 1 : verticalCount;
        horizontalCount = horizontalCount == 0 ? 1 : horizontalCount;

        float spaceForOneSegment = height / horizontalCount;
        float step = (spaceForOneSegment - segmentDimensions.y) / 2;

        Vector3 addBefore = new Vector3(0, step, 0);
        Vector3 addAfter = new Vector3(0,step + segmentDimensions.y, 0);

        DistributePositionsEvenly(
            Vector3.zero,
            addBefore,
            addAfter,
            horizontalCount,
            ref wParams.horSegPositions
        );

        spaceForOneSegment = width / verticalCount;
        step = (spaceForOneSegment - segmentDimensions.x) / 2;

        addBefore = new Vector3(step,0, 0);
        addAfter = new Vector3(step + segmentDimensions.x,0,0);


        DistributePositionsEvenly(
            new Vector3(winFrameDimensions.x, 0, 0),
            addBefore,
            addAfter,
            verticalCount,
            ref wParams.vertSegPositions
        );


        return wParams;
    }

    WindowParams GenerateArcParameters(WindowParams windowParams, Vector3 winSize)
    {
        float angle = Random.Range(45, 60);
        ArcGenerator arcGenerator = new ArcGenerator();

        int arcPoints = BaseObjSizes.openingArcSize.y - 3;

        Vector3[] outerArcF = arcGenerator.GenerationZwei(angle, winSize.x, new Vector3(0, winSize.y, 0), arcPoints);
        Vector3[] outerArcB = arcGenerator.GenerationZwei(angle, winSize.x, new Vector3(0, winSize.y, winFrameDimensions.z), arcPoints);

        float addX = (winSize.x - (winSize.x - winFrameDimensions.x * 2)) / 2;

        Vector3[] innerArcF = arcGenerator.GenerationZwei(angle, winSize.x - winFrameDimensions.x * 2, new Vector3(addX, winSize.y, 0), arcPoints);
        Vector3[] innerArcB = arcGenerator.GenerationZwei(angle, winSize.x - winFrameDimensions.x * 2, new Vector3(addX, winSize.y, winFrameDimensions.z), arcPoints);

        float arcHeight = outerArcB[arcPoints / 2 - 1].y - outerArcB[0].y;

        for (int i = 0; i < outerArcF.Length; i++)
        {
            outerArcF[i].y -= arcHeight;
            outerArcB[i].y -= arcHeight;
            innerArcF[i].y -= arcHeight;
            innerArcB[i].y -= arcHeight;
        }


        windowParams.outerArcF = outerArcF;
        windowParams.outerArcB = outerArcB;
        windowParams.innerArcF = innerArcF;
        windowParams.innerArcB = innerArcB;
        windowParams.angle = angle;

        return windowParams;
    }
}
