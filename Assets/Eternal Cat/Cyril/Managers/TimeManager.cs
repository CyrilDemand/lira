using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance { get; private set; } // Instance singleton
    public TextMeshProUGUI timeText; // Référence à l'UI Text pour afficher l'heure
    public float timeScale = 60; // Combien de minutes passent en une seconde réelle

    private float time; // Le temps en minutes depuis minuit

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
        time += Time.deltaTime * timeScale;
        time %= 1440; // Remettre à zéro à minuit (1440 minutes dans une journée)
    }

    void DisplayTime()
    {
        int hours = (int)time / 60;
        int minutes = (int)time % 60;
        timeText.text = hours.ToString("00") + ":" + minutes.ToString("00");
    }

    public void SetTimeAtMorning()
    {
        SetTime(6,30);
    }

    public void SetTime(int hours, int minutes)
    {
        time = hours * 60 + minutes;
    }
}