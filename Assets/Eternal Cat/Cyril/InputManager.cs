using UnityEngine;

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
        // Capturez les entrées de mouvement
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        // Capturez la touche d'interaction "E"
        interactPressed = Input.GetKeyDown(KeyCode.E);

        submitPressed = Input.GetKeyDown(KeyCode.Space);
    }

    public Vector2 GetMovementInput()
    {
        // Retournez un vecteur représentant l'entrée de mouvement
        return new Vector2(horizontalInput, verticalInput).normalized;
    }

    public bool isSubmitPressed()
    {
        return submitPressed;
    }

    public bool IsInteractPressed()
    {
        // Retournez vrai si "E" a été pressée
        return interactPressed;
    }
}