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

    public PathFinding pnjDeplacement;

    public float pnjMoveSpeedBeforeStop;

    // hashmap des personnages, avec comme clé le nom du personnage et comme valeur le sprite du personnage
    [Header("Characters")] [SerializeField]
    private List<CharacterSprite> characters = new List<CharacterSprite>();

    [SerializeField]
    private GameObject characterSpriteGameObject;

    [SerializeField]
    private GameObject characterNameGameObject;

    private string characterName;

    private string characterEmotion;

    [Serializable]
    public class CharacterSprite
    {
        public string name;
        public Sprite sprite;
        public CharacterEmotion emotion;
    }

    public enum CharacterEmotion
    {
        inquiet,
        neutre,
        amoureux,
        colere,
        interrogation,
        joyeux,
        surpris,
        triste
    }

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
        Debug.Log(pnjDeplacement);
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
        if ( pnjDeplacement != null)
        {
            pnjMoveSpeedBeforeStop = pnjDeplacement.moveSpeed;
            pnjDeplacement.moveSpeed = 0;
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
            // récupérer le nom du personnage et l'emotion et supprimer les espaces avant et après
            characterName = text.Split(':')[0].Trim().ToLower();
            characterEmotion = text.Split(':')[1].Trim().ToLower();
            dialogueText.text = text.Split(':')[2];

            // changer le sprite du personnage avec le nom du personnage
            DisplayCharacterName();
            ChooseCharacterSpriteByEmotion();

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

    private void ChooseCharacterSpriteByEmotion()
    {
        foreach (CharacterSprite characterSprite in characters)
        {
            if (characterSprite.emotion.ToString().ToLower() == characterEmotion && characterSprite.name.ToLower() == characterName)
            {
                Debug.Log("characterSprite.name : " + characterSprite.name + " characterName : " + characterName + " characterEmotion : " + characterEmotion);
                characterSpriteGameObject.GetComponent<Image>().sprite = characterSprite.sprite;
                characterSpriteGameObject.SetActive(true);
                break;
            }
        }
    }
    
    private void DisplayCharacterName()
    {
        characterNameGameObject.GetComponent<TextMeshProUGUI>().text = characterName;
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
