using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public struct WindowParams
{
    public List<Quaternion> rotations;
    public List<Vector3> positions;
    public List<Vector3> vertSegPositions;
    public List<Quaternion> vertSegRotatations;
    public List<Vector3> horSegPositions;
    public List<Quaternion> horSegRotatations;


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

    public WindowsGenerator(Base baseObj, Material windowMat, Material glassMat, Material segmentaionMat, bool leftFirewall, bool rightFirewall, bool backFirewall)
    {

        GenerateWindows(baseObj, windowMat, glassMat, segmentaionMat, leftFirewall, rightFirewall, backFirewall);
    }

    void GenerateWindows(Base baseObj, Material windowMat, Material glassMat, Material segmentaionMat, bool leftFirewall, bool rightFirewall, bool backFirewall)
    {
        Vector3 winSize = RandomiseWindowSize(baseObj.finalSize);
        WindowParams windowParams = GenerateWindowsPositions(baseObj.finalSize, winSize, leftFirewall, rightFirewall, backFirewall);
        windowParams = GenerateSegmentsPositions(windowParams, winSize);
        for (int i = 0; i < windowParams.positions.Count; i++)
        {
            new Window(
                baseObj,
                windowMat, 
                glassMat, 
                segmentaionMat,
                winSize, 
                windowParams.positions[i],
                windowParams.rotations[i],
                windowParams.vertSegPositions,
                windowParams.horSegPositions,
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

    WindowParams GenerateWindowsPositions(Vector3 baseSize, Vector3 winSize, bool leftFirewall, bool rightFirewall, bool backFirewall)
    {
        WindowParams windowParams = new WindowParams();

        List<Vector3> position = new List<Vector3>();
        List<Quaternion> rotations = new List<Quaternion>();


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

        DistributePostionsEvenly(
            new Vector3(winSize.x, y, windowOffset),
            Quaternion.Euler(new Vector3(0, 180, 0)),
            addBefore,
            addAfter,
            xCount,
            ref position,
            ref rotations
            );
        if (backFirewall == false)
        {
            DistributePostionsEvenly(
                new Vector3(0, y, baseSize.z - windowOffset),
                Quaternion.Euler(Vector3.zero),
                addBefore,
                addAfter,
                xCount,
                ref position,
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
            DistributePostionsEvenly(
                new Vector3(windowOffset, y, 0),
                Quaternion.Euler(new Vector3(0, 270, 0)),
                addBefore,
                addAfter,
                zCount,
                ref position,
                ref rotations
            );
        }

        if (rightFirewall == false)
        {
            DistributePostionsEvenly(
                new Vector3(baseSize.x - windowOffset, y, winSize.x),
                Quaternion.Euler(new Vector3(0, 90, 0)),
                addBefore,
                addAfter,
                zCount,
                ref position,
                ref rotations
            );
        }

        windowParams.positions = position;
        windowParams.rotations = rotations;
        return windowParams;

    }

    void DistributePostionsEvenly(Vector3 startingPos, Quaternion rotToAdd, Vector3 addBefore, Vector3 addAfter,int count ,ref List<Vector3> position, ref List<Quaternion> rotations)
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
    WindowParams GenerateSegmentsPositions(WindowParams wParams, Vector3 winSize)
    {
        wParams.vertSegPositions = new List<Vector3>();
        wParams.horSegPositions = new List<Vector3>();
        wParams.vertSegRotatations = new List<Quaternion>();
        wParams.horSegRotatations = new List<Quaternion>();

        float height = winSize.y - winFrameDimensions.y * 2;
        float width = winSize.x - winFrameDimensions.x * 2;

        float verticalGap = Random.Range(minDistanceBetweenSegments, maxDistanceBetweenSegments);
        float horizontalGap = Random.Range(verticalGap, maxDistanceBetweenSegments);

        int verticalCount = (int)(width / (segmentDimensions.x + horizontalGap * 2));
        int horizontalCount = (int)(height / (segmentDimensions.x + verticalGap * 2));

        float spaceForOneSegment = height / horizontalCount;
        float step = (spaceForOneSegment - segmentDimensions.y) / 2;

        Vector3 addBefore = new Vector3(0, step, 0);
        Vector3 addAfter = new Vector3(0,step + segmentDimensions.y, 0);

        DistributePostionsEvenly(
            Vector3.zero, 
            Quaternion.Euler(Vector3.zero),
            addBefore,
            addAfter,
            horizontalCount,
            ref wParams.horSegPositions,
            ref wParams.horSegRotatations
        );

        spaceForOneSegment = width / verticalCount;
        step = (spaceForOneSegment - segmentDimensions.x) / 2;

        addBefore = new Vector3(step,0, 0);
        addAfter = new Vector3(step + segmentDimensions.x,0,0);


        DistributePostionsEvenly(
            new Vector3(winFrameDimensions.x, 0, 0),
            Quaternion.Euler(Vector3.zero),
            addBefore,
            addAfter,
            verticalCount,
            ref wParams.vertSegPositions,
            ref wParams.vertSegRotatations
        );


        return wParams;
    }
}
