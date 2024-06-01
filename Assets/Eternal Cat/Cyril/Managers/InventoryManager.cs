using System.Collections.Generic;
using System.Linq; // Ajoutez cette ligne
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

[System.Serializable]
public class InventoryData
{
    public List<PlanteItem> plantes;
    public List<Outil> outils;
    public List<Ingredient> ingredients;
    public List<Potion> potions;
    public List<Cles> trousseauDeCles;
    public List<QueteItem> quetes;
}

public class InventoryManager : MonoBehaviour
{
    public Inventory inventory;

    public static InventoryManager instance;

    public bool inventaireIsOpen = false;

    [SerializeField]
    public GameObject panelInventaire;

    public string typeObjectShow = "Potion";

    private void Awake()
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

    public void ToggleInventory()
    {
        inventaireIsOpen = !inventaireIsOpen;
        if (inventaireIsOpen)
        {
            OpenInventory();
        }
        else
        {
            CloseInventory();
        }
    }

    public void OpenInventory()
    {
        reArrangeInventory();
        inventaireIsOpen = true;
        panelInventaire.SetActive(true);
        DisplayObject();
    }

    public void CloseInventory()
    {
        inventaireIsOpen = false;
        panelInventaire.SetActive(false);
    }

    private void DisplayObject()
    {
        Transform itemsContainer = panelInventaire.transform.GetChild(2);
        List<Item> itemsToDisplay = GetItemsToDisplay();

        // Afficher les items dans les slots disponibles
        int i = 0;
        foreach (Item item in itemsToDisplay)
        {
            if (i < itemsContainer.childCount)
            {
                Transform itemSlot = itemsContainer.GetChild(i);
                GameObject imageComponent = itemSlot.Find("Image").gameObject;
                imageComponent.GetComponent<Image>().sprite = item.itemIcon;
                imageComponent.GetComponent<Image>().color = new Color(1, 1, 1);
                imageComponent.SetActive(true);
                GameObject numberCom = itemSlot.Find("Number").gameObject;
                numberCom.GetComponent<TextMeshProUGUI>().text = item.currentStack.ToString();
                i++;
            }
        }

        // Désactiver les slots excédentaires
        for (int j = i; j < itemsContainer.childCount; j++)
        {
            Transform itemSlot = itemsContainer.GetChild(j);
            GameObject numberCom = itemSlot.Find("Number").gameObject;
            numberCom.GetComponent<TextMeshProUGUI>().text = "";
            GameObject imageComponent = itemSlot.Find("Image").gameObject;
            imageComponent.GetComponent<Image>().sprite = null;
            imageComponent.GetComponent<Image>().color = new Color(110 / 255f, 78 / 255f, 144 / 255f, 0.5f);
            imageComponent.SetActive(true);
        }
    }

    private List<Item> GetItemsToDisplay()
    {
        switch (typeObjectShow)
        {
            case "Plantes":
                return inventory.plantes.Cast<Item>().ToList();
            case "Outils":
                return inventory.outils.Cast<Item>().ToList();
            case "Cuisines":
                return inventory.ingredients.Cast<Item>().ToList();
            case "Potion":
                return inventory.potions.Cast<Item>().ToList();
            case "TrousseauDeCles":
                return inventory.cles.Cast<Item>().ToList();
            case "Quetes":
                return inventory.quetes.Cast<Item>().ToList();
            default:
                return new List<Item>();
        }
    }

    public void ChangeTypeObject(string type)
    {
        typeObjectShow = type;
        DisplayObject();
    }

    private void reArrangeInventory()
    {
        // Regarde chaque liste, voit s'il y a des items identiques et les regroupe
        Debug.Log("liste avant : " + inventory.plantes.Count);
        List<List<Item>> allLists = new List<List<Item>> 
        { 
            inventory.plantes.Cast<Item>().ToList(), 
            inventory.outils.Cast<Item>().ToList(), 
            inventory.ingredients.Cast<Item>().ToList(), 
            inventory.potions.Cast<Item>().ToList(), 
            inventory.cles.Cast<Item>().ToList(), 
            inventory.quetes.Cast<Item>().ToList() 
        };
        foreach (var list in allLists)
        {
            var groupedItems = list.GroupBy(item => item.itemName).ToList();
            Debug.Log("list : " + list.Count);
            Debug.Log("groupedItems : " + groupedItems.Count);
            list.Clear();
            foreach (var group in groupedItems)
            {
                Item item = group.First();
                item.currentStack = group.Sum(i => i.currentStack);
                if (item.currentStack > item.maxStack)
                {
                    int fullStacks = item.currentStack / item.maxStack;
                    int remainder = item.currentStack % item.maxStack;
                    for (int i = 0; i < fullStacks; i++)
                    {
                        Item newItem = Instantiate(item);
                        newItem.currentStack = item.maxStack;
                        list.Add(newItem);
                    }
                    if (remainder > 0)
                    {
                        Item remainderItem = Instantiate(item);
                        remainderItem.currentStack = remainder;
                        list.Add(remainderItem);
                    }
                }
                else
                {
                    list.Add(item);
                }
            }
        }
        Debug.Log("Inventaire réarrangé");
        DisplayObject();
        Debug.Log("Inventaire affiché");
        // afficher toutes les listes
        foreach (var list in allLists)
        {
            Debug.Log("Liste : " + list.Count);
        }
    }
}
