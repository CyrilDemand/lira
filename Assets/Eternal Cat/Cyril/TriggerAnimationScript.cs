using UnityEngine;

public class TriggerAnimation : MonoBehaviour
{
    [SerializeField]
    private Animator animator;



    // Déclenche l'animation lorsque le joueur entre dans le trigger
    private void OnTriggerEnter2D(Collider2D other)
    {
        animator.SetTrigger("Open");
    }
}