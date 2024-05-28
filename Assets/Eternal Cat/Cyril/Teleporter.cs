using UnityEngine;
using System.Collections;
using TMPro; // Assurez-vous d'avoir importé le namespace TextMeshPro

public class Teleporter : MonoBehaviour
{
    // Position cible pour la téléportation
    [SerializeField] public Vector2 destination;

    // Angle en degrés qui définit un cône de téléportation
    [SerializeField] private float detectionAngle = 45f;

    [SerializeField] public GameObject roomDestination;
    
    [SerializeField] public GameObject roomWhereIsTheTP;

    [SerializeField] private string destinationRoomName; // Ajoutez ceci pour le nom de la pièce de destination
    
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

        // Trouve et déplace progressivement le GameObject "Room UI"
        GameObject roomUI = GameObject.Find("Room UI");
        if (roomUI != null)
        {
            float targetDeltaY = -120f; // Déplacement de 200 unités sur l'axe Y (ou ajustez cette valeur si nécessaire)
            StartCoroutine(MoveRoomUI(roomUI, targetDeltaY, 1.0f)); // Déplacer sur 1 seconde

            // Trouve l'enfant "RoomText" et met à jour son texte
            Transform roomTextTransform = roomUI.transform.Find("RoomText");
            if (roomTextTransform != null)
            {
                TextMeshProUGUI roomText = roomTextTransform.GetComponent<TextMeshProUGUI>();
                if (roomText != null)
                {
                    roomText.text = destinationRoomName;
                }
                else
                {
                    Debug.LogError("TextMeshProUGUI component not found on RoomText.");
                }
            }
            else
            {
                Debug.LogError("RoomText not found under Room UI.");
            }
        }
        else
        {
            Debug.LogError("Room UI not found!");
        }
    }
    
    private IEnumerator FadeTransition(FadeController fadeController)
    {
        GameObject player = GameObject.Find("Player");
        if (player != null)
        {
            fadeController.InstantBlack(); // Instantanément noir
            yield return null; // attendre un frame pour que la transition soit visible
            player.GetComponent<Movement>().changeIsStopped();
            yield return StartCoroutine(fadeController.SlideBlackLeftToRight(player)); // Faire le slide noir de gauche à droite

            // Attendre la durée du slide
            yield return new WaitForSeconds(fadeController.slideDuration);

            // ICI, vous pouvez ajouter le code pour effectuer les changements de zone
            // Par exemple: changer la position du joueur, charger une nouvelle scène, etc.

            // Lance le fondu retour à la normale
            fadeController.FadeFromBlack();
            player.GetComponent<Movement>().changeIsStopped();
        }
    }


    private IEnumerator MoveRoomUI(GameObject roomUI, float deltaY, float duration)
    {
        Vector3 startPosition = roomUI.transform.localPosition;
        Vector3 endPosition = new Vector3(startPosition.x, startPosition.y + deltaY, startPosition.z);
        float elapsedTime = 0f;

        // Descente
        while (elapsedTime < duration)
        {
            roomUI.transform.localPosition = Vector3.Lerp(startPosition, endPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // S'assurer que la position finale est bien appliquée
        roomUI.transform.localPosition = endPosition;

        // Attendre une seconde
        yield return new WaitForSeconds(1.0f);

        // Remonter
        elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            roomUI.transform.localPosition = Vector3.Lerp(endPosition, startPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // S'assurer que la position finale est bien appliquée
        roomUI.transform.localPosition = startPosition;
    }
}
