using UnityEngine;
using System.Collections.Generic;

public class MeshSlicer : MonoBehaviour
{
    [SerializeField, Tooltip("Number of cuts along the X axis.")]
    private int cutsX = 3;
    [SerializeField, Tooltip("Number of cuts along the Y axis.")]
    private int cutsY = 3;
    [SerializeField, Tooltip("Number of cuts along the Z axis.")]
    private int cutsZ = 3;
    [SerializeField, Tooltip("Explode force applied to each piece.")]
    private float explodeForce = 10f;

    public void SliceIntoBlocks()
    {
        Mesh originalMesh = GetComponent<MeshFilter>().mesh;
        if (!originalMesh.isReadable)
        {
            Debug.LogError("Mesh is not readable. Enable 'Read/Write Enabled' in the import settings of the mesh.");
            return;
        }

        Vector3[] vertices = originalMesh.vertices;
        Vector3 min = originalMesh.bounds.min;
        Vector3 max = originalMesh.bounds.max;

        float stepX = (max.x - min.x) / cutsX;
        float stepY = (max.y - min.y) / cutsY;
        float stepZ = (max.z - min.z) / cutsZ;

        List<GameObject> pieces = new List<GameObject>();

        for (int x = 0; x < cutsX; x++)
        {
            for (int y = 0; y < cutsY; y++)
            {
                for (int z = 0; z < cutsZ; z++)
                {
                    Vector3 partMin = new Vector3(min.x + stepX * x, min.y + stepY * y, min.z + stepZ * z);
                    Vector3 partMax = new Vector3(min.x + stepX * (x + 1), min.y + stepY * (y + 1), min.z + stepZ * (z + 1));

                    Mesh partMesh = CreateMeshPart(vertices, partMin, partMax);
                    if (partMesh != null)
                    {
                        GameObject piece = CreatePiece(partMesh);
                        pieces.Add(piece);
                    }
                }
            }
        }

        foreach (GameObject piece in pieces)
        {
            Rigidbody rb = piece.AddComponent<Rigidbody>();
            rb.AddExplosionForce(explodeForce, transform.position, 10f);
        }

        Destroy(gameObject);
    }

    private Mesh CreateMeshPart(Vector3[] vertices, Vector3 min, Vector3 max)
    {
        List<Vector3> partVertices = new List<Vector3>();
        List<int> partTriangles = new List<int>();

        for (int i = 0; i < vertices.Length; i++)
        {
            if (vertices[i].x >= min.x && vertices[i].x <= max.x &&
                vertices[i].y >= min.y && vertices[i].y <= max.y &&
                vertices[i].z >= min.z && vertices[i].z <= max.z)
            {
                partVertices.Add(vertices[i]);
                partTriangles.Add(partVertices.Count - 1);
            }
        }

        if (partVertices.Count < 3) return null;

        Mesh partMesh = new Mesh();
        partMesh.vertices = partVertices.ToArray();
        partMesh.triangles = partTriangles.ToArray();
        partMesh.RecalculateNormals();

        return partMesh;
    }

    private GameObject CreatePiece(Mesh partMesh)
    {
        GameObject piece = new GameObject("Piece");
        piece.transform.position = transform.position;
        piece.transform.rotation = transform.rotation;
        piece.transform.localScale = transform.localScale;

        MeshFilter mf = piece.AddComponent<MeshFilter>();
        mf.mesh = partMesh;

        MeshRenderer mr = piece.AddComponent<MeshRenderer>();
        mr.materials = GetComponent<MeshRenderer>().materials;

        MeshCollider mc = piece.AddComponent<MeshCollider>();
        mc.convex = true;

        return piece;
    }
}
