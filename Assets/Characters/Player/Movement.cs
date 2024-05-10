using System.Collections;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Rigidbody2D rb;
    private Vector2 movementInput;
    private float currentSpeed = 0f;
    private Coroutine movementCoroutine;
    private bool isMoving = false;

    [SerializeField]
    private Animator animator;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    void Update()
    {
        if (!DialogueManager.getInstance().dialogueIsPlaying)
        {
            // Récupérer l'entrée du joueur
            movementInput = InputManager.instance.GetMovementInput();
            handleAnimation(movementInput);
            // Si un mouvement est détecté
            if (movementInput != Vector2.zero && !isMoving)
            {
                isMoving = true;

                // Annule la coroutine précédente
                if (movementCoroutine != null)
                    StopCoroutine(movementCoroutine);

                // Démarrer l'accélération
                movementCoroutine = StartCoroutine(Accelerate());
            }
            // Si le joueur arrête de bouger
            else if (movementInput == Vector2.zero && isMoving)
            {
                isMoving = false;

                // Annule la coroutine précédente
                if (movementCoroutine != null)
                    StopCoroutine(movementCoroutine);

                // Démarrer la décélération
                movementCoroutine = StartCoroutine(Decelerate());
            }
            else if (isMoving)
            {
                // Appliquer la vitesse actuelle dans la direction du mouvement
                rb.velocity = movementInput * currentSpeed;
            }
        }
        else
        {
            // Si le dialogue est en cours, stoppez le mouvement
            rb.velocity = Vector2.zero;
            currentSpeed = 0f;
            isMoving = false;

            if (movementCoroutine != null)
                StopCoroutine(movementCoroutine);
        }
    }

    private void handleAnimation(Vector2 movementInput)
    {
        if (movementInput.x>0)
        {
            animator.SetFloat("x", 1);
        }else if (movementInput.x<0)
        {
            animator.SetFloat("x", -1);
        }else if (movementInput.y>0)
        {
            animator.SetFloat("y", 1);
        }else if (movementInput.y<0)
        {
            animator.SetFloat("y", -1);
        }
        else
        {
            animator.SetFloat("x", 0);
            animator.SetFloat("y", 0);
        }
    }

    private IEnumerator Accelerate()
    {
        float targetSpeed = moveSpeed;
        float accelerationStep = targetSpeed / 6f; // Accélérer en 6 étapes

        while (currentSpeed < targetSpeed)
        {
            currentSpeed = Mathf.Min(currentSpeed + accelerationStep, targetSpeed);
            rb.velocity = movementInput * currentSpeed;
            yield return null; // Attend une frame
        }
    }

    private IEnumerator Decelerate()
    {
        float decelerationStep = currentSpeed / 3f; // Décélérer en 3 étapes

        while (currentSpeed > 0f)
        {
            currentSpeed = Mathf.Max(currentSpeed - decelerationStep, 0f);
            rb.velocity = movementInput * currentSpeed;
            yield return null; // Attend une frame
        }

        rb.velocity = Vector2.zero;
    }
}
