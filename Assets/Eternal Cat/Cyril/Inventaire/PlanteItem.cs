using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Plante", menuName = "Inventory/Plante")]
public class PlanteItem : Item
{
    public enum PlanteType { Champignon, Fleur }
    public PlanteType planteType;
}

