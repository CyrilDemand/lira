using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance { get; private set; } // Instance singleton
    public TextMeshProUGUI timeText; // Référence à l'UI Text pour afficher l'heure
    public float timeScale = 2; // Combien de secondes réelles représentent une heure de jeu

    private int time; // Le temps en minutes depuis minuit
    private float timeAccumulator = 0; // Accumulateur pour gérer l'écoulement du temps

    public int GetTime()
    {
        return time;
    }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Optionnel: Garde l'instance persistante entre les scènes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        SetTime(8, 0); // Définir l'heure initiale à 8h00
    }

    void Update()
    {
        UpdateTime();
        DisplayTime();
    }

    void UpdateTime()
    {
        timeAccumulator += Time.deltaTime;
        int minutesToAdd = (int)(timeAccumulator / (timeScale / 60)); // Calcule combien de minutes ajouter
        if (minutesToAdd > 0)
        {
            time += minutesToAdd;
            timeAccumulator -= minutesToAdd * (timeScale / 60); // Soustraire le temps utilisé de l'accumulateur
            time %= 1440; // Remettre à zéro à minuit (1440 minutes dans une journée)
            CheckScheduledActions();
        }
    }

    void CheckScheduledActions()
    {
        if (time % 15 == 0)
        {
            Debug.Log("Action toutes les 15 minutes");
            foreach (GameObject pnj in GlobalGameManager.instance.allPNJ)
            {
                PnjSchedulerManager scheduler = pnj.GetComponent<PnjSchedulerManager>();
                if (scheduler != null)
                {
                    scheduler.FollowSchedule(time);
                }
                else
                {
                    Debug.LogError("PnjSchedulerManager component not found on " + pnj.name);
                }
            }
        }
    }

    void DisplayTime()
    {
        int hours = time / 60;
        int minutes = time % 60;
        timeText.text = $"{hours:00}:{minutes:00}";
    }

    public void SetTimeAtMorning()
    {
        SetTime(6, 30);
    }

    public void SetTime(int hours, int minutes)
    {
        time = hours * 60 + minutes;
        timeAccumulator = 0; // Réinitialise l'accumulateur lors du changement d'heure
    }
}
