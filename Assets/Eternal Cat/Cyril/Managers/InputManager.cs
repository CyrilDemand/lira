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

    private void ReadGlossaryInputs()
    {
         if (Input.GetMouseButtonDown(0)) // Bouton gauche de la souris
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (mousePosition.x < 0) // Côté gauche de l'écran
            {
                GlossaireManager.instance.PreviousEntry();
            }
        }
        if (Input.GetMouseButtonDown(0)) // Bouton droit de la souris
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (mousePosition.x > 0) // Côté droit de l'écran
            {
                GlossaireManager.instance.NextEntry();
            }
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Debug.Log("left arrow pressed");
            GlossaireManager.instance.PreviousEntry();
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Debug.Log("right arrow pressed");
            GlossaireManager.instance.NextEntry();
        }
    }

    private bool isInMenu(){
        if (Input.GetKeyDown(KeyCode.G))
        {
            GlossaireManager.instance.ToggleGlossaire();
            return true;
        }
        if (GlossaireManager.instance.glossaireIsOpen)
        {
            ReadGlossaryInputs();
            return true;
        }

        // faire même chose pour inventaire
        if (Input.GetKeyDown(KeyCode.I))
        {
            InventoryManager.instance.ToggleInventory();
        }
        if (InventoryManager.instance.inventaireIsOpen)
        {
            return true;
        }

        return false;
    }

    void Update()
    {
        if (isInMenu())
        {
            return;
        }else{
            horizontalInput = Input.GetAxisRaw("Horizontal");
            verticalInput = Input.GetAxisRaw("Vertical");
            interactPressed = Input.GetKeyDown(KeyCode.E);
            submitPressed = Input.GetKeyDown(KeyCode.Space);
            if (submitPressed)
            {
                Debug.Log("submit pressed");
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                Sleep();
            }
        }
        
        
       
        // je veux utiliser les flèches gauche et droite pour changer la page du glossaire
       
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