﻿using System.Collections;
using System.Collections.Generic;
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
    public Building(int maxFloors,GeneratorController generatorController, VertexVisualiser vertexVisualiser)
    {
       floors = new Base[RandomizeFloorCount(maxFloors)];

        for (int i = 0; i < floors.Length; i++)
        {
            floors[i] =
                new Base(
                    generatorController.baseMaterial,
                    generatorController.parentObj.transform,
                    RandomizeOpeningStyle(),
                    RandomizeOpeningStyle(),
                    generatorController.leftFirewall,
                    generatorController.rightFirewall,
                    generatorController.backFirewall,
                    i == 0 ? null : floors[i - 1],
                    vertexVisualiser.VisualiseVertices);

            floors[i].GenerateWindows(
                generatorController.windowMaterial,
                generatorController.glassMaterial,
                generatorController.segmentsMaterial, 
                generatorController.leftFirewall,
                generatorController.rightFirewall,
                generatorController.backFirewall);
        }
        attic = new Attic(
            generatorController.baseMaterial,
            floors[floors.Length - 1],
            vertexVisualiser.VisualiseVertices);

        roof = new Roof(
            generatorController.roofMaterial,
            floors[floors.Length-1],
            attic,
            vertexVisualiser.VisualiseVertices);
        
    }

    int RandomizeFloorCount(int maxFloors)
    {
          return Random.Range(1, maxFloors+1);
    }

    int RandomizeOpeningStyle()
    {
        return Random.Range(0,2);
    }
}
