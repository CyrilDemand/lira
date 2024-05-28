using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventoryData
{
    public List<PlanteItem> plantes;
    public List<Outil> outils;
    public List<Cuisine> cuisines;
    public List<Potion> potions;
    public List<TrousseauDeCles> trousseauDeCles;
    public List<QueteItem> quetes;
}

public class InventoryManager : MonoBehaviour
{
    public Inventory inventory;

    public void SaveInventory()
    {
        InventoryData data = new InventoryData();
        data.plantes = inventory.plantes;
        data.outils = inventory.outils;
        data.cuisines = inventory.cuisines;
        data.potions = inventory.potions;
        data.trousseauDeCles = inventory.trousseauDeCles;
        data.quetes = inventory.quetes;

        string json = JsonUtility.ToJson(data);
        PlayerPrefs.SetString("InventoryData", json);
    }

    public void LoadInventory()
    {
        if (PlayerPrefs.HasKey("InventoryData"))
        {
            string json = PlayerPrefs.GetString("InventoryData");
            InventoryData data = JsonUtility.FromJson<InventoryData>(json);

            inventory.plantes = data.plantes;
            inventory.outils = data.outils;
            inventory.cuisines = data.cuisines;
            inventory.potions = data.potions;
            inventory.trousseauDeCles = data.trousseauDeCles;
            inventory.quetes = data.quetes;

            //inventory.UpdateUI(); // Call UpdateUI to refresh the inventory UI
        }
    }
}