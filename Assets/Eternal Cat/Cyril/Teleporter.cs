using UnityEngine;

public class Teleporter : MonoBehaviour
{
    // Position cible pour la téléportation
    [SerializeField] private Vector2 destination;

    // Angle en degrés qui définit un cône de téléportation
    [SerializeField] private float detectionAngle = 45f;

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
            }
        }
    }

    // Dessiner des gizmos dans la fenêtre de la scène pour visualiser la destination
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(new Vector2(destination.x, destination.y), 0.5f);
    }
}