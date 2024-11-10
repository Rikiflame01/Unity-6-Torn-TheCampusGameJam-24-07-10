using UnityEngine;
using UnityEditor;
using System.IO;

public class MaterialColourGenerator : EditorWindow
{
    private string materialsFolder = "Assets/Materials";

    [MenuItem("Tools/Generate 32 Colored Materials")]
    public static void ShowWindow()
    {
        GetWindow<MaterialColourGenerator>("Generate Colored Materials");
    }


    private void OnGUI()
    {
        GUILayout.Label("Generate 32 Colored Materials", EditorStyles.boldLabel);

        materialsFolder = EditorGUILayout.TextField("Materials Folder", materialsFolder);

        if (GUILayout.Button("Generate Materials"))
        {
            GenerateMaterials();
        }
    }

    private void GenerateMaterials()
    {
        if (!Directory.Exists(materialsFolder))
        {
            Directory.CreateDirectory(materialsFolder);
        }

        for (int i = 0; i < 32; i++)
        {
            Color color = Random.ColorHSV();
            string materialName = "Material_" + (i + 1);
            string materialPath = Path.Combine(materialsFolder, materialName + ".mat");

            Material material = new Material(Shader.Find("Standard"));
            material.color = color;

            AssetDatabase.CreateAsset(material, materialPath);
            Debug.Log("Created material: " + materialPath);
        }

        AssetDatabase.Refresh();
    }
}
