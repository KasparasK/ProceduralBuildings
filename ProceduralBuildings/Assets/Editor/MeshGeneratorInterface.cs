using UnityEngine;
using UnityEditor;
using System;

[CustomEditor(typeof(GeneratorController), true)]
[System.Serializable]
public class MeshGeneratorInterface : Editor
{
    GeneratorController generatorController;
    private GUIStyle generateButton;
    private GUIStyle horizontalLine;
    private GUIStyle controlPanel;
    float minStoriesCount = 1;
    float maxStoriesCount = 1;
    int selected = 0;

    void OnEnable()
    {
        generateButton = new GUIStyle();
        horizontalLine = new GUIStyle();
        controlPanel = new GUIStyle();
    }

    void HorizontalLine(Color color)
    {
        var c = GUI.color;
        GUI.color = color;
        GUILayout.Box(GUIContent.none, horizontalLine);
        GUI.color = c;
    }

    public override void OnInspectorGUI()
    {
        generatorController = (GeneratorController)target;
        base.OnInspectorGUI();

        horizontalLine.normal.background = EditorGUIUtility.whiteTexture;
        horizontalLine.margin = new RectOffset(0, 0, 4, 4);
        horizontalLine.fixedHeight = 1;
        controlPanel = "Box";
        controlPanel.normal.background = EditorGUIUtility.whiteTexture;
        float maxWidth = 300;
        GUILayout.BeginVertical(controlPanel,GUILayout.MinWidth(maxWidth));

        FirewallSettings();
        HorizontalLine(Color.black);

        OtherSettings();
        HorizontalLine(Color.black);

        FloorCountSettings(maxWidth);
        HorizontalLine(Color.black);

        CustomSizeSettings();
        HorizontalLine(Color.black);

        ControlButtons();
        HorizontalLine(Color.black);

        TestButtons();
        GUILayout.EndVertical();
    }

    void FirewallSettings()
    {
        GUILayout.Label("Firewalls", EditorStyles.boldLabel, GUILayout.Width(80));
        GUILayout.Space(10);
        generatorController.leftFirewall = GUILayout.Toggle(generatorController.leftFirewall, "Left firewall");
        GUILayout.Space(5);

        generatorController.rightFirewall = GUILayout.Toggle(generatorController.rightFirewall, "Right firewall");
        GUILayout.Space(5);

        generatorController.backFirewall = GUILayout.Toggle(generatorController.backFirewall, "Back firewall");
        GUILayout.Space(5);
    }
    void OtherSettings()
    {
        GUILayout.Label("Other settings", EditorStyles.boldLabel, GUILayout.Width(150));
        GUILayout.Space(10);

        generatorController.rowSameLit = GUILayout.Toggle(generatorController.rowSameLit, "Every row lit the same");
        GUILayout.Space(5);

        generatorController.generateCornerPillars = GUILayout.Toggle(generatorController.generateCornerPillars, "Corner pillars");
        GUILayout.Space(5);

        generatorController.sameSizeFloors = GUILayout.Toggle(generatorController.sameSizeFloors, "Every floor the same size");
        GUILayout.Space(5);

        string[] options = new string[]
        {
            "Random style of door and windows", "Only arched door and windows","Only square door and windows",
        };
        selected = EditorGUILayout.Popup("Windows and doors style", selected, options);

        switch (selected)
        {
            case 0:
                generatorController.onlyArchedOpenings = false;
                generatorController.onlySquareOpenings = false;
                break;
            case 1:
                generatorController.onlyArchedOpenings = true;
                generatorController.onlySquareOpenings = false;
                break;
            case 2:
                generatorController.onlyArchedOpenings = false;
                generatorController.onlySquareOpenings = true;
                break;
            default:
                generatorController.onlyArchedOpenings = false;
                generatorController.onlySquareOpenings = false;
                break;
        }
    }
    void FloorCountSettings(float maxWidth)
    {
        maxStoriesCount = generatorController.MaxStoriesCount;
        minStoriesCount = generatorController.MinStoriesCount;
        GUILayout.Label("Floors count", EditorStyles.boldLabel, GUILayout.Width(150));

        GUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(GeneratorController.minFloors.ToString(), GUILayout.Width(10));
        EditorGUILayout.MinMaxSlider(ref minStoriesCount, ref maxStoriesCount, GeneratorController.minFloors, GeneratorController.maxFloors, GUILayout.MaxWidth(maxWidth));
        EditorGUILayout.LabelField(GeneratorController.maxFloors.ToString(), GUILayout.Width(10));

        generatorController.MaxStoriesCount = maxStoriesCount;
        generatorController.MinStoriesCount = minStoriesCount;
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal(GUILayout.MaxWidth(maxWidth));

        float segmentWidth = maxWidth / (GeneratorController.maxFloors - 1);

        for (int i = GeneratorController.minFloors; i <= GeneratorController.maxFloors; i++)
        {

            EditorGUILayout.LabelField(i.ToString(), GUILayout.MaxWidth(segmentWidth));


        }
        GUILayout.EndHorizontal();

    }
    void CustomSizeSettings()
    {
        GUILayout.Label("Custom building size", EditorStyles.boldLabel, GUILayout.Width(150));
        GUILayout.Space(10);
        generatorController.useCustomBuildingSize = GUILayout.Toggle(generatorController.useCustomBuildingSize, "Use custom size");
        GUILayout.Space(5);
        generatorController.customBuildingSizeX = EditorGUILayout.Slider("X size",generatorController.customBuildingSizeX, GeneratorController.minSizeX, GeneratorController.maxSizeX);
        generatorController.customBuildingSizeZ = EditorGUILayout.Slider("Z size", generatorController.customBuildingSizeZ, GeneratorController.minSizeZ, GeneratorController.maxSizeZ);

    }
    void ControlButtons()
    {
        Color defaultColor = GUI.backgroundColor;
        GUI.backgroundColor = Color.green;
        generateButton.fontStyle = FontStyle.Bold;

        generateButton = "button";
        if (GUILayout.Button("Generate Building", generateButton, GUILayout.Width(130), GUILayout.Height(40)))
        {
            generatorController.Generate();
        }

        GUI.backgroundColor = defaultColor;
        generateButton = "button";
        GUILayout.Space(5);

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Export", GUILayout.Width(130), GUILayout.Height(30)))
        {
            if (!String.IsNullOrEmpty(generatorController.path))
            {
                ObjExporter.DoExport(false, generatorController.parentObj, generatorController.path);

            }
        }
        if (GUILayout.Button("Merge Mesh", GUILayout.Width(130), GUILayout.Height(30)))
        {
            generatorController.Merge();
        }

      GUILayout.EndHorizontal();
   
    }
    void TestButtons()
    {
        GUILayout.Label("Tests", EditorStyles.boldLabel, GUILayout.Width(150));
        GUILayout.Space(10);
        if (GUILayout.Button("Generate Building Test", GUILayout.Width(200)))
        {
            generatorController.GenerationTest();
        }
        GUILayout.Space(5);

    }
}
