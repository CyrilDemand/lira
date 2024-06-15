using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [Header("Visual Cue")]
    [SerializeField] private GameObject visualCue;

    [Header("Ink JSON")]
    [SerializeField] private TextAsset inkJSON;

    private bool playerInRange;

    public string dialogueKey;

    private void Awake()
    {
        if (visualCue != null)
        {
            visualCue.SetActive(false);
        }
        dialogueKey = "";
    }

    private void Update()
    {
        if (TriggerManager.Instance.playerInRange && !DialogueManager.getInstance().dialogueIsPlaying)
        {
            if (visualCue != null)
            {
                visualCue.SetActive(true);
            }
            
            if (InputManager.instance.IsInteractPressed())
            {
                DialogueManager.getInstance().EnterDialogueMode(inkJSON, dialogueKey);
            }
        }
        else
        {
            if (visualCue != null)
            {
                visualCue.SetActive(false);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            TriggerManager.Instance.pnjToStop = transform.parent.GetComponent<PathFinding>();
            TriggerManager.Instance.AddPlayerInRange();
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if(TriggerManager.Instance.pnjToStop == transform.parent.GetComponent<PathFinding>())
            {
                TriggerManager.Instance.pnjToStop = null;
            }
            TriggerManager.Instance.RemovePlayerInRange();
            playerInRange = false;
        }
    }
}
