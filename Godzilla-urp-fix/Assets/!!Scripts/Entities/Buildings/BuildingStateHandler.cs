using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

// This is for the 2d building, this script is considered depreciated for now.
public class BuildingStateHandler : MonoBehaviour
{
    [System.Serializable]
    public class HealthState
    {
        public int healthValue;
        public Sprite sprite;
        public GameObject prefabToEnable;
    }

    [SerializeField, Tooltip("The SpriteRenderer to update with different sprites.")]
    private SpriteRenderer buildingSpriteRenderer;

    [SerializeField, Tooltip("List of health states with corresponding sprites and prefabs to enable.")]
    private HealthState[] healthStates;

    private IHealth health;

    private void Awake()
    {
        health = GetComponent<IHealth>();
        if (health != null)
        {
            Health healthComponent = health as Health;
            if (healthComponent != null)
            {
                healthComponent.OnHealthChanged.AddListener(UpdateBuildingState);
            }
        }
        UpdateBuildingState(health.GetCurrentHealth());
    }

    private void UpdateBuildingState(int currentHealth)
    {
        foreach (var state in healthStates)
        {
            if (state.healthValue == currentHealth)
            {
                if (buildingSpriteRenderer != null && state.sprite != null)
                {
                    buildingSpriteRenderer.sprite = state.sprite;
                }

                if (state.prefabToEnable != null)
                {
                    state.prefabToEnable.SetActive(true);
                }
            }
            else
            {
                if (state.prefabToEnable != null)
                {
                    state.prefabToEnable.SetActive(false);
                }
            }
        }
    }

    public void TestDamage(int damageAmount)
    {
        if (health != null)
        {
            health.TakeDamage(damageAmount);
        }
    }
}


