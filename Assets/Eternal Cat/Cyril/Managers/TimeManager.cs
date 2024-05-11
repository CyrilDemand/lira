using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class TimeManager : MonoBehaviour
{
    public TextMeshProUGUI timeText; // Référence à l'UI Text pour afficher l'heure
    public float timeScale = 60; // Combien de minutes passent en une seconde réelle

    private float time; // Le temps en minutes depuis minuit

    void Start()
    {
        time = 480; // Commencer à 8h00 (480 minutes après minuit)
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
}