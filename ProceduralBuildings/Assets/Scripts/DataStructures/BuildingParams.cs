public class BuildingParams
{
    public int minStoriesCount, maxStoriesCount;

    public bool useCustomBuildingSize;
    public float customBuildingSizeX;
    public float customBuildingSizeZ;

    public bool leftFirewall, rightFirewall, backFirewall;
    public bool rowSameLit;
    public bool sameSizeFloors;
    public bool onlySquareOpenings, onlyArchedOpenings;

    public bool generateCornerPillars;

    public BuildingParams(
        bool leftFirewall,
        bool rightFirewall,
        bool backFirewall,
        bool useCustomBuildingSize,
        float customBuildingSizeX,
        float customBuildingSizeZ,
        bool rowSameLit,
        bool sameSizeFloors,
        int minStoriesCount,
        int maxStoriesCount,
        bool generateCornerPillars,
        bool onlySquareOpenings,
        bool onlyArchedOpenings)
    {
        this.minStoriesCount = minStoriesCount;
        this.maxStoriesCount = maxStoriesCount;
        this.rightFirewall = rightFirewall;
        this.leftFirewall = leftFirewall;
        this.backFirewall = backFirewall;
        this.rowSameLit = rowSameLit;
        this.useCustomBuildingSize = useCustomBuildingSize;
        this.customBuildingSizeX = customBuildingSizeX;
        this.customBuildingSizeZ = customBuildingSizeZ;
        this.sameSizeFloors = sameSizeFloors;
        this.generateCornerPillars = generateCornerPillars;
        this.onlySquareOpenings = onlySquareOpenings;
        this.onlyArchedOpenings = onlyArchedOpenings;
    }

    public BuildingParams()
    {
        this.minStoriesCount = 1;
        this.maxStoriesCount = 5;
        this.rightFirewall = false;
        this.leftFirewall = false;
        this.backFirewall = false;
        this.rowSameLit = false;
        this.useCustomBuildingSize = false;
        this.customBuildingSizeX = 2;
        this.customBuildingSizeZ = 2;
        this.sameSizeFloors = false;
        this.generateCornerPillars = false;
        this.onlySquareOpenings = false;
        this.onlyArchedOpenings = false;
    }
}
