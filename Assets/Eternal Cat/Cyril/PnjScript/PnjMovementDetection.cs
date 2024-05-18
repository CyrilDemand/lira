using System;
using UnityEngine;

public class PnjMovementDetection : MonoBehaviour
{
    [SerializeField] private String characterName;

    private string direction;
    // La position précédente du GameObject
    private Vector2 previousPosition;

    private Vector2 currentPosition;
    // Une marge pour les mouvements très petits
    public float movementThreshold = 0.01f;
    [SerializeField]
    private Animator animator;
    
    // pour savoir si l'objet a des animations qui s'arrêtent à la dernière frame
    [SerializeField]
    private Boolean isLastFrameStopped;
    private bool isAnimationPlaying = true;
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
        currentPosition = transform.position;
        float distanceMoved = Vector2.Distance(previousPosition, currentPosition);
        
        HandleAnimation();

        // Si la différence est supérieure au seuil, on considère que le GameObject a bougé
        if (distanceMoved > movementThreshold)
        {
            GlobalGameManager.instance.SetPnjLocation(characterName, transform.position);
        }
        
        // Mise à jour de la position précédente
        previousPosition = transform.position;
    }
    
    public void HandleAnimation()
    {
        string tmp = direction;
        bool isMoving = true;

        if (currentPosition.x > previousPosition.x)
        {
            direction = "droite";
            animator.SetFloat("x", 1);
            animator.SetFloat("y", 0);
        }
        else if (currentPosition.x < previousPosition.x)
        {
            direction = "gauche";
            animator.SetFloat("x", -1);
            animator.SetFloat("y", 0);
        }
        else if (currentPosition.y > previousPosition.y)
        {
            animator.SetFloat("y", 1);
            animator.SetFloat("x", 0);
        }
        else if (currentPosition.y < previousPosition.y)
        {
            animator.SetFloat("y", -1);
            animator.SetFloat("x", 0);
        }
        else
        {
            animator.SetFloat("x", 0);
            animator.SetFloat("y", 0);
            isMoving = false;
        }

        if (direction != tmp)
        {
            Debug.Log("changement de direction");
        }
        
        if (isAnimationPlaying && isLastFrameStopped)
        {
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            if (stateInfo.normalizedTime >= 0.99f && stateInfo.normalizedTime < 1.0f && !animator.IsInTransition(0))
            {
                animator.speed = 0;
                isAnimationPlaying = false;
            }
        }
        
    }
}