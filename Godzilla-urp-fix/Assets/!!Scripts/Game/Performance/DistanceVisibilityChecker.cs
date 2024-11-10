using UnityEngine;
using Unity.Jobs;
using Unity.Collections;
using Unity.Burst;
using System.Collections.Generic;

public class PlayerVisibilityManager : MonoBehaviour
{
    public float activationDistance = 100f;
    public List<GameObject> buildingGameObjects;

    private Transform playerTransform;
    private List<BuildingComponents> buildings;
    private NativeArray<Vector3> buildingPositions;
    private NativeArray<bool> visibilityResults;

    void Start()
    {
        playerTransform = transform;
        buildings = new List<BuildingComponents>();

        // Cache references to the components of each building
        foreach (var buildingObject in buildingGameObjects)
        {
            IHealth healthComponent = buildingObject.GetComponent<IHealth>();
            if (healthComponent != null && healthComponent.GetMaxHealth() > 0)
            {
                BuildingComponents components = new BuildingComponents
                {
                    GameObject = buildingObject,
                    Health = healthComponent,
                    Colliders = buildingObject.GetComponentsInChildren<Collider>(),
                    Rigidbodies = buildingObject.GetComponentsInChildren<Rigidbody>(),
                    Renderers = buildingObject.GetComponentsInChildren<Renderer>()
                };
                buildings.Add(components);
                SetComponentsActive(components, false);
            }
        }

        // Initialize NativeArrays
        buildingPositions = new NativeArray<Vector3>(buildings.Count, Allocator.Persistent);
        visibilityResults = new NativeArray<bool>(buildings.Count, Allocator.Persistent);

        // Check visibility every second
        InvokeRepeating("CheckBuildingsVisibility", 1f, 1f);
    }

    void CheckBuildingsVisibility()
    {
        // Fill the building positions array
        for (int i = 0; i < buildings.Count; i++)
        {
            buildingPositions[i] = buildings[i].GameObject.transform.position;
        }

        // Schedule the job
        VisibilityCheckJob visibilityCheckJob = new VisibilityCheckJob
        {
            buildingPositions = buildingPositions,
            playerPosition = playerTransform.position,
            activationDistance = activationDistance,
            results = visibilityResults
        };

        JobHandle jobHandle = visibilityCheckJob.Schedule(buildingPositions.Length, 64);
        jobHandle.Complete();

        // Apply the results
        for (int i = 0; i < buildings.Count; i++)
        {
            bool isVisible = visibilityResults[i];
            SetComponentsActive(buildings[i], isVisible);
        }

        // Remove buildings with zero health
        buildings.RemoveAll(building => building.Health.GetMaxHealth() == 0);
    }

    void OnDestroy()
    {
        // Dispose of NativeArrays
        if (buildingPositions.IsCreated) buildingPositions.Dispose();
        if (visibilityResults.IsCreated) visibilityResults.Dispose();
    }

    private void SetComponentsActive(BuildingComponents components, bool isActive)
    {
        foreach (var collider in components.Colliders)
        {
            if (collider == null) return;
            collider.enabled = isActive;
        }

        foreach (var rigidbody in components.Rigidbodies)
        {
            rigidbody.detectCollisions = isActive;
            rigidbody.isKinematic = !isActive;
        }

        foreach (var renderer in components.Renderers)
        {
            renderer.enabled = isActive;
        }
    }

    [BurstCompile]
    struct VisibilityCheckJob : IJobParallelFor
    {
        [ReadOnly] public NativeArray<Vector3> buildingPositions;
        [ReadOnly] public Vector3 playerPosition;
        [ReadOnly] public float activationDistance;
        public NativeArray<bool> results;

        public void Execute(int index)
        {
            float distance = Vector3.Distance(buildingPositions[index], playerPosition);
            results[index] = distance < activationDistance;
        }
    }

    public class BuildingComponents
    {
        public GameObject GameObject;
        public IHealth Health;
        public Collider[] Colliders;
        public Rigidbody[] Rigidbodies;
        public Renderer[] Renderers;
    }
}
