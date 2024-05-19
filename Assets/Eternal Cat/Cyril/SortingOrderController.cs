using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SortingOrderController : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // L'ordre de tri est basé sur la position y de l'objet
        spriteRenderer.sortingOrder = Mathf.RoundToInt(transform.position.y * -100);
    }
}