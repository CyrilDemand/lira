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
    private Animator animator;
    
    private Vector2 previousPosition;
    private Vector2 currentVelocity;

    private bool isStopped = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    void Update()
    {
        // Calculer la nouvelle vélocité basée sur le changement de position
        Vector2 currentPosition = transform.position;
        currentVelocity = (currentPosition - previousPosition) / Time.deltaTime;
        previousPosition = currentPosition;  // Mise à jour de la position précédente
        
        if (!DialogueManager.getInstance().dialogueIsPlaying && !isStopped)
        {
            HandleMovement();
            HandleAnimation(movementInput);
        }
        else
        {
            StopMovement();
        }
    }

    private void HandleMovement()
    {
        if (transform.CompareTag("Player"))
        {
            movementInput = InputManager.instance.GetMovementInput();
        }
        else
        {
            movementInput = currentVelocity;  // Utilisé pour les PNJ hypothétiquement
        }
        
        if (movementInput != Vector2.zero)
        {
            StartMovement();
        }
        else
        {
            StopMovement();
        }
    }

    private void StartMovement()
    {
        if (!isMoving)
        {
            isMoving = true;
            StartOrRestartCoroutine(Accelerate());
        }
        else
        {
            rb.velocity = movementInput * currentSpeed;
        }
    }

    public void changeIsStopped()
    {
        Debug.Log("changement isStopped");
        isStopped = !isStopped;
    }

    private void StopMovement()
    {
        if (isMoving)
        {
            isMoving = false;
            StartOrRestartCoroutine(Decelerate());
        }
    }

    private void StartOrRestartCoroutine(IEnumerator coroutineMethod)
    {
        if (movementCoroutine != null)
        {
            StopCoroutine(movementCoroutine);
        }
        movementCoroutine = StartCoroutine(coroutineMethod);
    }

    private IEnumerator Accelerate()
    {
        float targetSpeed = moveSpeed;
        float accelerationStep = targetSpeed / 6f;
        while (currentSpeed < targetSpeed)
        {
            currentSpeed += accelerationStep;
            rb.velocity = movementInput * currentSpeed;
            yield return null;
        }
    }

    private IEnumerator Decelerate()
    {
        float decelerationStep = currentSpeed / 3f;
        while (currentSpeed > 0f)
        {
            currentSpeed -= decelerationStep;
            rb.velocity = movementInput * currentSpeed;
            yield return null;
        }
        rb.velocity = Vector2.zero;
    }

    public void HandleAnimation(Vector2 movementInput)
    {
        if (movementInput.x > 0)
        {
            animator.SetFloat("x", 1);
            animator.SetFloat("y", 0);
        }else if (movementInput.x < 0)
        {
            animator.SetFloat("x", -1);
            animator.SetFloat("y", 0);
        }else if (movementInput.y > 0)
        {
            animator.SetFloat("y", 1);
            animator.SetFloat("x", 0);
        }else if (movementInput.y < 0)
        {
            animator.SetFloat("y", -1);
            animator.SetFloat("x", 0);
        }
        else
        {
            animator.SetFloat("x", 0);
            animator.SetFloat("y", 0);
        }
    }
}
