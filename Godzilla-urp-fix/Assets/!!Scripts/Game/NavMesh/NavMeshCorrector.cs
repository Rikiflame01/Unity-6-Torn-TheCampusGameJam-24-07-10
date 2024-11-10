using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshCorrector : MonoBehaviour
{
    public NavMeshSurface navMeshSurface;

    void Start()
    {
        navMeshSurface.transform.position += Vector3.up * 0.1f;
        navMeshSurface.BuildNavMesh();
    }
}
