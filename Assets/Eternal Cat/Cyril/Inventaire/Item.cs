using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : ScriptableObject
{
    public string itemName;
    public Sprite itemIcon;
    public bool isStackable;
    public int maxStack;
    public int currentStack;
    public string itemDescription;
}