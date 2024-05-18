using UnityEngine;
using System.Collections;
public class Teleporter : MonoBehaviour
{
    // Position cible pour la téléportation
    [SerializeField] public Vector2 destination;

    // Angle en degrés qui définit un cône de téléportation
    [SerializeField] private float detectionAngle = 45f;

    [SerializeField] public GameObject roomDestination;
    
    [SerializeField] public GameObject roomWhereIsTheTP;
    // Détecter les entrées dans le collider attaché à ce GameObject
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Obtenir la direction du GameObject entrant par rapport au téléporteur
        Vector2 directionToTeleporter = destination - (Vector2)other.transform.position;

        // Obtenir la direction du mouvement du GameObject
        Vector2 movementDirection = other.attachedRigidbody != null ? other.attachedRigidbody.velocity : Vector2.zero;

        // Vérifier que le mouvement se fait dans la direction du téléporteur
        if (movementDirection != Vector2.zero)
        {
            float angleBetween = Vector2.Angle(movementDirection, directionToTeleporter);

            // Afficher l'angle pour débogage
            Debug.Log($"Angle entre le mouvement et le téléporteur : {angleBetween}");

            // Assurez-vous que l'angle entre le mouvement et le téléporteur soit suffisamment petit
            if (angleBetween <= detectionAngle)
            {
                // Téléporter le GameObject à la destination
                other.transform.position = new Vector3(destination.x, destination.y, other.transform.position.z);
                SwapZone();
            }
        }
    }

    // Dessiner des gizmos dans la fenêtre de la scène pour visualiser la destination
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(new Vector2(destination.x, destination.y), 0.5f);
    }
    
    public void SwapZone()
    {
        // Récupère le FadeController et lance le fondu au noir
        FadeController fadeController = FindObjectOfType<FadeController>(); // Assure-toi que ce script est attaché à un objet approprié dans la scène
        if (fadeController != null)
        {
            StartCoroutine(FadeTransition(fadeController));
        }
        else
        {
            Debug.LogError("ZoneSwapper: Aucun FadeController trouvé dans la scène.");
        }
    }
    
    private IEnumerator FadeTransition(FadeController fadeController)
    {
        fadeController.FadeToBlack();

        // Attendre une durée plus courte avant de lancer le fondu retour
        yield return new WaitForSeconds(fadeController.fadeDuration * 0.0f); // Par exemple, attendre la moitié de la durée de fondu

        // ICI, vous pouvez ajouter le code pour effectuer les changements de zone
        // Par exemple: changer la position du joueur, charger une nouvelle scène, etc.

        // Lance le fondu retour à la normale
        fadeController.FadeFromBlack();
    }
}