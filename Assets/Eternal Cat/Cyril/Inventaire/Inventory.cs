using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory/Inventory")]
public class Inventory : ScriptableObject
{
    [SerializeField]
    public List<PlanteItem> plantes = new List<PlanteItem>();
    [SerializeField]
    public List<Outil> outils = new List<Outil>();
    [SerializeField]
    public List<Ingredient> ingredients = new List<Ingredient>();
    [SerializeField]
    public List<Potion> potions = new List<Potion>();
    [SerializeField]
    public List<Cles> cles = new List<Cles>();
    [SerializeField]
    public List<QueteItem> quetes = new List<QueteItem>();

    public void AddItem(Item item)
    {
        if (item is PlanteItem)
        {
            plantes.Add((PlanteItem)item);
        }
        else if (item is Outil)
        {
            outils.Add((Outil)item);
        }
        else if (item is Ingredient)
        {
            ingredients.Add((Ingredient)item);
        }
        else if (item is Potion)
        {
            potions.Add((Potion)item);
        }
        else if (item is Cles)
        {
            cles.Add((Cles)item);
        }
        else if (item is QueteItem)
        {
            quetes.Add((QueteItem)item);
        }
    }

    public void RemoveItem(Item item)
    {
        if (item is PlanteItem)
        {
            plantes.Remove((PlanteItem)item);
        }
        else if (item is Outil)
        {
            outils.Remove((Outil)item);
        }
        else if (item is Ingredient)
        {
            ingredients.Remove((Ingredient)item);
        }
        else if (item is Potion)
        {
            potions.Remove((Potion)item);
        }
        else if (item is Cles)
        {
            cles.Remove((Cles)item);
        }
        else if (item is QueteItem)
        {
            quetes.Remove((QueteItem)item);
        }
    }

    // Additional methods for managing inventory can be added here

    // getAllItems
    public List<Item> GetAllItems()
    {
        List<Item> allItems = new List<Item>();
        allItems.AddRange(plantes);
        allItems.AddRange(outils);
        allItems.AddRange(ingredients);
        allItems.AddRange(potions);
        allItems.AddRange(cles);
        allItems.AddRange(quetes);
        return allItems;
    }


}
