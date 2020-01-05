using UnityEngine;


public enum OpeningStyle
{
    ARCH,
    SQUARE,
}
public class Building
{
    private Base[] bases;
    public BaseParams[] baseParams;

    private Roof roof;
    private RoofParams roofParams;

    private Attic attic;
    private AtticParams atticParams;

    private Foundation foundation;
    private FoundationParams foundationParams;

    private Chimney chimney;
    private ChimneyParams chimneyParams;

    private Door door;

    private int floorCount;
    public Building(BuildingParams buildingParams, Material material, Transform parent)
    {
        floorCount = RandomizeFloorCount(buildingParams.minStoriesCount, buildingParams.maxStoriesCount);
        bases = new Base[floorCount];
        baseParams = new BaseParams[floorCount];

        ColorManager.SetColorPalleteID(RandomiseColorPallete());

        GenerateSegmentParams(buildingParams);

        GenerateSegments(buildingParams,material, parent);
    }

    void GenerateSegments(BuildingParams buildingParams, Material material, Transform parent)
    {
        foundation = new Foundation(material, parent, foundationParams, buildingParams);

        for (int i = 0; i < floorCount; i++)
        {
            if (i == 0)
            {
                bases[i] = new Base(ref baseParams[i], foundation.obj.transform, material,buildingParams, null);
                bases[i].GenerateDoor(baseParams[i].doorParams, material);
            }
            else
            {
                bases[i] = new Base(ref baseParams[i], bases[i - 1].obj.transform, material, buildingParams, baseParams[i - 1]);
            }


            bases[i].GenerateWindows(baseParams[i], material);
        }

        attic = new Attic(material, atticParams, bases[floorCount - 1].obj.transform);
        attic.GenerateWindows(atticParams, material);

        roof = new Roof(material, baseParams[floorCount - 1], roofParams, bases[floorCount - 1].obj.transform);

        chimney = new Chimney(chimneyParams, material, roof.obj.transform);
    }

    void GenerateSegmentParams(BuildingParams buildingParams)
    {

        foundationParams = new FoundationParams(buildingParams);

        for (int i = 0; i < floorCount; i++)
        {
            if (i == 0)
            {
                baseParams[i] = new BaseParams(foundationParams.finalSize, buildingParams, RandomizeOpeningStyle(), RandomizeOpeningStyle());
                baseParams[i].GenerateWindowsAndDoorParams(buildingParams);
            }
            else
            {
                //2 auktas nustato visu sekanciu aukstu langu isvaizda, pirmas aukstas turi savo
                baseParams[i] = new BaseParams(baseParams[i - 1].finalSize, buildingParams, i, i >= 2 ? baseParams[i - 1].windowStyle : RandomizeOpeningStyle());
                if(i == 1)
                    baseParams[i].GenerateWindowsParams(buildingParams);
                else
                    baseParams[i].GenerateWindowsParams(buildingParams, baseParams[i - 1].windowParams[0].finalSize);

            }
        }

        atticParams = new AtticParams(baseParams[floorCount - 1].finalSize);
        atticParams.GenerateWindowsParams(buildingParams,baseParams[floorCount-1]);

        roofParams = new RoofParams(atticParams.finalSize, baseParams[floorCount - 1].finalSize);
        chimneyParams = new ChimneyParams(roofParams, baseParams[floorCount - 1]);
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
