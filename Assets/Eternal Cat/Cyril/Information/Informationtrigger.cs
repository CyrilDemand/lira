using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Ajoutez cette ligne pour résoudre l'erreur de namespace Image


public class InformationTrigger : MonoBehaviour
{
    [Header("Visual Cue")] [SerializeField]
    private GameObject visualCue;

    [Header(("UI"))] [SerializeField]
    private Sprite spriteRenderer;

    [Header(("Information Panel"))] [SerializeField]
    private GameObject informationPanel;

    private bool playerInRange;

    private void Awake()
    {
        visualCue.SetActive(false);
        //information pannel mis en transparent
        informationPanel.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0);
    }

    private void Start()
    {
        // Initialisation de playerInRange à false
        playerInRange = false;
    }
    
    private void Update()
    {
        if (playerInRange)
        {
            visualCue.SetActive(true);
            if (InputManager.instance.IsInteractPressed())
            {
                // changer l'image du sprite renderer de Information Panel par le mien
                Debug.Log("Change image");

                // si l'image est transparente, changer l'image
                informationPanel.GetComponent<SpriteRenderer>().sprite = spriteRenderer;

                if(informationPanel.GetComponent<SpriteRenderer>().color.a == 0)
                {
                    informationPanel.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);

                    // faire un blur du de la caméra
                }else
                {
                    informationPanel.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
                }


            }
            
        }
        else
        {
            visualCue.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("Player in range");
            playerInRange = true;
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("Player out of range");
            playerInRange = false;
        }
    }
}
