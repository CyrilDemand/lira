using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Glossaire", menuName = "ScriptableObjects/Glossaire", order = 1)]
public class Glossaire : ScriptableObject
{
    public List<GlossaireEntry> entries;
}