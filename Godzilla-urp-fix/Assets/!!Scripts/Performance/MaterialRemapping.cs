#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public class MaterialRemapping : EditorWindow
{
    private string materialFolder = "Assets/Materials"; // Default material folder path
    private GameObject prefabToProcess;

    [MenuItem("Tools/Material Remapper")]
    public static void ShowWindow()
    {
        GetWindow<MaterialRemapping>("Material Remapper");
    }

    private void OnGUI()
    {
        GUILayout.Label("Material Remapper", EditorStyles.boldLabel);

        EditorGUILayout.Space();

        // Material folder field
        GUILayout.Label("Material Folder (contains materials with GPU instancing enabled):");
        materialFolder = EditorGUILayout.TextField(materialFolder);

        EditorGUILayout.Space();

        // Prefab field
        GUILayout.Label("Prefab to Process:");
        prefabToProcess = (GameObject)EditorGUILayout.ObjectField(prefabToProcess, typeof(GameObject), false);

        EditorGUILayout.Space();

        if (GUILayout.Button("Remap Materials"))
        {
            if (prefabToProcess != null && !string.IsNullOrEmpty(materialFolder))
            {
                RemapMaterialsInPrefab(prefabToProcess, materialFolder);
            }
            else
            {
                EditorUtility.DisplayDialog("Error", "Please specify both the material folder and the prefab to process.", "OK");
            }
        }
    }

    private void RemapMaterialsInPrefab(GameObject prefab, string materialFolderPath)
    {
        // Check if the material folder exists
        if (!AssetDatabase.IsValidFolder(materialFolderPath))
        {
            EditorUtility.DisplayDialog("Error", "The specified material folder does not exist.", "OK");
            return;
        }

        // Load all materials from the folder into a dictionary for quick lookup
        string[] materialGUIDs = AssetDatabase.FindAssets("t:Material", new[] { materialFolderPath });
        var materialDict = new System.Collections.Generic.Dictionary<string, Material>();

        foreach (string guid in materialGUIDs)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            Material mat = AssetDatabase.LoadAssetAtPath<Material>(path);
            if (mat != null)
            {
                materialDict[mat.name] = mat;
            }
        }

        if (materialDict.Count == 0)
        {
            EditorUtility.DisplayDialog("Error", "No materials found in the specified folder.", "OK");
            return;
        }

        // Get the prefab path
        string prefabPath = AssetDatabase.GetAssetPath(prefab);

        // Load the prefab contents
        GameObject prefabContents = PrefabUtility.LoadPrefabContents(prefabPath);

        // Find all MeshRenderer components in the prefab contents
        MeshRenderer[] meshRenderers = prefabContents.GetComponentsInChildren<MeshRenderer>();

        int remappedMaterials = 0;

        foreach (MeshRenderer renderer in meshRenderers)
        {
            Material[] materials = renderer.sharedMaterials;
            bool materialsChanged = false;

            for (int i = 0; i < materials.Length; i++)
            {
                Material oldMaterial = materials[i];
                if (oldMaterial != null)
                {
                    if (materialDict.TryGetValue(oldMaterial.name, out Material newMaterial))
                    {
                        materials[i] = newMaterial;
                        materialsChanged = true;
                        remappedMaterials++;
                        Debug.Log($"Remapped material '{oldMaterial.name}' to '{newMaterial.name}' on renderer '{renderer.gameObject.name}'.");
                    }
                }
            }

            if (materialsChanged)
            {
                renderer.sharedMaterials = materials;
            }
        }

        // Save the changes
        PrefabUtility.SaveAsPrefabAsset(prefabContents, prefabPath);

        // Unload the prefab contents
        PrefabUtility.UnloadPrefabContents(prefabContents);

        EditorUtility.DisplayDialog("Material Remapping Complete", $"Remapped {remappedMaterials} materials in prefab '{prefab.name}'.", "OK");
    }
}
#endif