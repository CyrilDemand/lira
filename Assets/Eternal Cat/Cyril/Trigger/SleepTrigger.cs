using System.Collections;
using UnityEngine;
using TMPro; // Assurez-vous d'importer le namespace TextMeshPro

public class SleepTrigger : MonoBehaviour
{
    private bool playerInRange;
    [Header("Visual Cue")]
    [SerializeField] private GameObject visualCue;

    private void Update()
    {
        if (playerInRange && !DialogueManager.getInstance().dialogueIsPlaying)
        {
            visualCue.SetActive(true);
            if (InputManager.instance.IsInteractPressed())
            {
                Sleep();
            }
        }
        else
        {
            visualCue.SetActive(false);
        }
    }

    public void Sleep()
    {
        // Récupère le FadeController et lance le fondu au noir
        FadeController fadeController = FindObjectOfType<FadeController>(); // Assure-toi que ce script est attaché à un objet approprié dans la scène
        if (fadeController != null)
        {
            StartCoroutine(SleepRoutine(fadeController));
        }
        else
        {
            Debug.LogError("SleepTrigger: Aucun FadeController trouvé dans la scène.");
        }
    }

    private IEnumerator SleepRoutine(FadeController fadeController)
    {
        GameObject player = GameObject.Find("Player");
        if (player != null)
        {
            // Arrête le mouvement du joueur
            player.GetComponent<Movement>().changeIsStopped();

            // Lance le fondu au noir
            fadeController.FadeToBlack();
            yield return new WaitForSeconds(fadeController.fadeDuration); // Attendre la durée du fondu

            // Réinitialise le temps au matin
            TimeManager.Instance.SetTimeAtMorning();

            // Lance le slide noir de gauche à droite
            yield return StartCoroutine(fadeController.SlideBlackLeftToRightCoroutine());

            // Attendre la fin du slide
            yield return new WaitForSeconds(fadeController.slideDuration);

            // Lance le fondu retour à la normale
            fadeController.FadeFromBlack();
            yield return new WaitForSeconds(fadeController.fadeDuration); // Attendre la durée du fondu

            // Redonne la possibilité au joueur de bouger
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
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerInRange = true;
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerInRange = false;
        }
    }
}
