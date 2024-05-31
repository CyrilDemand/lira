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

    public string dialogueKey;
    private void Awake()
    {
        visualCue.SetActive(false);
        visualCue.SetActive(false);
        dialogueKey = "";
    }
    
    private void Update()
    {
        if (playerInRange && !DialogueManager.getInstance().dialogueIsPlaying)
        {
            visualCue.SetActive(true);
            if (InputManager.instance.IsInteractPressed())
            {
                Debug.Log(inkJSON.text);
                DialogueManager.getInstance().pnjDeplacement = GetComponentInParent<PathFinding>();
                DialogueManager.getInstance().EnterDialogueMode(inkJSON, dialogueKey);
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
