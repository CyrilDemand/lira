using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class RoomNode
{
    // Nom de la salle (ou autre identifiant)
    [SerializeField]
    public string RoomName;

    // Liste des téléporteurs (par exemple, 2)
    [SerializeField]
    public List<GameObject> Teleporters;

    // Liste des salles connectées et leurs téléporteurs correspondants
    public Dictionary<RoomNode, List<Vector2>> ConnectedRooms;

    // Constructeur
    public RoomNode(string roomName, List<Vector2> teleporters)
    {
        ConnectedRooms = new Dictionary<RoomNode, List<Vector2>>();
    }

    // Ajouter une connexion entre cette salle et une autre
    public void ConnectTo(RoomNode otherRoom, List<Vector2> teleporters)
    {
        if (!ConnectedRooms.ContainsKey(otherRoom))
        {
            ConnectedRooms.Add(otherRoom, teleporters);
        }
    }

    // Pour plus de clarté lors du débogage
    public override string ToString()
    {
        return $"Room: {RoomName}, Teleporters: {Teleporters.Count}, Connections: {ConnectedRooms.Count}";
    }
}