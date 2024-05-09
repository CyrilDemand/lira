using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    void Update()
    {
        if (!DialogueManager.getInstance().dialogueIsPlaying)
        {
            // Obtenez le vecteur de mouvement depuis l'Input Manager
            Vector2 movementInput = InputManager.instance.GetMovementInput();
            rb.velocity = movementInput * moveSpeed; 
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
        
    }
}