using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityGeneration : MonoBehaviour
{
    private List<Building> buildings;
    private GameObject mainParent;
    public void Generate(Material material)
    {
        if(mainParent !=null)
            DestroyImmediate(mainParent);
        mainParent = new GameObject();
        mainParent.transform.position = Vector3.zero;
        mainParent.name = "CityBlock";

        
        Vector3 posToSet = Vector3.zero;
        buildings = new List<Building>();
        int buildingCountSide = 5;

        BuildingParams buildingParams = new BuildingParams(
            false,
            true,
            true,
            false,
            0,
            0,
            false,
            false,
            1,
            5,
            true,
            false,
            false);
        {
            GameObject parentObj = new GameObject();
            parentObj.transform.position = Vector3.zero;
            parentObj.name = "Building";
            parentObj.transform.parent = mainParent.transform;

            buildings.Add(new Building(buildingParams, material, parentObj.transform));

            int id = 1;
            int yAngle = 0;

            posToSet.x += buildings[0].foundationParams.finalSize.x;

            for (int j = 0; j < 4; j++)
        {
            for (int i = 1; i < buildingCountSide; i++, id++)
            {
                if(j == 3 && i == buildingCountSide-1)
                    break;
                
                parentObj = new GameObject();
                parentObj.transform.position = Vector3.zero;
                parentObj.name = "Building";

                if (i == buildingCountSide - 1)
                {
                    buildingParams.rightFirewall = false;
                }
                else
                {
                    buildingParams.leftFirewall = true;
                }
                buildings.Add(new Building(buildingParams, material, parentObj.transform));
                if (i != 1)
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

                parentObj.transform.position = posToSet;
                parentObj.transform.rotation = Quaternion.Euler(new Vector3(0, yAngle, 0));
                parentObj.transform.parent = mainParent.transform;
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
                case 3:
                 //   posToSet.z -= buildings[id - 1].foundationParams.finalSize.z;
                    break;
            }

        }
        }
       
    }
}
