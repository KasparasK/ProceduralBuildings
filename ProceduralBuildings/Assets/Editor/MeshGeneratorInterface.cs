using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[CustomEditor(typeof(GeneratorController), true)]
[System.Serializable]
public class MeshGeneratorInterface : Editor
{
    GeneratorController generatorController;

    public override void OnInspectorGUI()
    {
        generatorController = (GeneratorController)target;
        base.OnInspectorGUI();

        if (GUILayout.Button("Export", GUILayout.Width(100)))
        {
            if (!String.IsNullOrEmpty(generatorController.path))
            {
              //  ObjExporter.DoExport(false, generatorController.GetExportObj(), generatorController.path);

            }
        }
        if (GUILayout.Button("Generate Building", GUILayout.Width(100)))
        {
           generatorController.Generate();  
        }
    }
}
