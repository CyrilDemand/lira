using System;
using UnityEngine;

public class PnjMovementDetection : MonoBehaviour
{
    [SerializeField] private String characterName;
    // La position précédente du GameObject
    private Vector2 previousPosition;
    // Une marge pour les mouvements très petits
    public float movementThreshold = 0.01f;

    // Appelé avant le premier frame
    void Start()
    {
        // On initialise la position précédente à la position actuelle
        previousPosition = transform.position;
    }

    // Appelé à chaque frame
    void Update()
    {
        // On vérifie si le GameObject a bougé
        DetectMovement();
    }

    // Méthode pour détecter le mouvement
    void DetectMovement()
    {
        // Calcul de la différence de position
        float distanceMoved = Vector2.Distance(previousPosition, transform.position);

        // Si la différence est supérieure au seuil, on considère que le GameObject a bougé
        if (distanceMoved > movementThreshold)
        {
            GlobalGameManager.instance.SetPnjLocation(characterName, transform.position);
        }
        
        // Mise à jour de la position précédente
        previousPosition = transform.position;
    }
}