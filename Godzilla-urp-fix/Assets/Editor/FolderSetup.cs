using UnityEngine;
using UnityEditor;
using System.IO;

public class FolderSetup : EditorWindow
{
    [MenuItem("Tools/Generate Project Folders")]
    public static void ShowWindow()
    {
        GetWindow<FolderSetup>("Generate Project Folders");
    }

    private void OnGUI()
    {
        GUILayout.Label("Generate Initial Project Folders", EditorStyles.boldLabel);

        if (GUILayout.Button("Generate Folders"))
        {
            GenerateFolders();
        }
    }

    private static void GenerateFolders()
    {
        string[] folders = new string[]
        {
            "Assets/Animations",
            "Assets/Audio",
            "Assets/Fonts",
            "Assets/Materials",
            "Assets/Models",
            "Assets/Prefabs",
            "Assets/Resources",
            "Assets/Scripts",
            "Assets/Scenes",
            "Assets/Shaders",
            "Assets/Sprites",
            "Assets/Textures",
            "Assets/UI"
        };

        foreach (string folder in folders)
        {
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
                Debug.Log("Created folder: " + folder);
            }
            else
            {
                Debug.Log("Folder already exists: " + folder);
            }
        }

        AssetDatabase.Refresh();
    }
}
