//using UnityEngine;
//using UnityEditor;

//[CustomEditor(typeof(WaveFunctionCollapse))]
//public class WaveFunctionCollapseEditor : Editor
//{
//    public override void OnInspectorGUI()
//    {
//        DrawDefaultInspector();

//        WaveFunctionCollapse waveFunctionCollapse = (WaveFunctionCollapse)target;

//        if (GUILayout.Button("Generate City"))
//        {
//            waveFunctionCollapse.StartGeneration();
//        }

//        if (GUILayout.Button("Delete Generated Buildings"))
//        {
//            waveFunctionCollapse.DeleteGeneratedBuildings();
//        }
//    }
//}

//using UnityEngine;
//using UnityEditor;

//[CustomEditor(typeof(WaveFunctionCollapse))]
//public class WaveFunctionCollapseEditor : Editor
//{
//    public override void OnInspectorGUI()
//    {
//        DrawDefaultInspector();

//        WaveFunctionCollapse waveFunctionCollapse = (WaveFunctionCollapse)target;

//        // Check if there are any buildings under mapParent
//        bool hasGeneratedBuildings = waveFunctionCollapse.mapParent != null && waveFunctionCollapse.mapParent.transform.childCount > 0;

//        // Change button label based on whether buildings are present
//        string buttonLabel = hasGeneratedBuildings ? "Regenerate City" : "Generate City";

//        if (GUILayout.Button(buttonLabel))
//        {
//            if (hasGeneratedBuildings)
//            {
//                waveFunctionCollapse.DeleteGeneratedBuildings(); 
//            }
//            waveFunctionCollapse.StartGeneration(); 
//        }

//        if (GUILayout.Button("Delete Generated Buildings"))
//        {
//            waveFunctionCollapse.DeleteGeneratedBuildings();
//        }
//    }
//}