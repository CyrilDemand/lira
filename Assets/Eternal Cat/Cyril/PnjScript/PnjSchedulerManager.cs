using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PnjSchedulerManager : MonoBehaviour
{
    // Start is called before the first frame update
    public TextAsset scheduleJSON; // Assigner dans l'inspecteur Unity
    private Schedule pnjSchedule;

    void Start()
    {
        LoadSchedule();
        FollowSchedule(TimeManager.Instance.GetTime());
    }
    

    // Charger l'horaire à partir d'un fichier JSON
    private void LoadSchedule()
    {
        if (scheduleJSON != null)
        {
            Debug.Log($"{scheduleJSON.text}");
            pnjSchedule = JsonUtility.FromJson<Schedule>(scheduleJSON.text);
            Debug.Log($"{pnjSchedule}");
        }
        else
        {
            Debug.LogError("Schedule JSON file is not assigned or missing.");
        }
    }

    public void FollowSchedule(int time)
    {
        // Convertir le temps en minutes en heures et minutes
        int totalMinutes = Mathf.FloorToInt(time) % 1440; // s'assure que le temps ne dépasse pas 24 heures
        int hours = totalMinutes / 60;
        int minutes = totalMinutes % 60;

        // Formater l'heure en format hh:mm
        string currentTime = $"{hours:00}:{minutes:00}";

        // Itérer à travers les créneaux horaires pour trouver et exécuter le créneau actuel
        foreach (var slot in pnjSchedule.dailySchedule)
        {
            if (slot.time == currentTime)
            {
                MoveToLocation(slot.location);
                PerformActivity(slot.activity);
                ChangeDialogue(slot.dialogueKey);
                break;
            }
        }
    }

    private void MoveToLocation(string location)
    {
        // Trouver l'objet dans la scène qui correspond à la location
        GameObject targetLocation = GameObject.Find(location);
        if (targetLocation == null)
        {
            Debug.LogError("No target location found with the name: " + location);
            return;
        }

        // Assurer que le PNJ a un composant de pathfinding
        PathFinding pathFinding = GetComponent<PathFinding>();
        if (pathFinding == null)
        {
            Debug.LogError("PathFinding component not found on " + gameObject.name);
            return;
        }

        // Convertir la position de l'objet cible en Vector2 (si nécessaire)
        Vector2 destination = new Vector2(targetLocation.transform.position.x, targetLocation.transform.position.y);

        // Appeler la fonction SetDestination du script PathFinding
        pathFinding.SetDestination(destination);
        Debug.Log("Moving " + gameObject.name + " to: " + location);
    }


    private void PerformActivity(string activity)
    {
        // Implémenter l'action de l'activité
        Debug.Log("PNJ is performing: " + activity);
    }

    private void ChangeDialogue(string dialogueKey)
    {
        // Trouver le DialogueTrigger dans le PNJ
        DialogueTrigger dialogueTrigger = GetComponentInChildren<DialogueTrigger>();
        if (dialogueTrigger != null)
        {
            dialogueTrigger.dialogueKey = dialogueKey;
        }
    }

}
