using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;
using System.IO;

[InitializeOnLoad]
public static class SceneSwitcher
{
    private static string[] scenes;

    static SceneSwitcher()
    {
        SceneView.duringSceneGui += OnSceneGUI;
        LoadScenes();
    }

    private static void OnSceneGUI(SceneView sceneView)
    {
        Handles.BeginGUI();
        GUILayout.BeginArea(new Rect(10, 50, 200, 100), EditorStyles.helpBox);

        GUILayout.Label("Scene Switcher", EditorStyles.boldLabel);

        if (GUILayout.Button("Refresh Scenes"))
        {
            LoadScenes();
        }

        if (scenes != null)
        {
            foreach (var scenePath in scenes)
            {
                string sceneName = Path.GetFileNameWithoutExtension(scenePath);
                if (GUILayout.Button(sceneName))
                {
                    OpenScene(scenePath);
                }
            }
        }

        GUILayout.EndArea();
        Handles.EndGUI();
    }

    private static void OpenScene(string scenePath)
    {
        if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
        {
            EditorSceneManager.OpenScene(scenePath);
        }
    }

    public static void LoadScenes()
    {
        int sceneCount = EditorBuildSettings.scenes.Length;
        scenes = new string[sceneCount];

        for (int i = 0; i < sceneCount; i++)
        {
            scenes[i] = EditorBuildSettings.scenes[i].path;
        }

        Debug.Log("Scenes refreshed!");
    }
}

[CustomEditor(typeof(EditorBuildSettingsScene))]
public class SceneSwitcherInspector : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (GUILayout.Button("Refresh Scene Switcher"))
        {
            SceneSwitcher.LoadScenes();
        }
    }
}
