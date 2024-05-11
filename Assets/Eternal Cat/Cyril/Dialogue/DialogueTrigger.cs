using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [Header("Visual Cue")] [SerializeField]
    private GameObject visualCue;

    [Header(("Ink JSON"))] [SerializeField]
    private TextAsset inkJSON;

    private bool playerInRange;
    private void Awake()
    {
        visualCue.SetActive(false);
        visualCue.SetActive(false);
    }
    
    private GameObject GetClosestParentWithTag(string tag)
    {
        Transform currentParent = transform.parent; // Commencez par le parent immédiat de cet objet.

        // Boucle pour remonter la hiérarchie des parents
        while (currentParent != null)
        {
            if (currentParent.CompareTag(tag)) // Vérifiez si le parent a le tag désiré.
            {
                return currentParent.gameObject; // Retournez le GameObject si le tag correspond.
            }
            currentParent = currentParent.parent; // Passez au parent suivant.
        }

        return null; // Retournez null si aucun parent avec le tag spécifié n'est trouvé.
    }


    
    private void Update()
    {
        if (playerInRange && !DialogueManager.getInstance().dialogueIsPlaying)
        {
            visualCue.SetActive(true);
            if (InputManager.instance.IsInteractPressed())
            {
                Debug.Log(inkJSON.text);
                DialogueManager.getInstance().EnterDialogueMode(inkJSON);
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
            playerInRange = true;
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerInRange = false;
        }
    }
}
