
using UnityEngine;

public static class BaseObjSizes
{
   public static readonly Vector3Int baseSize = new Vector3Int(5,1,5);
   public static readonly Vector3Int roofSize = new Vector3Int(1, 2, 1);
   public static readonly Vector3Int atticSize = new Vector3Int(1, 1, 1);
   public static readonly Vector3Int openingSqSize = new Vector3Int(1, 4, 1);
   public static readonly Vector3Int openingArcSize = new Vector3Int(1, 11, 1);
   public static readonly Vector3Int planeSqSize = new Vector3Int(1, 1, 0);
   public static readonly Vector3Int planeArcSize = new Vector3Int(openingArcSize.y-3, 1, 0);
   public static readonly Vector3Int segmentationSize = new Vector3Int(1, 1, 1);

}
