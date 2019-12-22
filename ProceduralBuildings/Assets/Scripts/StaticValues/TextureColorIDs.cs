using UnityEngine;

public static class TextureColorIDs 
{

    public static readonly int pillarsColorX = 1;
    public static readonly int roofColorX = 5;
    public static readonly int baseColorX = 6;
    public static readonly int segmentColorX = 3;
    public static readonly int openingColorX = 2;
    public static readonly int foundationColorX = 4;
    public static readonly int chimneyColorX = 4;
    public static readonly int doorColorX = 3;

    public static readonly int blueWindowColorX = 16;
    public static readonly int yellowWindowColorX = 16;
}


public static class ColorManager
{
    private static int ID = 1;

    public static void SetColorPalleteID(int id)
    {
        ID = id;
    }

    public static Vector2Int GetRoofColor()
    {
        return new Vector2Int(TextureColorIDs.roofColorX,ID);
    }
    public static Vector2Int GetPillarsColor()
    {
        return new Vector2Int(TextureColorIDs.pillarsColorX, ID);
    }
    public static Vector2Int GetBaseColor()
    {
        return new Vector2Int(TextureColorIDs.baseColorX, ID);
    }
    public static Vector2Int GetSegmentColor()
    {
        return new Vector2Int(TextureColorIDs.segmentColorX, ID);
    }
    public static Vector2Int GetOpeningColor()
    {
        return new Vector2Int(TextureColorIDs.openingColorX, ID);
    }
    public static Vector2Int GetFoundationColor()
    {
        return new Vector2Int(TextureColorIDs.foundationColorX, ID);
    }
    public static Vector2Int GetChimneyColor()
    {
        return new Vector2Int(TextureColorIDs.chimneyColorX, ID);
    }

    public static Vector2Int GetBlueWindowColor()
    {
        return new Vector2Int(TextureColorIDs.blueWindowColorX, 1);
    }
    public static Vector2Int GetYellowWindowColor()
    {
        return new Vector2Int(TextureColorIDs.yellowWindowColorX, 2);
    }
    public static Vector2Int GetDoorColor()
    {
        return new Vector2Int(TextureColorIDs.doorColorX, ID);
    }
}
