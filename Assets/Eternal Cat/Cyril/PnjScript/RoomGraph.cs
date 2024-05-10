using System;
using System.Collections.Generic;
using UnityEngine;

public class RoomGraph
{
    // Liste de tous les téléporteurs disponibles dans la salle
    public Dictionary<GameObject, List<Teleporter>> RoomToTeleportersMap { get; private set; }

    public RoomGraph(GameObject[] allRooms, Teleporter[] allTeleporters)
    {
        RoomToTeleportersMap = new Dictionary<GameObject, List<Teleporter>>();

        // Initialisation de la map en regroupant les téléporteurs par pièce
        foreach (var room in allRooms)
        {
            RoomToTeleportersMap[room] = new List<Teleporter>();
        }

        foreach (var tp in allTeleporters)
        {
            if (tp.roomWhereIsTheTP != null && RoomToTeleportersMap.ContainsKey(tp.roomWhereIsTheTP))
            {
                RoomToTeleportersMap[tp.roomWhereIsTheTP].Add(tp);
            }
            else
            {
                Debug.LogWarning($"Le téléporteur {tp} n'est pas correctement associé à une salle.");
            }
        }
        
    }
}