using UnityEngine;

public class PlayerVisibilityHandler : MonoBehaviour
{
    public Material normalMaterial;
    public Material glowingMaterial;
    public SpriteRenderer spriteRenderer;

    public string normalSortingLayer = "Default";
    public string glowingSortingLayer = "AlwaysOnTop";
    public int normalOrderInLayer = 0;
    public int glowingOrderInLayer = 10;

    public LayerMask obstructionLayer;

    private void Start()
    {
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        spriteRenderer.material = normalMaterial;
        spriteRenderer.sortingLayerName = normalSortingLayer;
        spriteRenderer.sortingOrder = normalOrderInLayer;
    }

    void Update()
    {
        if (Camera.main == null) return;
        RaycastHit hit;
        Vector3 cameraPosition = Camera.main.transform.position;
        Vector3 playerPosition = transform.position;

        if (Physics.Linecast(cameraPosition, playerPosition, out hit, obstructionLayer))
        {
            if (hit.transform != transform)
            {
                // Obstruction detected
                spriteRenderer.material = glowingMaterial;
                spriteRenderer.sortingLayerName = glowingSortingLayer;
                spriteRenderer.sortingOrder = glowingOrderInLayer;
            }
            else
            {
                // No obstruction
                spriteRenderer.material = normalMaterial;
                spriteRenderer.sortingLayerName = normalSortingLayer;
                spriteRenderer.sortingOrder = normalOrderInLayer;
            }
        }
        else
        {
            // No obstruction
            spriteRenderer.material = normalMaterial;
            spriteRenderer.sortingLayerName = normalSortingLayer;
            spriteRenderer.sortingOrder = normalOrderInLayer;
        }
    }
}
