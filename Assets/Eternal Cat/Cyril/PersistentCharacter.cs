using UnityEngine;

public class PersistentCharacter : MonoBehaviour
{
    private static PersistentCharacter instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Le personnage ne sera pas détruit lors du changement de scène
        }
        else
        {
            Destroy(gameObject); // Détruire les doublons si un autre personnage existe
        }
    }
}