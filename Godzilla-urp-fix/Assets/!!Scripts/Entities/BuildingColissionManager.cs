using UnityEngine;

public class BuildingCollisionManager : MonoBehaviour
{
    [SerializeField] private string buildingLayerName = "Buildings";

    void Start()
    {
        int buildingLayer = LayerMask.NameToLayer(buildingLayerName);

        if (buildingLayer < 0)
        {
            return;
        }

        Physics.IgnoreLayerCollision(buildingLayer, buildingLayer);

    }
}
