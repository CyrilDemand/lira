using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<PlanteItem> plantes = new List<PlanteItem>();
    public List<Outil> outils = new List<Outil>();
    public List<Cuisine> cuisines = new List<Cuisine>();
    public List<Potion> potions = new List<Potion>();
    public List<TrousseauDeCles> trousseauDeCles = new List<TrousseauDeCles>();
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
        else if (item is Cuisine)
        {
            cuisines.Add((Cuisine)item);
        }
        else if (item is Potion)
        {
            potions.Add((Potion)item);
        }
        else if (item is TrousseauDeCles)
        {
            trousseauDeCles.Add((TrousseauDeCles)item);
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
        else if (item is Cuisine)
        {
            cuisines.Remove((Cuisine)item);
        }
        else if (item is Potion)
        {
            potions.Remove((Potion)item);
        }
        else if (item is TrousseauDeCles)
        {
            trousseauDeCles.Remove((TrousseauDeCles)item);
        }
        else if (item is QueteItem)
        {
            quetes.Remove((QueteItem)item);
        }
    }

    // Additional methods for managing inventory can be added here
}
