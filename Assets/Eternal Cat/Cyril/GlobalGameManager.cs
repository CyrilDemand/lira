using System;
using UnityEngine;

public class GlobalGameManager : MonoBehaviour
{
    public static GlobalGameManager instance;
    public Vector2 lastPlayerPosition; // Exemples de propriétés

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Assurez-vous que le gestionnaire persiste
        }
        else
        {
            Destroy(gameObject);
        }
    }
}