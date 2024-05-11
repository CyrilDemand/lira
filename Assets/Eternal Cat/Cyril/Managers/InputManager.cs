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
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        interactPressed = Input.GetKeyDown(KeyCode.E);
        submitPressed = Input.GetKeyDown(KeyCode.Space);
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
}