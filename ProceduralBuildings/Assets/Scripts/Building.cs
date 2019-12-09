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
    public Roof roof;
    public Attic attic;
    public Foundation foundation;
    public Building(int minStoriesCount, int maxStoriesCount, GeneratorController generatorController, VertexVisualiser vertexVisualiser)
    {

        floors = new Base[RandomizeFloorCount(minStoriesCount, maxStoriesCount)];

        for (int i = 0; i < floors.Length; i++)
        {
            floors[i] =
                new Base(
                    generatorController.mainMaterial,
                    generatorController.parentObj.transform,
                    RandomizeOpeningStyle(),
                    RandomizeOpeningStyle(),
                    generatorController.leftFirewall,
                    generatorController.rightFirewall,
                    generatorController.backFirewall,
                    i == 0 ? null : floors[i - 1],
                    vertexVisualiser.VisualiseVertices);

            floors[i].GenerateWindows(
                generatorController.mainMaterial,
                generatorController.leftFirewall,
                generatorController.rightFirewall,
                generatorController.backFirewall);
        }
        foundation = new Foundation(generatorController.mainMaterial, generatorController.parentObj.transform,floors[0]);
        attic = new Attic(
            generatorController.mainMaterial,
            floors[floors.Length - 1],
            vertexVisualiser.VisualiseVertices);

        roof = new Roof(
            generatorController.mainMaterial,
            floors[floors.Length-1],
            attic,
            vertexVisualiser.VisualiseVertices);

   
    }

    int RandomizeFloorCount(int from, int to)
    {
          return Random.Range(from, to + 1);
    }

    int RandomizeOpeningStyle()
    {
        return Random.Range(0,2);
    }
}
