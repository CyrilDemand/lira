using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class CapeSortingOrderController : MonoBehaviour
{
    private SpriteRenderer capeSpriteRenderer;
    private SpriteRenderer pnjSpriteRenderer;
    private PnjMovementDetection pnjMovement;

    void Start()
    {
        capeSpriteRenderer = GetComponent<SpriteRenderer>();
        Transform pnjTransform = transform.parent;
        if (pnjTransform != null)
        {
            pnjSpriteRenderer = pnjTransform.GetComponent<SpriteRenderer>();
            pnjMovement = pnjTransform.GetComponent<PnjMovementDetection>();
        }
    }

    void Update()
    {
        if (pnjSpriteRenderer != null && pnjMovement != null)
        {
            // Assurez-vous que la cape suit la même logique de tri de base que le PNJ
            capeSpriteRenderer.sortingOrder = pnjSpriteRenderer.sortingOrder;

            // Obtenez la direction du mouvement du PNJ
            string movementDirection = pnjMovement.GetCurrentDirection();

            // Ajustez l'ordre de tri en fonction de la direction
            if (movementDirection == "droite" || movementDirection == "haut" || movementDirection == "gauche")
            {
                // Déplacement vers la droite ou vers le haut
                capeSpriteRenderer.sortingOrder = pnjSpriteRenderer.sortingOrder + 1;
            }
            else if ( movementDirection == "bas")
            {
                // Déplacement vers la gauche ou vers le bas
                capeSpriteRenderer.sortingOrder = pnjSpriteRenderer.sortingOrder - 1;
            }
        }
    }
}