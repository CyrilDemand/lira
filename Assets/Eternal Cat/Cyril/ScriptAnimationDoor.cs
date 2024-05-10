using UnityEngine;

public class LiramillionDoorController : MonoBehaviour
{
    [SerializeField]
    private Animator animator;

    [SerializeField] 
    private Animator linkedAnimator;
    
    private bool isOpened = false;
    private bool isLinkedOpened = false;
    
    private Transform player;
    private bool isPlayerInTrigger = false;
    
   private void Start()
    {
        // Initialiser les états selon les variables d'animation actuelles
        isOpened = animator.GetBool("isOpened");
        isLinkedOpened = linkedAnimator.GetBool("isOpened");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Ouvrir si la porte n'est pas déjà ouverte
            if (!isOpened)
                Open(animator, ref isOpened);

            // Ouvrir la porte liée si elle n'est pas déjà ouverte
            if (!isLinkedOpened)
                Open(linkedAnimator, ref isLinkedOpened);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Fermer la porte si elle n'est pas déjà fermée
            if (isOpened)
                Close(animator, ref isOpened);

            // Fermer la porte liée si elle n'est pas déjà fermée
            if (isLinkedOpened)
                Close(linkedAnimator, ref isLinkedOpened);
        }
    }

    private void Open(Animator anim, ref bool isOpenedFlag)
    {
        anim.SetTrigger("StartOpening");
        isOpenedFlag = true;
    }

    private void Close(Animator anim, ref bool isOpenedFlag)
    {
        anim.SetTrigger("StartClosing");
        isOpenedFlag = false;
    }

    // Fonction appelée à la fin de l'animation Opening
    public void OnOpeningComplete()
    {
        animator.SetBool("isOpened", true);
        animator.SetBool("isClosed", false);

        linkedAnimator.SetBool("isOpened", true);
        linkedAnimator.SetBool("isClosed", false);

        isOpened = true;
        isLinkedOpened = true;
    }

    // Fonction appelée à la fin de l'animation Closing
    public void OnClosingComplete()
    {
        animator.SetBool("isClosed", true);
        animator.SetBool("isOpened", false);

        linkedAnimator.SetBool("isClosed", true);
        linkedAnimator.SetBool("isOpened", false);

        isOpened = false;
        isLinkedOpened = false;
    }
}