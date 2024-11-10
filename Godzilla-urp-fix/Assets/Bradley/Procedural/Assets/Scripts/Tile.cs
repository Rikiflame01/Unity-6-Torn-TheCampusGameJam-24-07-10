//using UnityEngine;

//public class Tile : MonoBehaviour
//{
//    public Tile[] upNeighbours;
//    public Tile[] rightNeighbours;
//    public Tile[] downNeighbours;
//    public Tile[] leftNeighbours;

//    private void Awake()
//    {

//        transform.localScale = Vector3.one;


//    }
//}

using UnityEngine;

[ExecuteInEditMode]  // This allows the script to run in the editor
public class Tile : MonoBehaviour
{
    public Tile[] upNeighbours;
    public Tile[] rightNeighbours;
    public Tile[] downNeighbours;
    public Tile[] leftNeighbours;

    private void Awake()
    {
        transform.localScale = Vector3.one;

        // Additional logic can be added here to ensure it runs in both the editor and during play mode
        if (!Application.isPlaying)
        {
            // Editor-specific initialization
            OnEditorAwake();
        }
    }

    private void OnEditorAwake()
    {
        // Any additional setup that is editor-specific can go here
    }
}

