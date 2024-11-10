using UnityEngine;
using UnityEditor;

public class GridLayoutEditor : EditorWindow
{
    [Header("Grid Settings")]
    public int numberOfColumns = 5;
    public float spacing = 2.0f;
    public float roadWidth = 3.0f;
    public GameObject gridElementPrefab;

    private SerializedObject serializedObject;
    private SerializedProperty numberOfColumnsProp;
    private SerializedProperty spacingProp;
    private SerializedProperty roadWidthProp;
    private SerializedProperty gridElementPrefabProp;

    [MenuItem("Window/Grid Layout Editor")]
    public static void ShowWindow()
    {
        GetWindow<GridLayoutEditor>("Grid Layout Editor");
    }

    private void OnEnable()
    {
        serializedObject = new SerializedObject(this);
        numberOfColumnsProp = serializedObject.FindProperty("numberOfColumns");
        spacingProp = serializedObject.FindProperty("spacing");
        roadWidthProp = serializedObject.FindProperty("roadWidth");
        gridElementPrefabProp = serializedObject.FindProperty("gridElementPrefab");
    }

    private void OnGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(numberOfColumnsProp);
        EditorGUILayout.PropertyField(spacingProp);
        EditorGUILayout.PropertyField(roadWidthProp);
        EditorGUILayout.PropertyField(gridElementPrefabProp);

        if (GUILayout.Button("Generate Grid"))
        {
            CreateGrid();
        }

        serializedObject.ApplyModifiedProperties();
    }

    private void CreateGrid()
    {
        if (gridElementPrefab == null)
        {
            Debug.LogError("Grid Element Prefab is not assigned.");
            return;
        }

        if (Selection.activeTransform == null)
        {
            Debug.LogError("No GameObject selected in the Hierarchy. Please select a GameObject to attach the grid elements.");
            return;
        }

        // Clean up old elements
        while (Selection.activeTransform.childCount > 0)
        {
            DestroyImmediate(Selection.activeTransform.GetChild(0).gameObject);
        }

        for (int row = 0; row < 3; row++)
        {
            float yPos = row * (spacing + roadWidth);

            for (int col = 0; col < numberOfColumns; col++)
            {
                float xPos = col * spacing;
                Vector3 position = new Vector3(xPos, 0, yPos);
                GameObject instance = PrefabUtility.InstantiatePrefab(gridElementPrefab) as GameObject;
                instance.transform.position = position;
                instance.transform.SetParent(Selection.activeTransform);
            }
        }
    }
}
