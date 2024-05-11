using UnityEngine;
using System.Collections;
    
public class InputManager : MonoBehaviour
{
    public static InputManager instance;

    private float horizontalInput;
    private float verticalInput;
    private bool interactPressed;
    private bool submitPressed;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        interactPressed = Input.GetKeyDown(KeyCode.E);
        submitPressed = Input.GetKeyDown(KeyCode.Space);
        if (Input.GetKeyDown(KeyCode.R))
        {
            Sleep();
        }
    }

    public Vector2 GetMovementInput()
    {
        return new Vector2(horizontalInput, verticalInput).normalized;
    }

    public bool IsInteractPressed()
    {
        return interactPressed;
    }

    public bool IsSubmitPressed()
    {
        return submitPressed;
    }

    public void Sleep()
    {
        // Récupère le FadeController et lance le fondu au noir
        FadeController fadeController = FindObjectOfType<FadeController>(); // Assure-toi que ce script est attaché à un objet approprié dans la scène
        fadeController.FadeToBlack();

        // Attendre que le fondu au noir soit complet
        StartCoroutine(WaitAndWakeUp(fadeController));
    }

    private IEnumerator WaitAndWakeUp(FadeController fadeController)
    {
        yield return new WaitForSeconds(fadeController.fadeDuration); // Attendre la durée du fondu
        TimeManager.Instance.SetTimeAtMorning(); // Réinitialise le temps au matin
        fadeController.FadeFromBlack(); // Lance le fondu retour à la normale
    }
}