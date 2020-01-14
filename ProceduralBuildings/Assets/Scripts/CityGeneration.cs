using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityGeneration : MonoBehaviour
{
    private List<Building> buildings;
    private GameObject mainParent;
    private GameObject quarterParent;
    private GameObject parentObj;
    private CombineMeshes combineMeshes;
    int buildingCountSide = 5;
    private int gridSize = 2;
    GameObject GenerateQuarter(Material material, List<Vector2> sizes, BuildingParams buildingParams)
    {
      
        
            buildingParams.useCustomBuildingSize = true;
            buildingParams.rightFirewall = true;
            buildingParams.backFirewall = false;
            buildingParams.leftFirewall = false;


            quarterParent = new GameObject();
            quarterParent.transform.position = Vector3.zero;
            quarterParent.name = "CityBlock";


            Vector3 posToSet = Vector3.zero;
            buildings = new List<Building>();

            int id = 0;
            int yAngle = 0;

            posToSet.x += sizes[0].y;

            for (int j = 0; j < 4; j++)
            {
                for (int i = 0; i < buildingCountSide - 1; i++, id++)
                {

                    parentObj = new GameObject();
                    parentObj.transform.position = Vector3.zero;
                    parentObj.name = "Building";

                    buildingParams.customBuildingSizeX = sizes[i].x;
                    buildingParams.customBuildingSizeZ = sizes[i].y;

                    if (i == buildingCountSide - 2)
                    {
                        buildingParams.rightFirewall = false;
                        buildingParams.backFirewall = true;
                }
                    else
                    {
                        buildingParams.leftFirewall = true;
                        buildingParams.backFirewall = false;

                }
                buildings.Add(new Building(buildingParams, material, parentObj.transform));
                    if (i != 0)
                    {
                        switch (j)
                        {
                            case 0:
                                posToSet.x += buildings[id - 1].foundationParams.finalSize.x;
                                break;
                            case 1:
                                posToSet.z += buildings[id - 1].foundationParams.finalSize.x;
                                break;
                            case 2:
                                posToSet.x -= buildings[id - 1].foundationParams.finalSize.x;
                                break;
                            case 3:
                                posToSet.z -= buildings[id - 1].foundationParams.finalSize.x;
                                break;
                        }
                    }
                   
                    combineMeshes.MergeChildren(parentObj);

                    parentObj.transform.position = posToSet;
                    parentObj.transform.rotation = Quaternion.Euler(new Vector3(0, yAngle, 0));
                    parentObj.transform.parent = quarterParent.transform;


                }

                yAngle -= 90;
                buildingParams.rightFirewall = true;
                switch (j)
                {
                    case 0:
                        posToSet.x += buildings[id - 1].foundationParams.finalSize.x;
                        posToSet.z += buildings[id - 1].foundationParams.finalSize.z;
                        break;
                    case 1:
                        posToSet.z += buildings[id - 1].foundationParams.finalSize.x;
                        posToSet.x -= buildings[id - 1].foundationParams.finalSize.z;

                        break;
                    case 2:
                        posToSet.x -= buildings[id - 1].foundationParams.finalSize.x;
                        posToSet.z -= buildings[id - 1].foundationParams.finalSize.z;
                        break;
                }

            }
        
        return quarterParent;
    }
    public void Generate(Material material, BuildingParams buildingParams)
    {
        if (mainParent != null)
            DestroyImmediate(mainParent);

        mainParent = new GameObject();
        mainParent.name = "City";
        combineMeshes = new CombineMeshes();
        float rowLength = GetRowLength(buildingCountSide);
        List<Vector2> sizes = GetFoundionSizes(buildingCountSide, rowLength);

        float quarterLength = rowLength + sizes[sizes.Count-1].y;
        float roadWidth = 2;
        for (int y = 0; y < gridSize; y++)
        {
            for (int x = 0; x < gridSize; x++)
            {
                GameObject quarter = GenerateQuarter(material, sizes, buildingParams);
                quarter.transform.parent = mainParent.transform;
                quarter.transform.position = new Vector3(x * quarterLength+roadWidth*x,0, y * quarterLength + roadWidth * y);
            }
        }
     

    }

    List<Vector2> GetFoundionSizes(int houseCount,float rowLength)
    {
        List<Vector2> sizes = new List<Vector2>();
        houseCount--;
        float xUsed = 0;
        float spaceForOne = rowLength / houseCount;
        float x, z;
        for (int i = 0; i < houseCount - 1; i++)
        {
            x = Random.Range(3, spaceForOne);
            z = Random.Range(3.5f, 4f);

            sizes.Add(new Vector2(x,z));
            xUsed += x;
        }

        x = rowLength - xUsed;
        z = Random.Range(3.5f, 4f);

        sizes.Add(new Vector2(x, z));

        return sizes;
    }

    float GetRowLength(int count)
    {
        return Random.Range((float)((count - 1) * 2.5), (float)((count - 1) * 3f));
    }
}
