using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public enum OpeningStyle
{
    ARCH,
    SQUARE,
}
public class Building
{
    public Base[] floors;
    public BaseParams[] baseParams;

    public Roof roof;
    private RoofParams roofParams;

    public Attic attic;
    private AtticParams atticParams;

    public Foundation foundation;
    private FoundationParams foundationParams;

    private Door door;

    private int floorCount;
    public Building(int minStoriesCount, int maxStoriesCount, GeneratorController generatorController, VertexVisualiser vertexVisualiser)
    {
        floorCount = RandomizeFloorCount(minStoriesCount, maxStoriesCount);
        floors = new Base[floorCount];
        baseParams = new BaseParams[floorCount];

        OpeningsGenerator openingsGenerator = new OpeningsGenerator();

        foundationParams = new FoundationParams();
        foundation = new Foundation(
            generatorController.mainMaterial,
            generatorController.parentObj.transform,
            foundationParams
        );

        for (int i = 0; i < floorCount; i++)
        {
            if (i == 0)
            {
                baseParams[i] = new BaseParams(
                    foundationParams.finalSize,
                    generatorController.leftFirewall,
                    generatorController.rightFirewall,
                    generatorController.backFirewall,
                    RandomizeOpeningStyle(),
                    RandomizeOpeningStyle());

                floors[i] = new Base(ref baseParams[i], foundation.obj.transform, generatorController.mainMaterial,null ,vertexVisualiser.VisualiseVertices);
              
            }
            else
            {
                baseParams[i] = new BaseParams(
                    baseParams[i - 1].finalSize,
                    generatorController.leftFirewall,
                    generatorController.rightFirewall,
                    generatorController.backFirewall,
                    RandomizeOpeningStyle());

                floors[i] = new Base(ref baseParams[i], floors[i-1].obj.transform, generatorController.mainMaterial, baseParams[i-1], vertexVisualiser.VisualiseVertices);

            }

            List<WindowParams> winParams = new List<WindowParams>();
            DoorParams doorParams = new DoorParams();
            openingsGenerator.GenerateOpenings(baseParams[i],ref winParams,ref doorParams);
            floors[i].windows = new List<Window>();
            for (int j = 0; j < winParams.Count; j++)
            {
                floors[i].windows.Add(new Window(floors[i].obj.transform, generatorController.mainMaterial, winParams[j]));
            }

           if(baseParams[i].groundFloor)
               door = new Door(floors[i].obj.transform, generatorController.mainMaterial,doorParams);

        }
     

        atticParams = new AtticParams(baseParams[floorCount-1].finalSize);
        attic = new Attic(
            generatorController.mainMaterial,
            atticParams,
            floors[floorCount - 1].obj.transform,
            vertexVisualiser.VisualiseVertices);

        roofParams = new RoofParams(atticParams.finalSize, baseParams[floorCount - 1].finalSize);
       roof = new Roof(
            generatorController.mainMaterial,
            baseParams[floorCount-1],
            roofParams,
            floors[floorCount - 1].obj.transform,
            vertexVisualiser.VisualiseVertices);


    }

    int RandomizeFloorCount(int from, int to)
    {
          return Random.Range(from, to + 1);
    }

    OpeningStyle RandomizeOpeningStyle()
    {
        OpeningStyle openingStyle = (OpeningStyle)Random.Range(0, 2);
        return openingStyle;
    }
}
