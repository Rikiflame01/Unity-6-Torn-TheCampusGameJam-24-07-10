using UnityEngine;

public class MeshDeformerWarp : MonoBehaviour
{
    private Mesh mesh;
    private Vector3[] originalVertices, displacedVertices;
    private Vector3[] vertexVelocities;

    [SerializeField, Tooltip("The amount of damage applied to the mesh.")]
    private float damageAmount = 0.1f;

    [SerializeField, Tooltip("The radius of the damage effect.")]
    private float damageRadius = 0.5f;

    [SerializeField, Tooltip("The spring force applied to vertices.")]
    private float springForce = 20f;

    [SerializeField, Tooltip("The damping force applied to vertices.")]
    private float damping = 5f;

    private void Start()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        if (!mesh.isReadable)
        {
            Debug.LogError("Mesh is not readable. Enable 'Read/Write Enabled' in the import settings of the mesh.");
            return;
        }

        originalVertices = mesh.vertices;
        displacedVertices = new Vector3[originalVertices.Length];
        for (int i = 0; i < originalVertices.Length; i++)
        {
            displacedVertices[i] = originalVertices[i];
        }
        vertexVelocities = new Vector3[originalVertices.Length];
    }

    private void Update()
    {
        for (int i = 0; i < displacedVertices.Length; i++)
        {
            UpdateVertex(i);
        }
        mesh.vertices = displacedVertices;
        mesh.RecalculateNormals();
    }

    private void UpdateVertex(int i)
    {
        Vector3 velocity = vertexVelocities[i];
        Vector3 displacement = displacedVertices[i] - originalVertices[i];
        velocity -= displacement * springForce * Time.deltaTime;
        velocity *= 1f - damping * Time.deltaTime;
        vertexVelocities[i] = velocity;
        displacedVertices[i] += velocity * Time.deltaTime;
    }

    public void ApplyDamage(Vector3 point, float radius)
    {
        Debug.Log("Applying damage at point: " + point + " with radius: " + radius);
        point = transform.InverseTransformPoint(point);

        for (int i = 0; i < displacedVertices.Length; i++)
        {
            Vector3 pointToVertex = displacedVertices[i] - point;
            float attenuatedDamage = damageAmount * Mathf.Exp(-pointToVertex.sqrMagnitude / (2f * radius * radius));

            if (pointToVertex.sqrMagnitude < radius * radius)
            {
                float velocity = attenuatedDamage;
                vertexVelocities[i] += pointToVertex.normalized * velocity;
            }

            Debug.Log("Vertex " + i + " displaced to: " + displacedVertices[i]);
        }
    }
}
