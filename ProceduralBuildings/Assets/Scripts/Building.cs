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
    public Base[] bases;
    public BaseParams[] baseParams;

    public Roof roof;
    private RoofParams roofParams;

    public Attic attic;
    private AtticParams atticParams;

    public Foundation foundation;
    private FoundationParams foundationParams;

    private Chimney chimney;
    private ChimneyParams chimneyParams;


    private Door door;

    private int floorCount;
    public Building(BuildingParams buildingParams, Material material, Transform parent, VertexVisualiser vertexVisualiser)
    {
        floorCount = RandomizeFloorCount(buildingParams.minStoriesCount, buildingParams.maxStoriesCount);
        bases = new Base[floorCount];
        baseParams = new BaseParams[floorCount];

        ColorManager.SetColorPalleteID(RandomiseColorPallete());

        OpeningsGenerator openingsGenerator = new OpeningsGenerator();

        foundationParams = new FoundationParams(buildingParams);
        foundation = new Foundation(
            material,
            parent,
            foundationParams
        );

        for (int i = 0; i < floorCount; i++)
        {
            if (i == 0)
            {
                baseParams[i] = new BaseParams(
                    foundationParams.finalSize,
                    buildingParams,
                    RandomizeOpeningStyle(),
                    RandomizeOpeningStyle());

                bases[i] = new Base(ref baseParams[i], foundation.obj.transform, material,null ,vertexVisualiser.VisualiseVertices);
              
            }
            else
            {
                baseParams[i] = new BaseParams(
                    baseParams[i - 1].finalSize,
                    buildingParams,
                    i,
                    i >= 2? baseParams[i-1].windowStyle : RandomizeOpeningStyle()); //2 auktas nustato visu sekanciu aukstu langu isvaizda, pirmas aukstas turi savo

                bases[i] = new Base(ref baseParams[i], bases[i-1].obj.transform, material, baseParams[i-1], vertexVisualiser.VisualiseVertices);

            }

            List<WindowParams> winParams = new List<WindowParams>();
            DoorParams doorParams = new DoorParams();
            if (baseParams[i].floorNum < 2)
            {
                openingsGenerator.GenerateOpenings(baseParams[i], ref winParams, ref doorParams, buildingParams.rowSameLit);
            }
            else
            {
                openingsGenerator.GenerateOpenings(baseParams[i],ref winParams, ref doorParams, buildingParams.rowSameLit, baseParams[i-1].windowParams[0].finalSize);

            }

            baseParams[i].windowParams = winParams;
            bases[i].windows = new List<Window>();

           for (int j = 0; j < winParams.Count; j++)
            {
                bases[i].windows.Add(new Window(bases[i].obj.transform, material, winParams[j]));
            }

           if(baseParams[i].floorNum == 0)
               door = new Door(bases[i].obj.transform, material, doorParams);

        }
     

        atticParams = new AtticParams(baseParams[floorCount-1].finalSize);
        attic = new Attic(
            material,
            atticParams,
            bases[floorCount - 1].obj.transform,
            vertexVisualiser.VisualiseVertices);
        List<WindowParams> atticWinParams = new List<WindowParams>();

        openingsGenerator.GenerateAtticOpenings(baseParams[baseParams.Length-1],atticParams, ref atticWinParams, baseParams[baseParams.Length - 1].windowParams[0].finalSize, buildingParams.rowSameLit);

        for (int j = 0; j < atticWinParams.Count; j++)
        {
           new Window(attic.obj.transform, material, atticWinParams[j]);
        }
        //-------------------------------
        roofParams = new RoofParams(atticParams.finalSize, baseParams[floorCount - 1].finalSize);
       roof = new Roof(
           material,
            baseParams[floorCount-1],
            roofParams,
            bases[floorCount - 1].obj.transform,
            vertexVisualiser.VisualiseVertices);

        chimneyParams = new ChimneyParams(roofParams, baseParams[floorCount - 1]);
        chimney = new Chimney(chimneyParams, material, roof.obj.transform);
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

    int RandomiseColorPallete()
    {
        const int from = 1;
        const int to = 4;
        return Random.Range(from, to);
    }
}
