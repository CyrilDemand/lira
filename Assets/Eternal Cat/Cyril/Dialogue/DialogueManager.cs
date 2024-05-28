using System;
using System.Collections;
using System.Collections.Generic;
using Ink.Runtime;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{

    [Header("Dialogue UI")] [SerializeField]
    private GameObject dialoguePanel;

    [SerializeField] private TextMeshProUGUI dialogueText;

    private Story currentStory;

    public bool dialogueIsPlaying { get; private set; }
    
    private static DialogueManager instance;

    [Header("Choices UI")] [SerializeField]
    private GameObject[] choices;

    private TextMeshProUGUI[] choicesText;

    private PathFinding pnjDeplacement;

    private float pnjMoveSpeedBeforeStop;
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Found more than one");
        }

        instance = this;
    }

    public static DialogueManager getInstance()
    {
        return instance;
    }

    private void Start()
    {
        this.dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);

        choicesText = new TextMeshProUGUI[choices.Length];
        int index = 0;
        foreach (GameObject choice in choices)
        {
            choicesText[index] = choice.GetComponentInChildren<TextMeshProUGUI>();
            index++;
        }
    }

    private void Update()
    {
        if (!dialogueIsPlaying)
        {
            return;
        }

        if (InputManager.instance.IsSubmitPressed())
        {
            ContinueStory();
        }
    }

    private void resumePNJPathFinding()
    {
        pnjDeplacement.moveSpeed = pnjMoveSpeedBeforeStop;
    }

    private void StopPlayerAnimation()
    {
        GameObject player = GameObject.FindGameObjectsWithTag("Player")[0];
        player.GetComponent<Movement>().HandleAnimation(Vector2.zero);
    }
    
    // Méthode pour arrêter le PNJ le plus proche qui est en interaction avec le joueur
    private void StopPNJ()
    {
        Debug.Log("stoppnj");
        // Trouver tous les PNJ avec le tag "PNJ"
        GameObject[] pnjs = GameObject.FindGameObjectsWithTag("PNJ");
        float closestDistance = float.MaxValue;
        GameObject closestPNJ = null;

        // Trouver le PNJ le plus proche du joueur
        foreach (GameObject pnj in pnjs)
        {
            float distance = Vector3.Distance(transform.position, pnj.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestPNJ = pnj;
            }
        }

        if (closestPNJ != null)
        {
            // Vérifier si le sous-objet avec le tag "InteractPNJTrigger" est actuellement déclenché par le joueur
            Transform interactTrigger = closestPNJ.transform.Find("Trigger");
            if (interactTrigger != null && interactTrigger.GetComponent<Collider2D>().IsTouchingLayers(LayerMask.GetMask("Player")))
            {
                // Accéder au script PathFinding du PNJ et mettre sa moveSpeed à 0
                PathFinding pathFindingScript = closestPNJ.GetComponent<PathFinding>();
                pnjDeplacement = pathFindingScript;
                if (pathFindingScript != null)
                {
                    pnjMoveSpeedBeforeStop = pathFindingScript.moveSpeed;
                    pathFindingScript.moveSpeed = 0;
                }
            }
        }
    }

    public void EnterDialogueMode(TextAsset inkJson, string dialogueKey)
    {
        StopPNJ();
        StopPlayerAnimation();
        currentStory = new Story(inkJson.text);
        currentStory.variablesState["currentDialogue"] = dialogueKey; // Mise à jour de la variable dans Ink
        dialogueIsPlaying = true;
        dialoguePanel.SetActive(true);
        
        ContinueStory();
    }


    private void ExitDialogue()
    {
        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
        dialogueText.text = "";
    }

    private void ContinueStory()
    {
        if (currentStory.canContinue)
        {
            string text = currentStory.Continue();
            dialogueText.text = text;
            Debug.Log("text : " + text);
            if (currentStory.currentChoices.Count > 0)
            {
                DisplayChoices();
            }
            else
            {
                removeChoices();
            }
            
        }
        else
        {
            Debug.Log("Fin du dialogue.");
            resumePNJPathFinding();
            ExitDialogue();
        }
    }

    private void DisplayChoices()
    {
        List<Choice> currentChoices = currentStory.currentChoices;
        
        EventSystem.current.SetSelectedGameObject(choices[0].gameObject);
        if (currentChoices.Count > choices.Length)
        {
            Debug.LogError("More choices were given than the UI can support");
        }

        int index = 0;
        foreach (Choice choice in currentChoices)
        {
            choices[index].gameObject.SetActive(true);
            choicesText[index].text = choice.text;
            index++;
        }

        for (int i = 0; i < choices.Length; i++)
        {
            choices[i].gameObject.SetActive(true);
        }
    }

    private void removeChoices()
    {
        foreach (GameObject choice in choices)
        {
            choice.SetActive(false);
        }
    }

    public void MakeChoice(int choiceIndex)
    {
        currentStory.ChooseChoiceIndex(choiceIndex);
    }
}
