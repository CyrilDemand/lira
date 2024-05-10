using System;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class GlobalGameManager : MonoBehaviour
{
    public static GlobalGameManager instance;
    [SerializeField]
    public GameObject player; // Exemples de propriétés
    private Dictionary<String, Vector2> pnjLocations;
    private GameObject[] allRooms;
    private Teleporter[] allTP;
    private void Awake()
    {
        if (instance == null)
        {
            Debug.Log("ICI");
            instance = this;
            DontDestroyOnLoad(gameObject); // Assurez-vous que le gestionnaire persiste
            InitializeValue();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeValue()
    {
        pnjLocations = new Dictionary<String, Vector2>();
        allRooms = GameObject.FindGameObjectsWithTag("Room");
        InitializeTP();
    }

    public void InitializeTP()
    {
        Debug.Log("InitializeTP");
        // Trouver tous les GameObjects avec le tag "Teleporter"
        GameObject[] allTPObjects = GameObject.FindGameObjectsWithTag("Teleporter");

        // Initialiser la liste des téléporteurs
        List<Teleporter> tps = new List<Teleporter>();

        // Parcourir chaque GameObject trouvé pour extraire le composant Teleporter
        foreach (GameObject tpObject in allTPObjects)
        {
            Teleporter tp = tpObject.GetComponent<Teleporter>();

            // Assurez-vous que le GameObject contient bien un composant Teleporter
            if (tp != null)
            {
                tps.Add(tp);
            }
            else
            {
                Debug.LogWarning($"Le GameObject {tpObject.name} n'a pas de composant Teleporter.");
            }
        }

        // Vérification : Affiche tous les téléporteurs trouvés
        foreach (Teleporter teleporter in tps)
        {
            Debug.Log($"Téléporteur trouvé : destination = {teleporter.destination}, roomDestination = {teleporter.roomDestination}");
        }
        Debug.Log("Avant le tps.toarray");
        allTP = tps.ToArray();
    }

    public GameObject[] getAllRooms()
    {
        return allRooms;
    }
    public Teleporter[] getAllTeleporteur()
    {
        string allTPInfo = "AllTP = [";
        for (int i = 0; i < allTP.Length; i++)
        {
            Teleporter tp = allTP[i];
            allTPInfo += $"{{destination: {tp.destination}, roomDestination: {tp.roomDestination.name}, roomDuTp: {tp.roomWhereIsTheTP}}}";

            // Ajouter une virgule entre les éléments sauf pour le dernier
            if (i < allTP.Length - 1)
            {
                allTPInfo += ", ";
            }
        }
        allTPInfo += "]";

        // Affiche le message dans la console
        Debug.Log(allTPInfo);
        return allTP;
    }
    public void AddPnjLocation(String name, Vector2 position)
    {
        pnjLocations.Add(name,position);
    }
    
    public void SetPnjLocation(String name, Vector2 position)
    {
        if (pnjLocations.ContainsKey(name))
        {
            pnjLocations[name] = position;
        }
        else
        {
            AddPnjLocation(name, position);
        }
    }
    
    public ref Dictionary<String, Vector2> GetPnjLocations()
    {
        return ref pnjLocations;
    }
}
