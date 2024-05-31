using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GlossaireManager : MonoBehaviour
{
    [SerializeField]
    public GameObject panelGlossaire;
    
    [SerializeField]
    public Glossaire glossaire;

    public int currentEntry = 0;
    
    public static GlossaireManager instance;
    
    public bool glossaireIsOpen = false;

    public void Awake()
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

    private void Start()
    {
        panelGlossaire.SetActive(false); // Assurez-vous que le glossaire est caché au départ
    }

    private void Update()
    {
        if (glossaireIsOpen)
        {
            DisplayCurrentEntry();
        }
    }
    
    private void OpenGlossaire()
    {
        panelGlossaire.SetActive(true);
        DisplayCurrentEntry();
    }
    
    private void CloseGlossaire()
    {
        panelGlossaire.SetActive(false);
    }
    
    public void ToggleGlossaire()
    {
        if (glossaireIsOpen)
        {
            CloseGlossaire();
        }
        else
        {
            OpenGlossaire();
        }
        glossaireIsOpen = !glossaireIsOpen;
    }
    
    public void NextEntry()
    {
        currentEntry++;
        if (currentEntry >= glossaire.entries.Count)
        {
            currentEntry = 0;
        }
    }
    
    public void PreviousEntry()
    {
        currentEntry--;
        if (currentEntry < 0)
        {
            currentEntry = glossaire.entries.Count - 1;
        }
    }
    
    public void DisplayCurrentEntry()
    {
        // prendre le gameObject se nommant "PanelGlossaire" et changer les sous gameObject "Description", "Image" et "Titre" pour afficher les informations de l'entrée courante
        // Assurez-vous que les sous gameObject "Description", "Image" et "Titre" sont bien assignés dans l'inspecteur
        

        GameObject descriptionObject = GameObject.Find("DescriptionGlossaire");
        GameObject image = GameObject.Find("Image");
        GameObject titre = GameObject.Find("Titre");
    
      
        descriptionObject.GetComponent<TextMeshProUGUI>().text = glossaire.entries[currentEntry].definition;
        image.GetComponent<Image>().sprite = glossaire.entries[currentEntry].image;
        Debug.Log(glossaire.entries[currentEntry].terme);
        titre.GetComponent<TextMeshProUGUI>().text = glossaire.entries[currentEntry].terme;
        
        Debug.Log(glossaire.entries[currentEntry].terme);
        Debug.Log(glossaire.entries[currentEntry].definition);
        Debug.Log(glossaire.entries[currentEntry].image);
    }
}